using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Cookie;
using NetParts.Models;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Controllers
{
    public class AddressController : Controller
    {
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IAddressRepository _addressRepository;
        private string Key = "Cookie.Register";
        private Cookie _cookie;
        private ILogger<AddressController> _logger;
        public AddressController(ITechnicalAssistanceRepository technicalAssistanceRepository, IAddressRepository addressRepository, ManageEmail manageEmail, Cookie cookie, ILogger<AddressController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _addressRepository = addressRepository;
            _cookie = cookie;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Address address)
        {
            string valor = _cookie.Consult(Key, true);
            TechnicalAssistance technical = JsonConvert.DeserializeObject<TechnicalAssistance>(valor);

            address.IdTecAssistance = technical.IdTecAssistance.Value;

            if (ModelState.IsValid)
            {
                address.ZipCode = address.ZipCode.Replace("-", string.Empty);

                _addressRepository.Create(address);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo endereço cadastrado");
                return RedirectToAction("Register", "Home", new { area = "" });
            }
            return View();
        }
    }
}