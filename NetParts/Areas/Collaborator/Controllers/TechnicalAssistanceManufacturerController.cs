using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Administrador })]
    public class TechnicalAssistanceManufacturerController : Controller
    {
        private ITechnicalAssistanceManufacturerRepository _technicalAssistanceManufacturerRepository;
        private IManufacturerRepository _manufacturerRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private ILogger<TechnicalAssistanceManufacturerController> _logger;
        public TechnicalAssistanceManufacturerController(ITechnicalAssistanceManufacturerRepository technicalAssistanceManufacturerRepository, IManufacturerRepository manufacturerRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, ILogger<TechnicalAssistanceManufacturerController> logger)
        {
            _technicalAssistanceManufacturerRepository = technicalAssistanceManufacturerRepository;
            _manufacturerRepository = manufacturerRepository;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _logger = logger;
        }
        public IActionResult Index(int? page, string search)
        {
            var technicalAssistanceManufacturer = _technicalAssistanceManufacturerRepository.GetAllTechnicalAssistanceManufacturers(page, search);
            _logger.LogInformation("Listando atribuição entre a assistência e fabricante");
            return View(technicalAssistanceManufacturer);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Create(TechnicalAssistanceManufacturer technicalAssistanceManufacturer)
        {

            if (ModelState.IsValid)
            {
                _technicalAssistanceManufacturerRepository.Create(technicalAssistanceManufacturer);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Nova atribuição entre assistência técnica e fabricante cadastrada");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
                _logger.LogError("Erro ao cadastrar atribuição");
                return View(technicalAssistanceManufacturer);
            }
        }
        [HttpGet]
        public IActionResult Update(int idTecAss, int idManu)
        {
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
                TechnicalAssistanceManufacturer technicalAssistanceManufacturer = _technicalAssistanceManufacturerRepository.GetTechnicalAssistanceManufacturer(idTecAss, idManu);
                _logger.LogInformation("Buscando atribuição pelo id");
                return View(technicalAssistanceManufacturer);
            }
        }
        [HttpPost]
        public IActionResult Update(TechnicalAssistanceManufacturer technicalAssistanceManufacturer)
        {
            if (ModelState.IsValid)
            {
                _technicalAssistanceManufacturerRepository.Update(technicalAssistanceManufacturer);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando atribuição entre assistência e fabricante");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
                _logger.LogError("Erro ao atualizar atribuição entre assistência e fabricante");
                return View(technicalAssistanceManufacturer);
            }
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int idTecAss, int idManu)
        {
            TechnicalAssistanceManufacturer technicalAssistanceManufacturer = _technicalAssistanceManufacturerRepository.GetTechnicalAssistanceManufacturer(idTecAss, idManu);
            _technicalAssistanceManufacturerRepository.Delete(idTecAss, idManu);
            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Atribuição excluída");
            return RedirectToAction(nameof(Index));
        }
    }
}