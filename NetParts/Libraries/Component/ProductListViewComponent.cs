using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetParts.Models;
using NetParts.Models.ViewModels.Components;
using NetParts.Repositories.Contracts;

namespace NetParts.Libraries.Component
{
    public class ProductListViewComponent : ViewComponent
    {
        private ICategoryRepository _categoryRepository;
        private IAdvertisementRepository _advertisementRepository;
        public ProductListViewComponent(ICategoryRepository categoryRepository, IAdvertisementRepository advertisementRepository)
        {
            _categoryRepository = categoryRepository;
            _advertisementRepository = advertisementRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            int? page = 1;
            string search = "";
            string ordination = "A";
            IEnumerable<Category> categories = null;

            if (HttpContext.Request.Query.ContainsKey("page"))
            {
                page = int.Parse(HttpContext.Request.Query["page"]);
            }
            if (HttpContext.Request.Query.ContainsKey("search"))
            {
                search = HttpContext.Request.Query["search"].ToString();
            }
            if (HttpContext.Request.Query.ContainsKey("ordination"))
            {
                ordination = HttpContext.Request.Query["ordination"];
            }
            if (ViewContext.RouteData.Values.ContainsKey("slug"))
            {
                string slug = ViewContext.RouteData.Values["slug"].ToString();
                Category CategoryMaster = _categoryRepository.GetCategory(slug);
                categories = _categoryRepository.GetCategoriesRecursive(CategoryMaster);
            }
            var viewModel = new ProductListViewModel() { list = _advertisementRepository.GetAllAdvertisements(page, search, ordination, categories, null)};
            await Task.FromResult(viewModel);
            return View(viewModel);
        }
    }
}
