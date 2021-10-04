using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class AddressController : Controller
    {
        private IAddressRepository _addressRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private LoginCollaborator _loginCollaborator;
        private ILogger<AddressController> _logger;
        public AddressController(IAddressRepository addressRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, LoginCollaborator loginCollaborator, ILogger<AddressController> logger)
        {
            _addressRepository = addressRepository;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _loginCollaborator = loginCollaborator;
            _logger = logger;
        }
        [HttpGet]
        public IActionResult Index(int? page)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            var address = _addressRepository.GetAllAddress(page, loginUsuario.IdTecAssistance);
            _logger.LogInformation("Listando endereço(s) da assistência técnica");
            return View(address);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
            return View();
        }

        [HttpPost]
        public IActionResult Create([FromForm]Models.Address address)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            address.IdTecAssistance = loginUsuario.IdTecAssistance;

            address.ZipCode = address.ZipCode.Replace("-", string.Empty);

            if (ModelState.IsValid)
            {
                _addressRepository.Create(address);
                @TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo endereço cadastrado");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Erro ao cadastrar endereço");
                return View(address);
            }
        }

    [HttpGet]
        public IActionResult Update(int id)
        {
            Address address = _addressRepository.GetAddress(id);
            _logger.LogInformation("Buscando endereço pelo id");
            return View(address);
        }

        [HttpPost]
        public IActionResult Update([FromForm]Models.Address address, int id)
        {
            address.IdTecAssistance = _loginCollaborator.GetCollaborator().IdTecAssistance;

            address.ZipCode = address.ZipCode.Replace("-", string.Empty);

            if (ModelState.IsValid)
            {
                _addressRepository.Update(address);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando endereço da assistência técnica");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                _logger.LogError("Erro ao atualizar endereço");
                return View(address);
            }
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            Address address = _addressRepository.GetAddress(id);
            _addressRepository.Delete(id);
            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Endereço excluído");
            return RedirectToAction(nameof(Index));
        }

    }
}