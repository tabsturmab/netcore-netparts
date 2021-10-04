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
    public class AddressRepository : IAddressRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;
        public AddressRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Address address)
        {
            _banco.Add(address);
            _banco.SaveChanges();
        }
        public void Update(Address address)
        {
            _banco.Update(address);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Address address = GetAddress(Id);
            _banco.Remove(address);
            _banco.SaveChanges();
        }
        public Address GetAddress(int Id)
        {
            return _banco.Address.Include(a => a.TechnicalAssistance).Where(a => a.IdAddress == Id).OrderBy(a => a.TechnicalAssistance).FirstOrDefault();
        }
        public IPagedList<Address> GetAllAddress(int? page, int IdTecAssistance)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;

            return _banco.Address.Include(a => a.TechnicalAssistance).Where(a => a.IdTecAssistance == IdTecAssistance).OrderBy(a => a.TechnicalAssistance).ToPagedList<Address>(numberPage, RecordPage);
        }
        public IEnumerable<Address> GetAllAddress()
        {
            return _banco.Address;
        }
        public IList<Address> GetAllAddress(int IdTecAssistance)
        {
            return _banco.Address.Include(a => a.TechnicalAssistance).OrderBy(a => a.TechnicalAssistance).Where(a => a.IdTecAssistance == IdTecAssistance).ToList();
        }
    }
}
