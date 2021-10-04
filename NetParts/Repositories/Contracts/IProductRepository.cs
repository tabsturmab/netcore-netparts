using System.Collections.Generic;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IProductRepository
    {
        void Create(Product product);
        void Update(Product product);
        void Delete(int Id);
        Product GetProduct(int Id);
        List<Product> GetProductCategory(int id);
        IEnumerable<Product> GetAllProducts();
        Product GetProductPartNumber(string partnumber);
        IPagedList<Product> GetAllProducts(int? page, string search);
        IPagedList<Product> GetAllProducts(int? page, string search, string ordination, IEnumerable<Category> categories);
    }
}
