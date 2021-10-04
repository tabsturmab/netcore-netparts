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

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class AdvertisementController : Controller
    {
        private IAdvertisementRepository _advertisementRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IProductRepository _productRepository;
        private LoginCollaborator _loginCollaborator;
        private ILogger<AdvertisementController> _logger;
        public AdvertisementController(ITechnicalAssistanceRepository technicalAssistanceRepository, IProductRepository productRepository, IAdvertisementRepository advertisementRepository, LoginCollaborator loginCollaborator, ILogger<AdvertisementController> logger)
        {
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _productRepository = productRepository;
            _advertisementRepository = advertisementRepository;
            _loginCollaborator = loginCollaborator;
            _logger = logger;
        }
        public IActionResult Index(int? page, string search)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            var advertisement = _advertisementRepository.GetAllAdvertisements(page, search, loginUsuario.IdTecAssistance);
            _logger.LogInformation("Listando anúncio(s) da assistência técnica");
            return View(advertisement);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
            ViewBag.Products = _productRepository.GetAllProducts().Select(a => new SelectListItem(a.PartNumber, a.IdProduct.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Create(Advertisement advertisement)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            advertisement.IdTecAssistance = loginUsuario.IdTecAssistance;

            if (ModelState.Remove("IdAdvert"))
            {
                _advertisementRepository.Create(advertisement);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo anúncio cadastrado");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Products = _productRepository.GetAllProducts().Select(a => new SelectListItem(a.PartNumber, a.IdProduct.ToString()));
                _logger.LogError("Erro ao cadastrar anúncio");
                return View(advertisement);
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Products = _productRepository.GetAllProducts().Select(a => new SelectListItem(a.PartNumber, a.IdProduct.ToString()));
                Advertisement advertisement = _advertisementRepository.GetAdvertisement(id);
                _logger.LogInformation("Buscando anúncio pelo id");
                return View(advertisement);
            }
        }
        [HttpPost]
        public IActionResult Update(Advertisement advertisement)
        {
            if (ModelState.IsValid)
            {
                _advertisementRepository.Update(advertisement);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando anúncio da assistência técnica");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.TechnicalAssistances = _technicalAssistanceRepository.GetAllTechnicalAssistance().Select(a => new SelectListItem(a.SocialReason, a.IdTecAssistance.ToString()));
                ViewBag.Products = _productRepository.GetAllProducts().Select(a => new SelectListItem(a.PartNumber, a.IdProduct.ToString()));
                _logger.LogError("Erro ao atualizar anúncio");
                return View(advertisement);
            }
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            Advertisement advertisement = _advertisementRepository.GetAdvertisement(id);
            _advertisementRepository.Delete(id);
            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Anúncio excluído");
            return RedirectToAction(nameof(Index));
        }
    }
}