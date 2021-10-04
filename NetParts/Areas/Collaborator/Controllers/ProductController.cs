using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AngleSharp.Io.Dom;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Areas.Collaborator.Controllers;
using NetParts.Libraries.Cookie;
using NetParts.Libraries.File;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ProductAggregator;
using NetParts.Models.ViewModels;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador, CollaboratorTypeConstant.Comum })]
    public class ProductController : Controller
    {
        private IProductRepository _productRepository;
        private IImageRepository _imageRepository;
        private LoginCollaborator _loginCollaborator;
        private static IConfiguration _configuration;
        private ICategoryRepository _categoryRepository;
        private IManufacturerRepository _manufacturerRepository;
        private ITechnicalAssistanceManufacturerRepository _technicalAssistanceManufacturerRepository;
        private ILogger<ProductController> _logger;

        public ProductController(IProductRepository productRepository, ICategoryRepository categoryRepository, LoginCollaborator loginCollaborator, IManufacturerRepository manufacturerRepository, IImageRepository imageRepository, ILogger<ProductController> logger, IConfiguration configuration, ITechnicalAssistanceManufacturerRepository technicalAssistanceManufacturerRepository)
        {
            _productRepository = productRepository;
            _imageRepository = imageRepository;
            _loginCollaborator = loginCollaborator;
            _categoryRepository = categoryRepository;
            _manufacturerRepository = manufacturerRepository;
            _configuration = configuration;
            _logger = logger;
            _technicalAssistanceManufacturerRepository = technicalAssistanceManufacturerRepository;
        }
        public IActionResult Index(int? page, string search)
        {
            var products = _productRepository.GetAllProducts(page, search);
            _logger.LogInformation("Listando produtos");
            return View(products);
        }
        [HttpGet]
        public IActionResult Create()
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();

            ViewBag.Categories = _categoryRepository.GetAllCategory().OrderBy(c=>c.NameCategory).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            ViewBag.Manufacturers = _technicalAssistanceManufacturerRepository.GetAllByManufacturerTechnicalAssistance(loginUsuario.IdTecAssistance).OrderBy(a => a.Manufacturer.NameManufacturer).Select(a => new SelectListItem(a.Manufacturer.NameManufacturer, a.IdManufacturer.ToString()));
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(ProductImage productImage)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();

            if (ModelState.IsValid)
            {
                ImageController imageController = new ImageController(_configuration);
                _productRepository.Create(productImage.product);

                foreach (IFormFile file in productImage.ImageFile)
                {
                    var image = await imageController.Store(file, productImage.product.IdProduct);
                    _imageRepository.Create(image);
                }

                //TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Novo produto cadastrado");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categories = _categoryRepository.GetAllCategory().OrderBy(c => c.NameCategory).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
                ViewBag.Manufacturers = _technicalAssistanceManufacturerRepository.GetAllByManufacturerTechnicalAssistance(loginUsuario.IdTecAssistance).OrderBy(a => a.Manufacturer.NameManufacturer).Select(a => new SelectListItem(a.Manufacturer.NameManufacturer, a.IdManufacturer.ToString()));
                _logger.LogError("Erro ao cadastrar produto");
                return View(productImage);
            }
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            ViewBag.Categories = _categoryRepository.GetAllCategory().OrderBy(c => c.NameCategory).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().OrderBy(a => a.NameManufacturer).Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
            Product product = _productRepository.GetProduct(id);
            ProductImage productImage = new ProductImage();
            productImage.product = product;
            _logger.LogInformation("Buscando produto pelo id");
            return View(productImage);
        }
        [HttpPost]
        public async Task<IActionResult> Update(ProductImage productImage)
        {
            if (ModelState.IsValid)
            {
                ImageController imageController = new ImageController(_configuration);


                _productRepository.Update(productImage.product);

                if (productImage.ImageFile != null)
                {
                    foreach (IFormFile file in productImage.ImageFile)
                    {
                        var image = await imageController.Store(file, productImage.product.IdProduct);
                        _imageRepository.Create(image);
                    }
                }

                //TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando produto");
                return RedirectToAction(nameof(Index));
            }
            else
            {
                ViewBag.Categories = _categoryRepository.GetAllCategory().OrderBy(c => c.NameCategory).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
                ViewBag.Manufacturers = _manufacturerRepository.GetAllManufacturer().OrderBy(a => a.NameManufacturer).Select(a => new SelectListItem(a.NameManufacturer, a.IdManufacturer.ToString()));
                _logger.LogError("Erro ao atualizar produto");
                return View(productImage);
            }
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            _imageRepository.DeleteImagesProduct(id);
            _productRepository.Delete(id);

            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Produto excluído");
            return RedirectToAction(nameof(Index));
        }
    }
}