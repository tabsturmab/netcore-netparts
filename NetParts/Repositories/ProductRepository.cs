using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public class ProductRepository : IProductRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;
        public ProductRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Product product)
        {
            _banco.Add(product);
            _banco.SaveChanges();
        }
        public void Update(Product product)
        {
            _banco.Update(product);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Product product = GetProduct(Id);
            _banco.Remove(product);
            _banco.SaveChanges();
        }
        public Product GetProduct(int Id)
        {
            return _banco.Products.Include(a=>a.Images).Include(a => a.Category).Include(a => a.Manufacturer).Where(a=>a.IdProduct == Id).OrderBy(a => a.PartNumber).FirstOrDefault();
        }
        public List<Product> GetProductCategory(int id)
        {
            return _banco.Products.OrderBy(a => a.PartNumber).Where(a => a.IdCategory == id).ToList();
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return _banco.Products;
        }
        public IPagedList<Product> GetAllProducts(int? page, string search)
        {
            return GetAllProducts(page, search, "A", null);
        }
        public IPagedList<Product> GetAllProducts(int? page, string search, string ordination, IEnumerable<Category> categories)
        {
            int recordPage = _conf.GetValue<int>("recordPage");

            int numberPage = page ?? 1;

            var bancoProduct = _banco.Products.AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                bancoProduct = bancoProduct.Where(a => a.PartNumber.Contains(search.Trim()));
            }
            if (ordination == "A")
            {
                bancoProduct = bancoProduct.OrderBy(a => a.PartNumber);
            }
            if (ordination == "ME")
            {
                bancoProduct = bancoProduct.OrderBy(a => a.Description);
            }
            if (ordination == "MA")
            {
                bancoProduct = bancoProduct.OrderByDescending(a => a.Description);
            }
            if (categories != null && categories.Count() > 0)
            {
                bancoProduct = bancoProduct.Where(a => categories.Select(b => b.IdCategory).Contains(a.IdCategory));
            }
            return bancoProduct.Include(a => a.Images).Include(a => a.Category).Include(a => a.Manufacturer).OrderBy(a => a.PartNumber).ToPagedList<Product>(numberPage, recordPage);
        }

        public Product GetProductPartNumber(string partnumber)
        {
            return _banco.Products.Where(a => a.PartNumber == partnumber).FirstOrDefault();
        }

    }
}
