using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using AutoMapper;
using Coravel.Invocable;
using Microsoft.Extensions.Configuration;
using NetParts.Libraries.Json.Resolver;
using NetParts.Libraries.Manager.Pagamento.PagarMe;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;
using PagarMe;

namespace NetParts.Libraries.Manager.Schedule.Invocable
{
    public class OrderPaymentSituationJob : IInvocable
    {
        private GerenciarPagarMe _gerenciarPagarMe;
        private IOrderRepository _orderRepository;
        private IOrderSituationRepository _orderSituationRepository;
        private IMapper _mapper;
        private IConfiguration _configuration;
        private IAdvertisementRepository _advertisementRepository;

        public OrderPaymentSituationJob(GerenciarPagarMe gerenciarPagarMe, IOrderRepository orderRepository, IOrderSituationRepository orderSituationRepository, IMapper mapper, IConfiguration configuration, IAdvertisementRepository advertisementRepository)
        {
            _gerenciarPagarMe = gerenciarPagarMe;
            _orderRepository = orderRepository;
            _orderSituationRepository = orderSituationRepository;
            _mapper = mapper;
            _configuration = configuration;
            _advertisementRepository = advertisementRepository;
        }
        public Task Invoke()
        {
            var orderExecuted = _orderRepository.GetAllOrderSituation(OrderSituationConstant.PEDIDO_REALIZADO);
            foreach (var order in orderExecuted)
            {
                string situation = null;
                var transaction = _gerenciarPagarMe.GetTransaction(order.TransactionId);

                int toleranciaDias = _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaExpiracao") + _configuration.GetValue<int>("Pagamento:PagarMe:BoletoDiaToleranciaVencido");
                if (transaction.Status == TransactionStatus.WaitingPayment &&
                    transaction.PaymentMethod == PaymentMethod.Boleto && DateTime.Now > order.DateRegisterOrder.AddDays(toleranciaDias))
                {
                    situation = OrderSituationConstant.PAGAMENTO_NAO_EFETUADO;
                    ReturnProductsStock(order);
                }
                if (transaction.Status == TransactionStatus.Refused)
                {
                    situation = OrderSituationConstant.PAGAMENTO_REJEITADO;
                    ReturnProductsStock(order);
                }
                if (transaction.Status == TransactionStatus.Authorized || transaction.Status == TransactionStatus.Paid)
                {
                    situation = OrderSituationConstant.PAGAMENTO_APROVADO;
                }
                if (transaction.Status == TransactionStatus.Refunded)
                {
                    situation = OrderSituationConstant.ESTORNO;
                    ReturnProductsStock(order);
                }
                if (situation != null)
                {
                    TransacaoPagarMe transacaoPagarMe = _mapper.Map<Transaction, TransacaoPagarMe>(transaction);
                    transacaoPagarMe.Customer.Gender = Gender.Female;

                    OrderSituation orderSituation = new OrderSituation();
                    orderSituation.IdOrder = order.IdOrder;
                    orderSituation.Situation = situation;
                    orderSituation.Date = transaction.DateUpdated.Value;
                    orderSituation.Data = JsonConvert.SerializeObject(transacaoPagarMe);

                    _orderSituationRepository.Create(orderSituation);
                    order.Situation = situation;
                    _orderRepository.Update(order);
                }
            }
            Debug.WriteLine("--OrderPaymentSituationJob - Executed!--");

            return Task.CompletedTask;
        }

        private void ReturnProductsStock(Order order)
        {
            TransacaoPagarMe transation = JsonConvert.DeserializeObject<TransacaoPagarMe>(order.DataTransaction, new JsonSerializerSettings() { ContractResolver = new ProductItemResolver<List<Item>>() });

            foreach (var product in transation.Item)
            {
                Advertisement productDB = _advertisementRepository.GetAdvertisement(Convert.ToInt32(product.Id));
                productDB.Amount += product.Quantity;

                _advertisementRepository.Update(productDB);
            }
        }
    }
}
