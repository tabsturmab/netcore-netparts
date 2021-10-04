using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IManufacturerRepository
    {
        void Create(Manufacturer manufacturer);
        void Update(Manufacturer manufacturer);
        void Delete(int Id);
        Manufacturer GetManufacturer(int Id);
        IEnumerable<Manufacturer> GetAllManufacturer();
        IPagedList<Manufacturer> GetAllManufacturer(int? page);

    }
}
