using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Lang;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Administrador })]
    //[CollaboratorAultorization(CollaboratorTypeConstant.Administrador)]
    public class CategoryController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ILogger<CategoryController> _logger;

        public CategoryController(ICategoryRepository categoryRepository, IProductRepository productRepository, ILogger<CategoryController> logger)
        {
            _categoryRepository = categoryRepository;
            _logger = logger;
            _productRepository = productRepository;
        }
        public IActionResult Index(int? page, string search)
        {
            var categories = _categoryRepository.GetAllCategory(page, search);
            _logger.LogInformation("Listando categorias");
            return View(categories);
        }
        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Categories = _categoryRepository.GetAllCategory().Select(a=>new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            return View();
        }
        [HttpPost]
        public IActionResult Create([FromForm]Category category)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Create(category);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Nova categoria cadastrada");
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = _categoryRepository.GetAllCategory().Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            _logger.LogError("Erro ao cadastrar categoria");
            return View();
        }
        [HttpGet]
        public IActionResult Update(int id)
        {
            var category = _categoryRepository.GetCategory(id);
            ViewBag.Categories = _categoryRepository.GetAllCategory().Where(a=>a.IdCategory != id).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            _logger.LogInformation("Buscando categoria pelo id");
            return View(category);
        }
        [HttpPost]
        public IActionResult Update([FromForm] Category category, int id)
        {
            if (ModelState.IsValid)
            {
                _categoryRepository.Update(category);
                TempData["MSG_S"] = Msg.MSG_S001;
                _logger.LogInformation("Atualizando categoria");
                return RedirectToAction(nameof(Index));
            }
            ViewBag.Categories = _categoryRepository.GetAllCategory().Where(a => a.IdCategory != id).Select(a => new SelectListItem(a.NameCategory, a.IdCategory.ToString()));
            _logger.LogError("Erro ao atualizar categoria");
            return View();
        }
        [HttpGet]
        [ValidateHttpReferer]
        public IActionResult Delete(int id)
        {
            var categoriesSon = _categoryRepository.GetCategoriesMaster(id);
            if (categoriesSon.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in categoriesSon)
                {
                    sb.Append($"'{item.NameCategory}' ");
                }

                TempData["MSG_E"] = string.Format(Msg.MSG_E012, sb.ToString());
                return RedirectToAction(nameof(Index));
            }

            var productsSon = _productRepository.GetProductCategory(id);

            if (productsSon.Count > 0)
            {
                StringBuilder sb = new StringBuilder();

                foreach (var item in productsSon)
                {
                    sb.Append($"'{item.PartNumber}' ");
                }

                TempData["MSG_E"] = string.Format(Msg.MSG_E013, sb.ToString());
                return RedirectToAction(nameof(Index));
            }

            _categoryRepository.Delete(id);
            TempData["MSG_S"] = Msg.MSG_S002;
            _logger.LogInformation("Categoria excluída");
            return RedirectToAction(nameof(Index));
        }
    }
}