using System.Threading.Tasks;
using Coravel.Invocable;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Libraries.Manager.Schedule.Invocable
{
    public class OrderDeliveryJob : IInvocable
    {
        private IOrderRepository _orderRepository;
        private IOrderSituationRepository _orderSituationRepository;
        public OrderDeliveryJob(IOrderRepository orderRepository, IOrderSituationRepository orderSituationRepository)
        {
            _orderRepository = orderRepository;
            _orderSituationRepository = orderSituationRepository;
        }
        public Task Invoke()
        {
            var orders = _orderRepository.GetAllOrderSituation(OrderSituationConstant.EM_TRANSPORTE);
            foreach (var order in orders)
            {
                var result = new Correios.NET.Services().GetPackageTracking(order.FreightCodTracking);

                if (result.IsDelivered)
                {
                    OrderSituation orderSituation = new OrderSituation();
                    orderSituation.IdOrder = order.IdOrder;
                    orderSituation.Situation = OrderSituationConstant.ENTREGUE;
                    orderSituation.Date = result.DeliveryDate.Value;
                    orderSituation.Data = JsonConvert.SerializeObject(result);

                    _orderSituationRepository.Create(orderSituation);

                    order.Situation = OrderSituationConstant.ENTREGUE;
                    _orderRepository.Update(order);
                }
            }
            return Task.CompletedTask;
        }
    }
}
