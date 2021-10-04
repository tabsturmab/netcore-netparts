using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IAddressRepository
    {
        void Create(Address address);
        void Update(Address address);
        void Delete(int Id);

        Address GetAddress(int Id);
        IEnumerable<Address> GetAllAddress();
        IPagedList<Address> GetAllAddress(int? page, int IdTecAssistance);
        IList<Address> GetAllAddress(int IdTecAssistance);
    }
}
