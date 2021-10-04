using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Repositories
{
    public class CategoryRepository : ICategoryRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;
        public CategoryRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Category category)
        {
            _banco.Add(category);
            _banco.SaveChanges();
        }
        public void Update(Category category)
        {
            _banco.Update(category);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Category category = GetCategory(Id);
            _banco.Remove(category);
            _banco.SaveChanges();
        }
        public Category GetCategory(int Id)
        {
            return _banco.Categories.Find(Id);
        }
        public IPagedList<Category> GetAllCategory(int? page, string search)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;
            return _banco.Categories.OrderBy(a => a.NameCategory).ToPagedList<Category>(numberPage, RecordPage);
        }
        public IEnumerable<Category> GetAllCategory()
        {
            return _banco.Categories;
        }
        public Category GetCategory(string Slug)
        {
            return _banco.Categories.Where(a=>a.Slug == Slug).OrderBy(a => a.NameCategory).FirstOrDefault();
        }
        private List<Category> Categories;
        private List<Category> ListCategoryRecursive = new List<Category>();
        public IEnumerable<Category> GetCategoriesRecursive(Category categoryMaster)
        {
            if (Categories == null)
            {
                Categories = GetAllCategory().ToList();
            }
            if (!ListCategoryRecursive.Exists(a => a.IdCategory == categoryMaster.CategoryMasterId))
            {
                ListCategoryRecursive.Add(categoryMaster);
            }
            var ListCategorySon = Categories.Where(a => a.CategoryMasterId == categoryMaster.IdCategory);
            if (ListCategorySon.Count() > 0)
            {
                ListCategoryRecursive.AddRange(ListCategorySon.ToList());
                foreach (var category in ListCategorySon)
                {
                   GetCategoriesRecursive(category);
                }
            }
            return ListCategoryRecursive;
        }
        public Category GetCategoryName(string nome)
        {
            return _banco.Categories.Where(a => a.NameCategory == nome).OrderBy(a => a.NameCategory).FirstOrDefault();
        }

        public List<Category> GetCategoriesMaster(int id)
        {
            return _banco.Categories.Where(a => a.CategoryMasterId == id).OrderBy(a => a.NameCategory).ToList(); ;
        }
    }
}
