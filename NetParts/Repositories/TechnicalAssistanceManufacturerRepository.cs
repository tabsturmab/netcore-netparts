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
    public class TechnicalAssistanceManufacturerRepository : ITechnicalAssistanceManufacturerRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;

        public TechnicalAssistanceManufacturerRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(TechnicalAssistanceManufacturer technicalAssistanceManufacturer)
        {
            _banco.Add(technicalAssistanceManufacturer);
            _banco.SaveChanges();
        }
        public void Update(TechnicalAssistanceManufacturer technicalAssistanceManufacturer)
        {
            _banco.Update(technicalAssistanceManufacturer);
            _banco.SaveChanges();
        }
        public void Delete(int idTecAss, int idManu)
        {
            TechnicalAssistanceManufacturer technicalAssistanceManufacturer = GetTechnicalAssistanceManufacturer(idTecAss, idManu);
            _banco.Remove(technicalAssistanceManufacturer);
            _banco.SaveChanges();
        }
        public TechnicalAssistanceManufacturer GetTechnicalAssistanceManufacturer(int idTecAss, int idManu)
        {
            return _banco.TechnicalAssistanceManufacturer.Include(t => t.TechnicalAssistance).Include(t => t.Manufacturer).Where(t => t.IdTecAssistance == idTecAss && t.IdManufacturer == idManu).OrderBy(t => t.TechnicalAssistance.SocialReason).FirstOrDefault();
        }
        public IPagedList<TechnicalAssistanceManufacturer> GetAllTechnicalAssistanceManufacturers(int? page, string search)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;

            if (search != null)
            {
                return _banco.TechnicalAssistanceManufacturer.Include(t => t.TechnicalAssistance).Include(m => m.Manufacturer).Where(a => a.TechnicalAssistance.SocialReason.Contains(search.Trim())).OrderBy(t => t.TechnicalAssistance.SocialReason).ToPagedList<TechnicalAssistanceManufacturer>(numberPage, RecordPage);
            }
            return _banco.TechnicalAssistanceManufacturer.Include(t => t.TechnicalAssistance).Include(m => m.Manufacturer).OrderBy(t => t.TechnicalAssistance.SocialReason).ToPagedList<TechnicalAssistanceManufacturer>(numberPage, RecordPage);

        }
        public IEnumerable<TechnicalAssistanceManufacturer> GetAllTechnicalAssistanceManufacturer()
        {
            return _banco.TechnicalAssistanceManufacturer;
        }

        public IEnumerable<TechnicalAssistanceManufacturer> GetAllByManufacturerTechnicalAssistance(int idTecAssistance)
        {
            return _banco.TechnicalAssistanceManufacturer.Include(t => t.TechnicalAssistance).Include(t => t.Manufacturer).Where(tec => tec.IdTecAssistance == idTecAssistance);
        }
    }
}
