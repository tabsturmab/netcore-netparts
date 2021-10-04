using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Cookie;
using NetParts.Libraries.Email;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Libraries.Security;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ProductAggregator;
using NetParts.Models.ViewModels;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;
using X.PagedList;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    public class HomeController : Controller
    {
        private IAddressRepository _addressRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private ICollaboratorRepository _collaboratorRepository;
        private IOrderAdvertisementRepository _orderAdvertisementRepository;
        private IOrderRepository _orderRepository;
        private LoginCollaborator _loginCollaborator;
        private ManageEmail _manageEmail;
        private ILogger<HomeController> _logger;
        private Cookie _cookie;

        public HomeController(IAddressRepository addressRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, ICollaboratorRepository collaboratorRepository, IOrderAdvertisementRepository orderAdvertisementRepository, IOrderRepository orderRepository, LoginCollaborator loginCollaborator, ManageEmail manageEmail, ILogger<HomeController> logger, Cookie cookie)
        {
            _addressRepository = addressRepository;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _collaboratorRepository = collaboratorRepository;
            _orderAdvertisementRepository = orderAdvertisementRepository;
            _orderRepository = orderRepository;
            _manageEmail = manageEmail;
            _loginCollaborator = loginCollaborator;
            _logger = logger;
            _cookie = cookie;
        }
        [HttpGet]
        public IActionResult Login()
        {
            _logger.LogInformation("Entrando na página de login");
            return View();
        }
        [HttpPost]
        public IActionResult Login([FromForm]Models.Collaborator collaborator, string returnUrl = null)
        {
           Hash hash = new Hash();

            _logger.LogInformation("Pegando o colaborador pelo email e senha");
            Models.Collaborator collaboratorBD = _collaboratorRepository.Login(collaborator.Email, hash.CriptografarSenha(collaborator.Password));

            if (collaboratorBD != null)
            {
                _loginCollaborator.Login(collaboratorBD);
                _logger.LogInformation("Verificando informações do colaborador");
                return new RedirectResult(Url.Action(nameof(Panel)));
            }
            TempData["MSG_E"] = Msg.MSG_S016;
            _logger.LogError("Erro ao buscar o colaborador");
            return View();
        }
        [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador, CollaboratorTypeConstant.Comum })]
        [ValidateHttpReferer]
        public IActionResult Logout()
        {
            _loginCollaborator.Logout();
            _logger.LogInformation("Efetuando Logout do colaborador");
            _cookie.RemoveAll();
            return RedirectToAction("Index", "Home", new {area = ""});
        }

        [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador, CollaboratorTypeConstant.Comum })]
        [ValidateHttpReferer]
        public IActionResult Panel()
        {
            Models.Collaborator collaborator = _loginCollaborator.GetCollaborator();
            IPagedList<OrderAdvertisement> list = _orderAdvertisementRepository.GetAllOrderAdvertisements(collaborator.IdTecAssistance, null, null);

            Dictionary<String, Grafico> valoresOrder = new Dictionary<String, Grafico>();

            var random = new Random();

            List<string> valores = new List<string>();
            List<string> labels = new List<string>();
            List<string> cores = new List<string>();

            foreach (OrderAdvertisement order in list)
            {
                var dadosProduto = JsonConvert.DeserializeObject<TransacaoPagarMe>(order.Order.DataTransaction);

                foreach (var item in dadosProduto.Item)
                {
                    if (valoresOrder.ContainsKey(item.Title))
                    {
                        valoresOrder[order.Advertisement.Product.Description].valor += item.Quantity;
                    }
                    else
                    {
                        string cor = String.Format("'#{0:X6}'", random.Next(0x1000000));
                        string label = "'" + order.Advertisement.Product.Description + "'";
                        int valor = item.Quantity;

                        Grafico grafico = new Grafico();
                        grafico.cor = cor;
                        grafico.label = label;
                        grafico.valor = valor;

                        valoresOrder.Add(order.Advertisement.Product.Description, grafico);
                    }
                }
            }

            foreach (var grafi in valoresOrder)
            {
                valores.Add(grafi.Value.valor.ToString());
                labels.Add(grafi.Value.label);
                cores.Add(grafi.Value.cor);
            }

            ViewBag.Valores = String.Join(", ", valores);
            ViewBag.Labels = String.Join(", ", labels);
            ViewBag.Cores = String.Join(", ", cores);

            _logger.LogInformation("Listagem do painel de controle do colaborador");
            return View();
        }

        [HttpGet]
        public IActionResult RecoverPassword()
        {
            return View();
        }

        [HttpPost]
        public IActionResult RecoverPassword([FromForm]Models.Collaborator collaborator)
        {
            var collaboratorBD = _collaboratorRepository.GetCollaboratorEmail(collaborator.Email);

            if (collaboratorBD != null && collaboratorBD.Count > 0)
            {
                string idCrip = Base64Cipher.Base64Encode(collaboratorBD.First().IdCollaborator.ToString());
                _manageEmail.SendResetPassword(collaboratorBD.First(), idCrip);

                TempData["MSG_S"] = Msg.MSG_S006;
                ModelState.Clear();
            }
            else
            {
                TempData["MSG_E"] = Msg.MSG_E014;
            }

            return View();
        }

        [HttpGet]
        public IActionResult CreateNewPassword(string id)
        {
            try
            {
                var idCollaboratorDecrip = Base64Cipher.Base64Decode(id);
                int idCollaborator;
                if (!int.TryParse(idCollaboratorDecrip, out idCollaborator))
                {
                    TempData["MSG_E"] = Msg.MSG_E015;
                }
            }
            catch(FormatException e)
            {
                TempData["MSG_E"] = Msg.MSG_E015;
            }
            return View();
        }

        [HttpPost]
        public IActionResult CreateNewPassword([FromForm]Models.Collaborator collaborator, string id)
        {
            Hash hash = new Hash();

            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Cpf");
            ModelState.Remove("Email");
            ModelState.Remove("TypeCollaborator");

            if (ModelState.IsValid)
            {
                int idCollaborator;
                try
                {
                    var idCollaboratorDecrip = Base64Cipher.Base64Decode(id);
                    if (!int.TryParse(idCollaboratorDecrip, out idCollaborator))
                    {
                        TempData["MSG_E"] = Msg.MSG_E015;
                        return View();
                    }
                }
                catch (FormatException e)
                {
                    TempData["MSG_E"] = Msg.MSG_E015;
                    return View();
                }
                var collaboratorDB = _collaboratorRepository.GetCollaborator(idCollaborator);
                if (collaboratorDB != null)
                {
                    string password = hash.CriptografarSenha(collaborator.Password);
                    collaboratorDB.Password = password;

                    _collaboratorRepository.UpdatePassword(collaboratorDB);
                    TempData["MSG_S"] = Msg.MSG_S007;
                }
            }
            return View();
        }

        [HttpGet]
        public IActionResult AddAdress()
        {
            return View();
        }
        [HttpPost]
        public IActionResult AddAdress(Address address, string returnUrl = null)
        {
            _logger.LogInformation("Obtendo id do colaborador");
            var loginUsuario = _loginCollaborator.GetCollaborator();
            address.IdTecAssistance = loginUsuario.IdTecAssistance;

            if (ModelState.IsValid)
            {
                _addressRepository.Create(address);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo endereço cadastrado");
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("AddAddress", "ShoppingCart", new { area = "" });
            }
            else
            {
                _logger.LogError("Erro ao cadastrar endereço");
                return View(address);
            }
        }
        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );
            return LocalRedirect(returnUrl);
        }
    }
}