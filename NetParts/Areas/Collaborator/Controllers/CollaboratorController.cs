using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Libraries.Security;
using NetParts.Libraries.Text;
using NetParts.Models.Constant;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class CollaboratorController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private ICollaboratorRepository _collaboratorRepository;
        private ManageEmail _manageEmail;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IAddressRepository _addressRepository;
        private ILogger<CollaboratorController> _logger;

        public CollaboratorController(LoginCollaborator loginCollaborator, ICollaboratorRepository collaboratorRepository, ManageEmail manageEmail, ITechnicalAssistanceRepository technicalAssistanceRepository, IAddressRepository addressRepository, ILogger<CollaboratorController> logger)
        {
            _loginCollaborator = loginCollaborator;
            _collaboratorRepository = collaboratorRepository;
            _manageEmail = manageEmail;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _addressRepository = addressRepository;
            _logger = logger;
        }

        public IActionResult Index(int? page)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            
            IPagedList<Models.Collaborator> collaborators = _collaboratorRepository.GetAllCollaborators(page, loginUsuario.IdTecAssistance);
            _logger.LogInformation("Listando informações do colaborador"); 
            return View(collaborators);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]Models.Collaborator collaborator)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            collaborator.IdTecAssistance = loginUsuario.IdTecAssistance;

            collaborator.Cpf = collaborator.Cpf.Replace(".", string.Empty).Replace("-", string.Empty);

            ModelState.Remove("Password");

            if (ModelState.IsValid)
            {
                Hash hash = new Hash();
                string password = KeyGenerator.GetUniqueKey(10);
                collaborator.Password = hash.CriptografarSenha(password);
                _collaboratorRepository.Create(collaborator);
                _manageEmail.SendCollaboratorPassword(collaborator, password);
                TempData["MSG_S"] = Msg.MSG_S004;
                _logger.LogInformation("Novo colaborador cadastrado");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Erro ao criar o colaborador");
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult GeneratePassword(int id)
        {
            Hash hash = new Hash();

            Models.Collaborator collaborator = _collaboratorRepository.GetCollaborator(id);
            string password = KeyGenerator.GetUniqueKey(10);
            collaborator.Password = hash.CriptografarSenha(password);
            _collaboratorRepository.UpdatePassword(collaborator);
            _manageEmail.SendCollaboratorPassword(collaborator, password);
            TempData["MSG_S"] = Msg.MSG_S003;
            _logger.LogInformation("Gerando senha, hash da senha e enviando e-mail ao colaborador");
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Update(int id)
        {
            Models.Collaborator collaborator = _collaboratorRepository.GetCollaborator(id);
            _logger.LogInformation("Buscando colaborador pelo id");
            return View(collaborator);
        }

        [HttpPost]
        public IActionResult Update([FromForm]Models.Collaborator collaborator, int id)
        {
            collaborator.IdTecAssistance = _loginCollaborator.GetCollaborator().IdTecAssistance;

            ModelState.Remove("Password");
            if (ModelState.IsValid)
            {
                _collaboratorRepository.Update(collaborator);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando informações do colaborador");
                return RedirectToAction(nameof(Index));
            }
            _logger.LogError("Informações inválidas");
            return View();
        }

        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            _collaboratorRepository.Delete(id);

            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Colaborador excluído");
            return RedirectToAction(nameof(Index));
        }
    }
}