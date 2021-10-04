using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Libraries.Login;
using NetParts.Libraries.Text;
using NetParts.Models;
using NetParts.Repositories.Contracts;
using X.PagedList;

namespace NetParts.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        IConfiguration _conf;
        NetPartsContext _banco;

        public OrderRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Update(Order order)
        {
            _banco.Update(order);
            _banco.SaveChanges();
        }
        public void Create(Order order)
        {
            _banco.Add(order);
            _banco.SaveChanges();
        }
        public Order GetOrder(int Id, int IdTecAssistance)
        {
            //quem comprou
            return _banco.Orders.Include(a => a.TechnicalAssistance).Include(a => a.OrderAdvertisement).Include(a => a.OrderSituation).Include(a => a.OrderAdvertisement).Where(a => a.IdOrder == Id && a.IdTecAssistance == IdTecAssistance).OrderByDescending(a => a.IdOrder).FirstOrDefault();
        }
        public IPagedList<Order> GetAllOrderTechnical(int? page, int idTechnical)
        {
            int recordPage = _conf.GetValue<int>("recordPage");

            int numberPage = page ?? 1;

            return _banco.Orders.Include(a => a.TechnicalAssistance).Include(a => a.OrderSituation).Include(a => a.OrderAdvertisement).Where(t => t.IdTecAssistance == idTechnical).OrderByDescending(a => a.IdOrder).ToPagedList<Order>(numberPage, recordPage);
        }
        public IPagedList<Order> GetAllOrder(int? page, string numberOrder, string cnpjAta, LoginCollaborator loginCollaborator)
        {
            int recordPage = _conf.GetValue<int>("recordPage");

            int numberPage = page ?? 1;

            var query = _banco.Orders.Include(a => a.TechnicalAssistance).Include(a => a.OrderSituation).Include(a => a.OrderAdvertisement).Include(a => a.TechnicalAssistance).OrderByDescending(a => a.IdOrder).AsQueryable();
            
            if (cnpjAta != null)
            {
                query = query.Where(a => a.TechnicalAssistance.Cnpj == cnpjAta);
            }
            if (numberOrder != null)
            {
                string transactionId = string.Empty;
                int id = Mascara.ExtractNumOrder(numberOrder, out transactionId);
            }
            return query.ToPagedList<Order>(numberPage, recordPage);
        }
        public IEnumerable<Order> GetAllOrders()
        {
            return _banco.Orders;
        }

        public List<Order> GetAllOrderSituation(string status)
        {
           return _banco.Orders.Include(a => a.TechnicalAssistance).Include(a => a.OrderSituation).Include(t => t.TechnicalAssistance).Where(a => a.Situation == status).ToList();
        }
    }
}
