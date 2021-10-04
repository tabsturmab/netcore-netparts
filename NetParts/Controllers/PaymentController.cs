using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using NetParts.Controllers.Base;
using NetParts.Libraries.Cookie;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Login;
using NetParts.Libraries.Manager.Frete;
using NetParts.Libraries.Manager.Pagamento.PagarMe;
using NetParts.Libraries.ShoppingCart;
using NetParts.Libraries.Text;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using NetParts.Models.ViewModels.Pagamento;
using NetParts.Repositories.Contracts;
using PagarMe;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NetParts.Models.Constant;
using Address = NetParts.Models.Address;
using NetParts.Libraries.Email;

namespace NetParts.Controllers
{
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    [ValidateCookiePagamentoController]
    public class PaymentController : BaseController
    {
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private Cookie _cookie;
        private GerenciarPagarMe _gerenciarPagarMe;
        private IOrderRepository _orderRepository;
        private IOrderSituationRepository _orderSituationRepository;
        private IOrderAdvertisementRepository _orderAdvertisementRepository;
        private IAdvertisementRepository _advertisementRepository;
        private ManageEmail _manageEmail;
        public PaymentController(ManageEmail manageEmail, IOrderRepository orderRepository,
            IOrderSituationRepository orderSituationRepository, IOrderAdvertisementRepository orderAdvertisementRepository, GerenciarPagarMe gerenciarPagarMe,LoginCollaborator loginCollaborator, Cookie cookie, CookieShoppingCart shoppingCart, IAddressRepository addressRepository, IAdvertisementRepository advertisementRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CookieFrete cookieFrete, ITechnicalAssistanceRepository technicalAssistanceRepository) : base(loginCollaborator, shoppingCart, addressRepository, advertisementRepository, mapper, wscorreios, cookieFrete)
        {
            _manageEmail = manageEmail;
            _orderRepository = orderRepository;
            _orderSituationRepository = orderSituationRepository;
            _cookie = cookie;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _gerenciarPagarMe = gerenciarPagarMe;
            _advertisementRepository = advertisementRepository;
            _orderAdvertisementRepository = orderAdvertisementRepository;
        }
        [HttpGet]
        public IActionResult Index()
        {
            List<ProductItem> productItemComplete = LoadProductDb();
            ValorPrazoFrete frete = ObterFrete();

            ViewBag.Frete = frete;
            ViewBag.Advertisement = productItemComplete;
            ViewBag.Parcelamentos = CalcularParcelamento(productItemComplete);

            return View("Index");
        }
        [HttpPost]
        public IActionResult Index([FromForm] IndexViewModel indexViewModel)
        {
            if (ModelState.IsValid)
            {
                Address enderecoEntrega = ObterEndereco(false);
                Address enderecoFatura = ObterEndereco(true);
                ValorPrazoFrete frete = ObterFrete();
                List<ProductItem> advertisement = LoadProductDb();
                Parcelamento parcela = BuscarParcelamento(advertisement, indexViewModel.Parcelamento.Numero);

                try
                {
                    Transaction transaction = _gerenciarPagarMe.GerarPagCartaoCredito(indexViewModel.CartaoCredito, parcela, enderecoFatura, enderecoEntrega, frete, advertisement);
                    Order order = ProcessOrder(advertisement, transaction);

                    return new RedirectToActionResult("Index", "Order", new { id = order.IdOrder });
                }
                catch (PagarMeException e)
                {
                    TempData["MSG_E"] = MontarMensagensDeErro(e);

                    return Index();
                }
            }
            else
            {
                return Index();
            }
        }
        public IActionResult BoletoBancario()
        {
            Address enderecoEntrega = ObterEndereco(false);
            ValorPrazoFrete frete = ObterFrete();
            List<ProductItem> advertisement = LoadProductDb();
            var valorTotal = ObterValorTotalCompra(advertisement);

            try
            {
                Transaction transaction = _gerenciarPagarMe.GerarBoleto(valorTotal, advertisement, enderecoEntrega, frete);
                Order order = ProcessOrder(advertisement, transaction);
                return new RedirectToActionResult("Index", "Order", new { id = order.IdOrder });
            }
            catch (PagarMeException e)
            {
                TempData["MSG_E"] = MontarMensagensDeErro(e);
                return RedirectToAction(nameof(Index));
            }
        }
        private Order ProcessOrder(List<ProductItem> advertisement, Transaction transaction)
        {
            TransacaoPagarMe transacaoPagarMe;
            Order order;

            LowStock(advertisement);

            SaveOrder(advertisement, transaction, out transacaoPagarMe, out order);

            SaveOrderSituation(advertisement, transacaoPagarMe, order);

            _cookieShoppingCart.RemoveAll();

            TechnicalAssistance technicalAssistance = _technicalAssistanceRepository.GetTechnicalAssistance(order.IdTecAssistance.Value);
            _manageEmail.SendOrderData(order, technicalAssistance);

            TechnicalAssistance technicalAssistanceSale = _technicalAssistanceRepository.GetTechnicalAssistance(order.OrderAdvertisement.First().Advertisement.IdTecAssistance);
            _manageEmail.SendOrderDataSale(order.OrderAdvertisement.First(),technicalAssistanceSale);

            return order;
        }
        private void LowStock(List<ProductItem> advertisement)
        {
            foreach (var product in advertisement)
            {
                Advertisement productDB = _advertisementRepository.GetAdvertisement(product.IdAdvert);
                productDB.Amount -= product.QuantityProduct;

                _advertisementRepository.Update(productDB);
            }
        }
        private void SaveOrderSituation(List<ProductItem> advertisement, TransacaoPagarMe transacaoPagarMe, Order order)
        {
            TransactionAdvertisement ta = new TransactionAdvertisement {Transaction = transacaoPagarMe, Advertisement = advertisement};
            OrderSituation orderSituation = _mapper.Map<Order, OrderSituation>(order);
            orderSituation.Situation = OrderSituationConstant.PEDIDO_REALIZADO;

            _orderSituationRepository.Create(orderSituation);
        }
        private void SaveOrder(List<ProductItem> advertisement, Transaction transaction,  out TransacaoPagarMe transacaoPagarMe, out Order order)
        {
            transacaoPagarMe = _mapper.Map<TransacaoPagarMe>(transaction);
            order = _mapper.Map<TransacaoPagarMe, Order>(transacaoPagarMe);

            order.Situation = OrderSituationConstant.PEDIDO_REALIZADO;

            _orderRepository.Create(order);

            var idOrder = order.IdOrder;

            foreach (ProductItem item in advertisement)
            {
                OrderAdvertisement orderAdvertisement = new OrderAdvertisement();
                orderAdvertisement.IdAdvert = item.IdAdvert;
                orderAdvertisement.IdOrder = idOrder;

                _orderAdvertisementRepository.Create(orderAdvertisement);
            }
        }
        private Parcelamento BuscarParcelamento(List<ProductItem> advertisement, int numero)
        {
            return _gerenciarPagarMe.CalcularPagamentoParcelado(ObterValorTotalCompra(advertisement)).Where(a => a.Numero == numero).First();
        }
        private Address ObterEndereco(bool endFat)
        {
            Address enderecoEntrega = null;
            var enderecoEntregaId = int.Parse(_cookie.Consult("Cart.EnderecoCookie", false));

            Collaborator collaborator = _loginCollaborator.GetCollaborator();
            var enderecoFatura = _technicalAssistanceRepository.GetTechnicalAssistance(collaborator.IdTecAssistance).Address.First();

            if (enderecoEntregaId == enderecoFatura.IdAddress || endFat)
            {
                enderecoEntrega = enderecoFatura;
            }
            else
            {
                enderecoEntrega = _addressRepository.GetAddress(enderecoEntregaId);
            }
            return enderecoEntrega;
        }
        private ValorPrazoFrete ObterFrete()
        {
            var enderecoEntrega = ObterEndereco(false);
            int cep = int.Parse(Mascara.Remover(enderecoEntrega.ZipCode));
            var tipoFreteSelecionadoPeloUsuario = _cookie.Consult("Cart.TipoFrete", false);
            var carrinhoHash = GerarHash(_cookieShoppingCart.Consult());

            Frete frete = _cookieFrete.Consult().Where(a => a.CEP == cep && a.CodCart == carrinhoHash).FirstOrDefault();

            if (frete != null)
            {
                return frete.ListValues.Where(a => a.TipoFrete == tipoFreteSelecionadoPeloUsuario).FirstOrDefault();
            }
            return null;
        }
        private decimal ObterValorTotalCompra(List<ProductItem> advertisement)
        {
            ValorPrazoFrete frete = ObterFrete();
            decimal total = Convert.ToDecimal(frete.Valor);

            foreach (var product in advertisement)
            {
                total += (Convert.ToDecimal(product.Price * product.QuantityProduct));
            }
            return total;
        }
        private string MontarMensagensDeErro(PagarMeException e)
        {
            StringBuilder sb = new StringBuilder();

            if (e.Error.Errors.Count() > 0)
            {
                sb.Append("Erro no pagamento: ");
                foreach (var erro in e.Error.Errors)
                {
                    sb.Append("- " + erro.Message + "<br />");
                }
            }
            return sb.ToString();
        }
        private List<SelectListItem> CalcularParcelamento(List<ProductItem> advertisement)
        {
            var total = ObterValorTotalCompra(advertisement);
            var parcelamento = _gerenciarPagarMe.CalcularPagamentoParcelado(total);

            return parcelamento.Select(a => new SelectListItem(
                String.Format(
                    "{0}x {1} {2} - TOTAL: {3}",
                    a.Numero, a.ValorPorParcela.ToString("C"), (a.Juros) ? "c/ juros" : "s/ juros", a.Valor.ToString("C")
                ),
                a.Numero.ToString()
            )).ToList();
        }
    }
}