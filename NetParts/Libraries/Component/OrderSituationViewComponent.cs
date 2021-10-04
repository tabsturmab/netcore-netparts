using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetParts.Models;
using NetParts.Models.Constant;

namespace NetParts.Libraries.Component
{
    public class OrderSituationViewComponent : ViewComponent
    {
        List<OrderSituationStatus> TimelineOne { get; set; }
        List<string> StatusTimelineOne = new List<string>() {
            OrderSituationConstant.PEDIDO_REALIZADO,
            OrderSituationConstant.PAGAMENTO_APROVADO,
            OrderSituationConstant.NF_EMITIDA,
            OrderSituationConstant.EM_TRANSPORTE,
            OrderSituationConstant.ENTREGUE,
            OrderSituationConstant.FINALIZADO
        };
        List<OrderSituationStatus> TimelineTwo { get; set; }
        List<string> StatusTimelineTwo = new List<string>() {
            OrderSituationConstant.PAGAMENTO_NAO_EFETUADO
        };

        List<OrderSituationStatus> TimelineThree { get; set; }
        List<string> StatusTimelineThree = new List<string>() {
            OrderSituationConstant.ESTORNO
        };

        List<OrderSituationStatus> TimelineFour { get; set; }
        List<string> StatusTimelineFour = new List<string>() {
            OrderSituationConstant.ESTORNO
        };

        public OrderSituationViewComponent()
        {
            TimelineOne = new List<OrderSituationStatus>();
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PEDIDO_REALIZADO, Concluded = false, Cor = "complete" });
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PAGAMENTO_APROVADO, Concluded = false, Cor = "complete" });
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.NF_EMITIDA, Concluded = false, Cor = "complete" });
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.EM_TRANSPORTE, Concluded = false, Cor = "complete" });
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.ENTREGUE, Concluded = false, Cor = "complete" });
            TimelineOne.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.FINALIZADO, Concluded = false, Cor = "complete" });

            TimelineTwo = new List<OrderSituationStatus>();
            TimelineTwo.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PEDIDO_REALIZADO, Concluded = false, Cor = "complete" });
            TimelineTwo.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PAGAMENTO_NAO_EFETUADO, Concluded = false, Cor = "complete-red" });

            TimelineThree = new List<OrderSituationStatus>();
            TimelineThree.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PEDIDO_REALIZADO, Concluded = false, Cor = "complete" });
            TimelineThree.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.PAGAMENTO_APROVADO, Concluded = false, Cor = "complete" });
            TimelineThree.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.NF_EMITIDA, Concluded = false, Cor = "complete" });
            TimelineThree.Add(new OrderSituationStatus() { Situation = OrderSituationConstant.ESTORNO, Concluded = false, Cor = "complete-red" });
        }
        public async Task<IViewComponentResult> InvokeAsync(Order order)
        {
            List<OrderSituationStatus> timeline = null;

            if (StatusTimelineOne.Contains(order.Situation))
            {
                timeline = TimelineOne;
            }

            if (StatusTimelineTwo.Contains(order.Situation))
            {
                timeline = TimelineTwo;
            }

            if (StatusTimelineThree.Contains(order.Situation))
            {
                timeline = TimelineThree;

                var nfe = order.OrderSituation.Where(a => a.Situation == OrderSituationConstant.NF_EMITIDA).FirstOrDefault();
                if (nfe == null)
                {
                    timeline.Remove(timeline.FirstOrDefault(a => a.Situation == OrderSituationConstant.NF_EMITIDA));
                }
            }

            if (timeline != null)
            {
                foreach (var orderSituation in order.OrderSituation)
                {
                    var orderSituationTimeline = timeline.Where(t => t.Situation == orderSituation.Situation).FirstOrDefault();
                    orderSituationTimeline.DateStatus = orderSituation.Date;
                    orderSituationTimeline.Concluded = true;
                }
            }
            await Task.FromResult(timeline);
            return View(timeline);
        }
    }
}
