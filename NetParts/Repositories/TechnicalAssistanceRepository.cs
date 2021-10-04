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
    public class TechnicalAssistanceRepository : ITechnicalAssistanceRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;

        public TechnicalAssistanceRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(TechnicalAssistance technicalAssistance)
        {
            _banco.Add(technicalAssistance);
            _banco.SaveChanges();
        }
        public void Update(TechnicalAssistance technicalAssistance)
        {
            _banco.Update(technicalAssistance);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            TechnicalAssistance technicalAssistance = GetTechnicalAssistance(Id);
            _banco.Remove(technicalAssistance);
            _banco.SaveChanges();
        }
        public TechnicalAssistance GetTechnicalAssistance(int Id)
        {
            return _banco.TechnicalAssistance.Include(a => a.Archives).Include(a => a.Address).Where(a => a.IdTecAssistance == Id).FirstOrDefault();
        }
        public IPagedList<TechnicalAssistance> GetAllTechnicalAssistance(int? page, string search)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;
            return _banco.TechnicalAssistance.Include(a => a.Archives).ToPagedList<TechnicalAssistance>(numberPage, RecordPage);
        }
        public IEnumerable<TechnicalAssistance> GetAllTechnicalAssistance()
        {
            return _banco.TechnicalAssistance.Include(a => a.Archives).Include(a => a.Address); 
        }
        public IPagedList<TechnicalAssistance> GetAllTechnicalAssistance(int? page, int IdTecAssistance)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;
            return _banco.TechnicalAssistance.Include(a => a.Archives).Include(a => a.Address).Where(a => a.IdTecAssistance == IdTecAssistance).ToPagedList<TechnicalAssistance>(numberPage, RecordPage);
        }
        public List<TechnicalAssistance> GetTechnicalAssistanceEmail(string email)
        {
            return _banco.TechnicalAssistance.Where(a => a.EmailAta == email).AsNoTracking().ToList();
        }
    }
}
