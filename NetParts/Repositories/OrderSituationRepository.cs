using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Repositories.Contracts;

namespace NetParts.Repositories
{
    public class OrderSituationRepository : IOrderSituationRepository
    {
        IConfiguration _conf;
        NetPartsContext _banco;

        public OrderSituationRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Update(OrderSituation orderSituation)
        {
            _banco.Update(orderSituation);
            _banco.SaveChanges();
        }
        public void Create(OrderSituation orderSituation)
        {
            _banco.Add(orderSituation);
            _banco.SaveChanges();
        }
        public void Delete(int id)
        {
            OrderSituation orderSituation = GetOrderSituation(id);
            _banco.Remove(orderSituation);
            _banco.SaveChanges();
        }
        public OrderSituation GetOrderSituation(int id)
        {
            return _banco.OrderSituation.Include(a => a.Order).Where(a => a.IdOrderSituation == id).FirstOrDefault();
        }
    }
}
