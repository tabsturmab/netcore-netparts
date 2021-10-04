using System;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;

namespace NetParts.Libraries.Manager.Schedule.Invocable
{
    public class OrderFinishedJob : IInvocable
    {
        private IOrderRepository _orderRepository;
        private IOrderSituationRepository _orderSituationRepository;
        private IConfiguration _configuration;
        public OrderFinishedJob(IOrderRepository orderRepository, IOrderSituationRepository orderSituationRepository, IAdvertisementRepository advertisementRepository, IConfiguration configuration)
        {
            _orderRepository = orderRepository;
            _orderSituationRepository = orderSituationRepository;
            _configuration = configuration;
        }
        public Task Invoke()
        {
            var orders = _orderRepository.GetAllOrderSituation(OrderSituationConstant.ENTREGUE);
            foreach (var order in orders)
            {
                var orderSituationDB = order.OrderSituation.FirstOrDefault(a => a.Situation == OrderSituationConstant.ENTREGUE);

                if (orderSituationDB != null)
                {
                    int tolerancia = _configuration.GetValue<int>("Finalizado:Days");

                    if (DateTime.Now >= orderSituationDB.Date.AddDays(tolerancia))
                    {
                        OrderSituation orderSituation = new OrderSituation();
                        orderSituation.IdOrder = order.IdOrder;
                        orderSituation.Situation = OrderSituationConstant.FINALIZADO;
                        orderSituation.Date = DateTime.Now;
                        orderSituation.Data = string.Empty;

                        _orderSituationRepository.Create(orderSituation);

                        order.Situation = OrderSituationConstant.FINALIZADO;
                        _orderRepository.Update(order);
                    }
                }
            }
            return Task.CompletedTask;
        }
    }
}
