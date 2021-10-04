using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Repositories
{
    public class ManufacturerRepository : IManufacturerRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;
        public ManufacturerRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Manufacturer manufacturer)
        {
            _banco.Add(manufacturer);
            _banco.SaveChanges();
        }
        public void Update(Manufacturer manufacturer)
        {
            _banco.Update(manufacturer);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Manufacturer manufacturer = GetManufacturer(Id);
            _banco.Remove(manufacturer);
            _banco.SaveChanges();
        }
        public Manufacturer GetManufacturer(int Id)
        {
            return _banco.Manufacturers.Find(Id);
        }
        public IPagedList<Manufacturer> GetAllManufacturer(int? page)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;
            return _banco.Manufacturers.ToPagedList<Manufacturer>(numberPage, RecordPage);
        }
        public IEnumerable<Manufacturer> GetAllManufacturer()
        {
            return _banco.Manufacturers;
        }
    }
}
