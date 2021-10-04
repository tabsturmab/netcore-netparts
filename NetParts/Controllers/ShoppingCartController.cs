using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetParts.Controllers.Base;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Libraries.Manager.Frete;
using NetParts.Libraries.ShoppingCart;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ProductAggregator;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Controllers
{
    public class ShoppingCartController : BaseController
    {
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private LoginCollaborator _loginCollaborator;
        public ShoppingCartController(LoginCollaborator loginCollaborator, CookieShoppingCart shoppingCart, ITechnicalAssistanceRepository technicalAssistanceRepository, IAddressRepository addressRepository, IAdvertisementRepository advertisementRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CookieFrete cookieFrete) : base(loginCollaborator, shoppingCart, addressRepository, advertisementRepository, mapper, wscorreios, cookieFrete)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _loginCollaborator = loginCollaborator;
        }
        public IActionResult Index()
        {
            List<ProductItem> productItemComplete = LoadProductDb();

            return View(productItemComplete);
        }

        public IActionResult InStock()
        {
            List<ProductItem> productItemComplete = LoadProductDb();

            foreach (var advert in productItemComplete)
            {
                if (advert.Amount <= 0)
                {
                    ViewBag.MSG_E = Msg.MSG_E008;
                    return View("Index", productItemComplete);
                }
                if (advert.Amount < advert.QuantityProduct)
                {
                    ViewBag.MSG_E = Msg.MSG_E008;
                    return View("Index", productItemComplete);
                }
            }
            return RedirectToAction(nameof(AddAddress));
        }

        public IActionResult AddItem(int id)
        {
            var collaborator = _loginCollaborator.GetCollaborator();

            if (collaborator == null)
            {
                return View("/Areas/Collaborator/Views/Home/Login.cshtml");
            }

            Advertisement advertisement = _advertisementRepository.GetAdvertisement(id);

            TechnicalAssistance technical = _technicalAssistanceRepository.GetTechnicalAssistance(collaborator.IdTecAssistance);

            if (!technical.EnabledDisabled)
            {
                return View("Disabled");
            }

            if (advertisement == null)
            {
                return View("ItemNoExit");
            }

            if (advertisement.IdTecAssistance == collaborator.IdTecAssistance)
            {
                return View("ItemSameAssistance");
            }

            var listProductItemsCookie = _cookieShoppingCart.Consult();

            if (listProductItemsCookie.Count > 0)
            {
                var anuncioVenda = _advertisementRepository.GetAdvertisement(listProductItemsCookie.First().IdAdvert);

                if (anuncioVenda.IdTecAssistance != advertisement.IdTecAssistance)
                {
                    return View("ItemSameAdvert");
                }
            }

            var item = new ProductItem() { IdAdvert = id, IdTecAssistance = advertisement.IdTecAssistance, QuantityProduct = 1 };
            _cookieShoppingCart.Create(item);
            return RedirectToAction(nameof(Index));
        }
        public IActionResult ChangeQuantity(int id, int quantity)
        {
            Advertisement advertisement = _advertisementRepository.GetAdvertisement(id);
            if (quantity < 1)
            {
                return BadRequest(new { message = Msg.MSG_E007 });
            }
            else if (quantity > advertisement.Amount)
            {
                return BadRequest(new { message = Msg.MSG_E008 });
            }
            else
            {
                var item = new ProductItem() {IdAdvert = id, QuantityProduct = quantity};
                _cookieShoppingCart.Update(item);
                return Ok(new { mensagem = Msg.MSG_S001 });
            }
        }
        public IActionResult RemoveItem(int id)
        {
            _cookieShoppingCart.Remove(new ProductItem() {IdAdvert = id});
            return RedirectToAction(nameof(Index));
        }

        [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador, CollaboratorTypeConstant.Comum })]
        public IActionResult AddAddress(int? page)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            var address = _technicalAssistanceRepository.GetTechnicalAssistance(loginUsuario.IdTecAssistance);

            ViewBag.Products = LoadProductDb();

            ViewBag.loginUsuario = loginUsuario;
            ViewBag.address = address.Address;
            return View();
        }
        public async Task<IActionResult> CalcularFrete(int cepDestino)
        {
            try
            {
                Frete frete = _cookieFrete.Consult().Where(a => a.CEP == cepDestino && a.CodCart == GerarHash(_cookieShoppingCart.Consult())).FirstOrDefault();
                if (frete != null)
                {
                    return Ok(frete);
                }
                else
                {
                    List<ProductItem> products = LoadProductDb();

                    CalcularPacote calcularPacote = new CalcularPacote();

                    List<Pacote> pacotes = calcularPacote.CalcularPacotesDeProdutos(products);

                    ValorPrazoFrete valorPAC = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.PAC, pacotes, products);
                    ValorPrazoFrete valorSEDEX = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX, pacotes, products);
                    ValorPrazoFrete valorSEDEX10 = await _wscorreios.CalcularFrete(cepDestino.ToString(), TipoFreteConstant.SEDEX10, pacotes, products);

                    List<ValorPrazoFrete> list = new List<ValorPrazoFrete>();
                    if (valorPAC != null) list.Add(valorPAC);
                    if (valorSEDEX != null) list.Add(valorSEDEX);
                    if (valorSEDEX10 != null) list.Add(valorSEDEX10);

                    frete = new Frete()
                    {
                        CEP = cepDestino,
                        CodCart = GerarHash(_cookieShoppingCart.Consult()),
                        ListValues = list
                    };

                    _cookieFrete.Create(frete);

                    return Ok(frete);
                }
            }
            catch (Exception e)
            {
                return BadRequest(e);
            }
        }
    }
}