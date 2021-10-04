using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface ICategoryRepository
    {
        void Create(Category category);
        void Update(Category category);
        void Delete(int Id);
        Category GetCategory(int Id);
        Category GetCategory(string Slug);
        List<Category> GetCategoriesMaster(int id);
        IEnumerable<Category> GetCategoriesRecursive(Category categoryMaster);
        Category GetCategoryName(string nome);
        IEnumerable<Category> GetAllCategory();
        IPagedList<Category> GetAllCategory(int? page, string search);
    }
}
