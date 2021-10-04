using System;
using System.Collections.Generic;
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
   // [CollaboratorAultorization(CollaboratorTypeConstant.Administrador)]
    public class ManufacturerController : Controller
    {
        List<string> myList = new List<string>() { "1", "2", "" };
        private IManufacturerRepository _manufacturerRepository;
        private ILogger<ManufacturerController> _logger;
        public ManufacturerController(IManufacturerRepository manufacturerRepository, ILogger<ManufacturerController> logger)
        {
            _manufacturerRepository = manufacturerRepository;
            _logger = logger;
        }
        public IActionResult Index(int? page)
        {
            var manufacturer = _manufacturerRepository.GetAllManufacturer(page);
            _logger.LogInformation("Listando fabricante");
            return View(manufacturer);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm]Manufacturer manufacturer)
        {
            if (ModelState.IsValid)
            {
                _manufacturerRepository.Create(manufacturer);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo fabricante cadastrado");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            _logger.LogError("Erro ao cadastrar fabricante");
            return View();
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _manufacturerRepository.GetManufacturer(id);
            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            _logger.LogInformation("Buscando fabricante pelo id");
            return View(category);
        }
        [HttpPost]
        public IActionResult Update([FromForm] Manufacturer manufacturer, int id)
        {
            if (ModelState.IsValid)
            {
                _manufacturerRepository.Update(manufacturer);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando fabricante");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            _logger.LogError("Erro ao atualizar fabricante");
            return View();
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            _manufacturerRepository.Delete(id);
            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Fabricante excluído");
            return RedirectToAction(nameof(Index));
        }
    }
}