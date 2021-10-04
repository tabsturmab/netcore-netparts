using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface ITechnicalAssistanceManufacturerRepository
    {
        void Create(TechnicalAssistanceManufacturer technicalAssistanceManufacturer);
        void Update(TechnicalAssistanceManufacturer technicalAssistanceManufacturer);
        void Delete(int idTecAss, int idManu);
        TechnicalAssistanceManufacturer GetTechnicalAssistanceManufacturer(int idTecAss, int idManu);
        IEnumerable<TechnicalAssistanceManufacturer> GetAllByManufacturerTechnicalAssistance(int idTecAssistance);
        IPagedList<TechnicalAssistanceManufacturer> GetAllTechnicalAssistanceManufacturers(int? page, string search);
    }
}
