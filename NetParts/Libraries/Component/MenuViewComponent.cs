using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetParts.Repositories.Contracts;

namespace NetParts.Libraries.Component
{
    public class MenuViewComponent : ViewComponent
    {
        private ICategoryRepository _categoryRepository;
        public MenuViewComponent(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var ListCategory = _categoryRepository.GetAllCategory().ToList();
            await Task.FromResult(ListCategory);
            return View(ListCategory);
        }
    }
}
