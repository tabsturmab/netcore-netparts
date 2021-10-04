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
    public class AdvertisementRepository : IAdvertisementRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;
        public AdvertisementRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Advertisement advertisement)
        {
            _banco.Add(advertisement);
            _banco.SaveChanges();
        }
        public void Update(Advertisement advertisement)
        {
            _banco.Update(advertisement);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Advertisement advertisement = GetAdvertisement(Id);
            _banco.Remove(advertisement);
            _banco.SaveChanges();
        }
        public Advertisement GetAdvertisement(int Id)
        {
            return _banco.Advertisement.Include(p => p.Product).Include(i => i.Product.Images).Include(t => t.TechnicalAssistance).Include(a => a.TechnicalAssistance.Address).Include(a => a.OrderAdvertisement).Where(p => p.IdAdvert == Id && p.TechnicalAssistance.EnabledDisabled == true).OrderBy(p => p.Product.PartNumber).FirstOrDefault();
        }
        public IEnumerable<Advertisement> GetAllAdvertisements()
        {
            return _banco.Advertisement;
        }
        public IPagedList<Advertisement> GetAllAdvertisements(int? page, string search)
        {
            return GetAllAdvertisements(page, search, "A", null, null);
        }
        public IPagedList<Advertisement> GetAllAdvertisements(int? page, string search, string ordination, IEnumerable<Category> categories, int? idTecAssistance)
        {
            int recordPage = _conf.GetValue<int>("recordPage");

            int numberPage = page ?? 1;

            var bancoAdvert = _banco.Advertisement.AsQueryable();
            if (!string.IsNullOrEmpty(search))
            {
                bancoAdvert = bancoAdvert.Where(a => a.Product.PartNumber.Contains(search.Trim()));
            }
            if (ordination == "A")
            {
               bancoAdvert = bancoAdvert.OrderBy(a => a.Product.PartNumber);
            }
            if (ordination == "ME")
            {
                bancoAdvert = bancoAdvert.OrderBy(a => a.Price);
            }

            if (ordination == "MA")
            {
                bancoAdvert = bancoAdvert.OrderByDescending(a => a.Price);
            }
            if (categories != null && categories.Count() > 0)
            {
                bancoAdvert = bancoAdvert.Where(a => categories.Select(b => b.IdCategory).Contains(a.Product.IdCategory));
            }

            if (idTecAssistance != null)
            {
                bancoAdvert = bancoAdvert.Where(t => t.IdTecAssistance == idTecAssistance);
            }

            return bancoAdvert.Include(a => a.Product.Category).Include(a => a.Product).Include(a => a.Product.Images).Include(a => a.Product.Manufacturer).Include(t => t.TechnicalAssistance).Include(a => a.TechnicalAssistance.Address).Include(t => t.OrderAdvertisement).Where(a => a.TechnicalAssistance.EnabledDisabled == true).ToPagedList<Advertisement>(numberPage, recordPage);
        }
        public IPagedList<Advertisement> GetAllAdvertisements(int? page, string search, int IdTecAssistance)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;
            return GetAllAdvertisements(page, search, "A", null, IdTecAssistance).ToPagedList<Advertisement>(numberPage, RecordPage);
            //return _banco.Advertisement.Include(a => a.TechnicalAssistance).Include(a => a.OrderAdvertisement).Include(p => p.Product.Manufacturer).Include(p=> p.Product).Where(a => a.IdTecAssistance == IdTecAssistance && a.TechnicalAssistance.EnabledDisabled == true).ToPagedList<Advertisement>(numberPage, RecordPage);
        }
    }
}
