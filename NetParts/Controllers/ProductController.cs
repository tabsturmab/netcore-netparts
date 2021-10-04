using Microsoft.AspNetCore.Mvc;
using NetParts.Repositories.Contracts;

namespace NetParts.Controllers
{
    public class ProductController : Controller
    {
        private ICategoryRepository _categoryRepository;
        private IProductRepository _productRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IAdvertisementRepository _advertisementRepository;
        public ProductController(ICategoryRepository categoryRepository, IProductRepository productRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, IAdvertisementRepository advertisementRepository)
        {
            _categoryRepository = categoryRepository;
            _productRepository = productRepository;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _advertisementRepository = advertisementRepository;
        }

        [HttpGet]
        [Route("/Product/Category/{slug}")]
        public IActionResult ListCategory(string slug)
        {
            return View(_categoryRepository.GetCategory(slug));
        }

        [HttpGet]
        public ActionResult Show(int id)
        {
            return View(_advertisementRepository.GetAdvertisement(id));
        }
    }
}