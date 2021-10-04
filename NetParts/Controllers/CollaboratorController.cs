using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Text;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;
using NetParts.Libraries.Cookie;
using NetParts.Models;
using Newtonsoft.Json;
using NetParts.Libraries.Security;
using NetParts.Libraries.Login;

namespace NetParts.Controllers
{
    public class CollaboratorController : Controller
    {
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private ICollaboratorRepository _collaboratorRepository;
        private LoginCollaborator _loginCollaborator;
        private string Key = "Cookie.Register";
        private Cookie _cookie;
        private ManageEmail _manageEmail;
        private ILogger<CollaboratorController> _logger;

        public CollaboratorController(ITechnicalAssistanceRepository technicalAssistanceRepository, ICollaboratorRepository collaboratorRepository, LoginCollaborator loginCollaborator, ManageEmail manageEmail, Cookie cookie, ILogger<CollaboratorController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _collaboratorRepository = collaboratorRepository;
            _loginCollaborator = loginCollaborator;
            _cookie = cookie;
            _manageEmail = manageEmail;
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
        public IActionResult Create(Collaborator collaborator)
        {
            string valor = _cookie.Consult(Key, true);

            if (valor == null)
                return new StatusCodeResult(403);

            TechnicalAssistance technical = JsonConvert.DeserializeObject<TechnicalAssistance>(valor);

            collaborator.IdTecAssistance = technical.IdTecAssistance.Value;

            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                collaborator.Cpf = collaborator.Cpf.Replace(".", string.Empty).Replace("-", string.Empty);

                Hash hash = new Hash();
                string password = KeyGenerator.GetUniqueKey(10);
                collaborator.Password = hash.CriptografarSenha(password);
                _collaboratorRepository.Create(collaborator);
                _manageEmail.SendCollaboratorPassword(collaborator, password);
                TempData["MSG_S"] = Msg.MSG_S004;
                _logger.LogInformation("Novo colaborador cadastrado");
                return RedirectToAction("Create", "Address", new { area = "" });
            }
            _logger.LogError("Informações inválidas");
            return View();
        }
        [HttpGet]
        public IActionResult UpdatePassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult UpdatePassword(Collaborator collaborator)
        {
            Hash hash = new Hash();

            Collaborator collaboratorDB = _collaboratorRepository.GetCollaborator(_loginCollaborator.GetCollaborator().IdCollaborator.Value);
            collaborator.IdTecAssistance = _loginCollaborator.GetCollaborator().IdTecAssistance;

            ModelState.Remove("FirstName");
            ModelState.Remove("LastName");
            ModelState.Remove("Cpf");
            ModelState.Remove("Email");
            ModelState.Remove("TypeCollaborator");

            if (ModelState.IsValid)
            {
                string password = hash.CriptografarSenha(collaborator.Password);
                collaboratorDB.Password = password;
                _collaboratorRepository.UpdatePassword(collaboratorDB);
                _loginCollaborator.Login(collaboratorDB);
                _cookie.RemoveAll();
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Alteração da senha");
                return RedirectToAction("Login", "Home", new { area = "Collaborator" });
            }
            _logger.LogError("Informações inválidas");
            return View();
        }
    }
}