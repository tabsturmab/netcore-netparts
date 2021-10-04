using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Cookie;
using NetParts.Libraries.Email;
using NetParts.Libraries.File;
using NetParts.Libraries.Lang;
using NetParts.Models;
using NetParts.Models.ViewModels;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Controllers
{
    public class TechnicalAssistanceController : Controller
    {
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IAddressRepository _addressRepository;
        private string Key = "Cookie.Register";
        private Cookie _cookie;
        private static IConfiguration _configuration;
        private IArchiveRepository _archiveRepository;
        private ILogger<TechnicalAssistanceController> _logger;
        private ManageEmail _manageEmail;

        public TechnicalAssistanceController(ITechnicalAssistanceRepository technicalAssistanceRepository,IAddressRepository addressRepository, Cookie cookie, ManageEmail manageEmail, IConfiguration configuration, IArchiveRepository archiveRepository, ILogger<TechnicalAssistanceController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _configuration = configuration;
            _addressRepository = addressRepository;
            _cookie = cookie;
            _archiveRepository = archiveRepository;
            _manageEmail = manageEmail;
            _logger = logger;
        }
        public IActionResult Index(int? page)
        {
            return View();
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(TechnicalAssistanceArchive technicalAssistanceArchive)
        {
            if (ModelState.IsValid)
            {
                technicalAssistanceArchive.technicalAssistance.Cnpj = technicalAssistanceArchive.technicalAssistance.Cnpj.Replace(".", string.Empty).Replace("/", string.Empty).Replace("-", string.Empty);
                technicalAssistanceArchive.technicalAssistance.Phone = technicalAssistanceArchive.technicalAssistance.Phone.Replace("(", string.Empty).Replace(")", string.Empty).Replace(" ", string.Empty).Replace("-", string.Empty);

                ArchiveController archiveController = new ArchiveController(_configuration);

                technicalAssistanceArchive.technicalAssistance.EnabledDisabled = false;
                technicalAssistanceArchive.technicalAssistance.DateRegister = DateTime.Now;

                _technicalAssistanceRepository.Create(technicalAssistanceArchive.technicalAssistance);

                foreach (IFormFile file in technicalAssistanceArchive.ImageFile)
                {
                    var archive = await archiveController.Store(file, technicalAssistanceArchive.technicalAssistance.IdTecAssistance.Value);
                    _archiveRepository.Create(archive);
                }
                string Valor = JsonConvert.SerializeObject(technicalAssistanceArchive.technicalAssistance);
                _cookie.Create(Key, Valor);

                _manageEmail.RegistrationAssistance(technicalAssistanceArchive.technicalAssistance, 3, 5);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Nova assistência técnica cadastrada");
                return RedirectToAction("Create", "Collaborator", new { area = "" });
            }
            else
            {
                return View();
            }
        }
    }
}