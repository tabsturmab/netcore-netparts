using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Repositories
{
    public class OrderAdvertisementRepository : IOrderAdvertisementRepository
    {
        private IConfiguration _conf;
        NetPartsContext _banco;

        public OrderAdvertisementRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(OrderAdvertisement orderAdvertisement)
        {
            _banco.Add(orderAdvertisement);
            _banco.SaveChanges();
        }
        public OrderAdvertisement GetOrderAdvertisementByAdvertisement(int idAdvert, int idOrder)
        {
            return _banco.OrderAdvertisement.Include(t => t.Advertisement).Include(a => a.Advertisement.Product)
                .Include(a => a.Advertisement.Product.Images).Include(o => o.Advertisement.TechnicalAssistance)
                .Include(t => t.Order).Where(t => t.IdAdvert == idAdvert && t.IdOrder == idOrder).OrderByDescending(a => a.IdOrder).First();
        }
        public IPagedList<OrderAdvertisement> GetAllOrderAdvertisements(int idTecAssistance, int? page, string search)
        {
            int RecordPage = _conf.GetValue<int>("RecordPage");
            int numberPage = page ?? 1;

            var orderGroup = _banco.OrderAdvertisement.Include(a => a.Advertisement)
                .Include(a => a.Advertisement.Product)
                .Include(a => a.Advertisement.Product.Images).Include(o => o.Advertisement.TechnicalAssistance)
                .Include(o => o.Order).Where(e => e.Advertisement.IdTecAssistance == idTecAssistance).OrderByDescending(a => a.IdOrder)
                .GroupBy(order => order.IdOrder);

            List<OrderAdvertisement> lista = new List<OrderAdvertisement>();

            foreach (var order in orderGroup)
            {
                OrderAdvertisement orders = order.First();
                lista.Add(orders);
            }

            return lista.OrderByDescending(a => a.IdOrder).ToPagedList(numberPage, RecordPage);
        }
        public OrderAdvertisement GetOrderAdvertisementByTecAssistance(int idTecAssistance, int idOrder)
        {
            //quem vendeu
            var order = _banco.OrderAdvertisement.Include(a => a.Advertisement)
                .Include(a => a.Advertisement.Product)
                .Include(a => a.Advertisement.Product.Images)
                .Include(o => o.Advertisement.TechnicalAssistance)
                .Include(o => o.Order)
                .Include(s => s.Order.OrderSituation)
                .Where(e => e.Advertisement.IdTecAssistance == idTecAssistance && e.IdOrder == idOrder).OrderByDescending(e => e.IdOrder);

            return order.FirstOrDefault();
        }

        public IEnumerable<OrderAdvertisement> GetAllOrderAdvertisements()
        {
            return _banco.OrderAdvertisement.Include(a => a.Advertisement).Include(a => a.Advertisement.Product).Include(a => a.Advertisement.Product.Images).Include(o => o.Advertisement.TechnicalAssistance).Include(o => o.Order).Distinct().OrderByDescending(a => a.IdOrder).ToPagedList();
        }
    }
}
