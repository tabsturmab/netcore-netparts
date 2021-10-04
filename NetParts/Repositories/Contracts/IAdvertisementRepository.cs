using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IAdvertisementRepository
    {
        void Create(Advertisement advertisement);
        void Update(Advertisement advertisement);
        void Delete(int Id);
        Advertisement GetAdvertisement(int Id);
        IEnumerable<Advertisement> GetAllAdvertisements();
        IPagedList<Advertisement> GetAllAdvertisements(int? page, string search, int IdTecAssistance);
        IPagedList<Advertisement> GetAllAdvertisements(int? page, string search);
        IPagedList<Advertisement> GetAllAdvertisements(int? page, string search, string ordination, IEnumerable<Category> categories, int? IdTecAssistance);
    }
}