using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface ITechnicalAssistanceRepository
    {
        void Create(TechnicalAssistance technicalAssistance);
        void Update(TechnicalAssistance technicalAssistance);
        void Delete(int Id);
        TechnicalAssistance GetTechnicalAssistance(int Id);
        IPagedList<TechnicalAssistance> GetAllTechnicalAssistance(int? page, int IdTecAssistance);
        IPagedList<TechnicalAssistance> GetAllTechnicalAssistance(int? page, string search);
        IEnumerable<TechnicalAssistance> GetAllTechnicalAssistance();
        List<TechnicalAssistance> GetTechnicalAssistanceEmail(string email);
    }
}
