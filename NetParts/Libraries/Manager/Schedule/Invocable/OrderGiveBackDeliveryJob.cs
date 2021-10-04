using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Coravel.Invocable;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Libraries.Manager.Schedule.Invocable
{
    public class OrderGiveBackDeliveryJob : IInvocable
    {
        private IOrderRepository _orderRepository;
        private IOrderSituationRepository _orderSituationRepository;

        public OrderGiveBackDeliveryJob(IOrderRepository orderRepository,
            IOrderSituationRepository orderSituationRepository)
        {
            _orderRepository = orderRepository;
            _orderSituationRepository = orderSituationRepository;
        }

        public Task Invoke()
        {
            var orders = _orderRepository.GetAllOrderSituation(OrderSituationConstant.DEVOLVER);
            foreach (var order in orders)
            {
                var result = new Correios.NET.Services().GetPackageTracking(order.FreightCodTracking);

                if (result.IsDelivered)
                {
                    OrderSituation orderSituation = new OrderSituation();
                    orderSituation.IdOrder = order.IdOrder;
                    orderSituation.Situation = OrderSituationConstant.DEVOLVER_ENTREGUE;
                    orderSituation.Date = result.DeliveryDate.Value;
                    orderSituation.Data = JsonConvert.SerializeObject(result);

                    _orderSituationRepository.Create(orderSituation);

                    order.Situation = OrderSituationConstant.DEVOLVER_ENTREGUE;
                    _orderRepository.Update(order);
                }
            }
            return Task.CompletedTask;
        }
    }
}
