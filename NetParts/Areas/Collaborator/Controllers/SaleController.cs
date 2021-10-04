using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Json.Resolver;
using NetParts.Libraries.Login;
using NetParts.Libraries.Manager.Pagamento.PagarMe;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Models.ViewModels.Order;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;
using PagarMe;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class SaleController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private IOrderRepository _orderRepository;
        private IOrderAdvertisementRepository _orderAdvertisementRepository;
        private ILogger<SaleController> _logger;
        private IOrderSituationRepository _orderSituationRepository;
        private IAdvertisementRepository _advertisementRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private ManageEmail _manageEmail;
        private GerenciarPagarMe _gerenciarPagarMe;
        public SaleController(LoginCollaborator loginCollaborator, IOrderRepository orderRepository, IOrderAdvertisementRepository orderAdvertisementRepository, ILogger<SaleController> logger, IOrderSituationRepository orderSituationRepository, IAdvertisementRepository advertisementRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, ManageEmail manageEmail, GerenciarPagarMe gerenciarPagarMe)
        {
            _loginCollaborator = loginCollaborator;
            _orderRepository = orderRepository;
            _orderAdvertisementRepository = orderAdvertisementRepository;
            _logger = logger;
            _orderSituationRepository = orderSituationRepository;
            _gerenciarPagarMe = gerenciarPagarMe;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _manageEmail = manageEmail;
            _advertisementRepository = advertisementRepository;
        }
        public IActionResult Index(int? page, string search)
        {
            Models.Collaborator collaborator = _loginCollaborator.GetCollaborator();
            var orders = _orderAdvertisementRepository.GetAllOrderAdvertisements(collaborator.IdTecAssistance, page, search);
            _logger.LogInformation("Listando venda(s) por pedido(s)");

            return View(orders);
        }
        public IActionResult View(int id)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            OrderAdvertisement order =
                _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);
            _logger.LogInformation("Exibindo de compra");

            var viewModel = new DisplayViewModel() {OrderAdvertisement = order};

            return View(viewModel);
        }
        public IActionResult NFE([FromForm] DisplayViewModel viewModel, int id)
        {
            ModelState.Remove("Order");
            ModelState.Remove("TrackingCode");
            ModelState.Remove("CardCredit");
            ModelState.Remove("Billet");
            ModelState.Remove("GiveBack");
            ModelState.Remove("GiveBackReasonReject");

            if (ModelState.IsValid)
            {
                var loginUsuario = _loginCollaborator.GetCollaborator();
                OrderAdvertisement orderAdvertisement =
                    _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);
                
                string url = viewModel.NFE.NFE_URL;

                orderAdvertisement.Order.NFe = url;
                orderAdvertisement.Order.Situation = OrderSituationConstant.NF_EMITIDA;

                var orderSituation = new OrderSituation();
                orderSituation.Date = DateTime.Now;
                orderSituation.Data = url;
                orderSituation.IdOrder = id;
                orderSituation.Situation = OrderSituationConstant.NF_EMITIDA;

                _orderSituationRepository.Create(orderSituation);

                _orderRepository.Update(orderAdvertisement.Order);

                TechnicalAssistance technicalAssistance = _technicalAssistanceRepository.GetTechnicalAssistance(orderAdvertisement.Order.IdTecAssistance.Value);
                _manageEmail.SendOrderNFe(orderAdvertisement, technicalAssistance);

                viewModel.OrderAdvertisement = orderAdvertisement;
            }
            else
            {
                ViewBag.MODAL_NFE = true;
            }
            return View(nameof(View), viewModel);
        }
        public IActionResult TrackingCode([FromForm] DisplayViewModel viewModel, int id)
        {
            ModelState.Remove("Order");
            ModelState.Remove("NFE");
            ModelState.Remove("CardCredit");
            ModelState.Remove("Billet");
            ModelState.Remove("GiveBack");
            ModelState.Remove("GiveBackReasonReject");

            if (ModelState.IsValid)
            {
                var loginUsuario = _loginCollaborator.GetCollaborator();
                OrderAdvertisement orderAdvertisement =
                    _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

                string codTracking = viewModel.TrackingCode.CodeTracking;

                orderAdvertisement.Order.FreightCodTracking = codTracking;
                orderAdvertisement.Order.Situation = OrderSituationConstant.EM_TRANSPORTE;

                var orderSituation = new OrderSituation();
                orderSituation.Date = DateTime.Now;
                orderSituation.Data = codTracking;
                orderSituation.IdOrder = id;
                orderSituation.Situation = OrderSituationConstant.EM_TRANSPORTE;

                _orderSituationRepository.Create(orderSituation);

                _orderRepository.Update(orderAdvertisement.Order);

                TechnicalAssistance technicalAssistance = _technicalAssistanceRepository.GetTechnicalAssistance(orderAdvertisement.Order.IdTecAssistance.Value);
                _manageEmail.SendOrderTrackingCode(orderAdvertisement, technicalAssistance);

                viewModel.OrderAdvertisement = orderAdvertisement;
            }
            else
            {
                ViewBag.MODAL_TRACKING = true;
            }
            return View(nameof(View), viewModel);
        }

        //public IActionResult RegisterCancellationOrderCreditCard([FromForm] DisplayViewModel viewModel, int id)
        //{
        //    ModelState.Remove("Order");
        //    ModelState.Remove("NFE");
        //    ModelState.Remove("TrackingCode");
        //    ModelState.Remove("Billet");
        //    ModelState.Remove("GiveBack");
        //    ModelState.Remove("GiveBackReasonReject");

        //    if (ModelState.IsValid)
        //    {
        //        var loginUsuario = _loginCollaborator.GetCollaborator();
        //        OrderAdvertisement orderAdvertisement =
        //            _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //        viewModel.CardCredit.PaymentForm = MethodPaymentConstant.CartaoCredito;

        //        _gerenciarPagarMe.ReversalCardCredit(orderAdvertisement.Order.TransactionId);

        //        orderAdvertisement.Order.Situation = OrderSituationConstant.ESTORNO;

        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;

        //        orderSituation.Data = JsonConvert.SerializeObject(viewModel.CardCredit);
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.ESTORNO;

        //        _orderSituationRepository.Create(orderSituation);
        //        _orderRepository.Update(orderAdvertisement.Order);

        //        ReturnProductsStock(orderAdvertisement.Order);
        //        viewModel.OrderAdvertisement = orderAdvertisement;
        //    }
        //    else
        //    {
        //        ViewBag.MODAL_CREDITCARD = true;
        //    }
        //    return View(nameof(View), viewModel);
        //}
        //public IActionResult RegisterCancellationOrderBillet([FromForm] DisplayViewModel viewModel, int id)
        //{
        //    ModelState.Remove("Order");
        //    ModelState.Remove("NFE");
        //    ModelState.Remove("TrackingCode");
        //    ModelState.Remove("CardCredit");
        //    ModelState.Remove("GiveBack");
        //    ModelState.Remove("GiveBackReasonReject");


        //    if (ModelState.IsValid)
        //    {
        //        var loginUsuario = _loginCollaborator.GetCollaborator();
        //        OrderAdvertisement orderAdvertisement =
        //        _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //        viewModel.Billet.PaymentForm = MethodPaymentConstant.Boleto;

        //        _gerenciarPagarMe.ReversalBillet(orderAdvertisement.Order.TransactionId, viewModel.Billet);

        //        orderAdvertisement.Order.Situation = OrderSituationConstant.ESTORNO;

        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.Data = JsonConvert.SerializeObject(viewModel.Billet);
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.ESTORNO;

        //        _orderSituationRepository.Create(orderSituation);
        //        _orderRepository.Update(orderAdvertisement.Order);

        //        ReturnProductsStock(orderAdvertisement.Order);
        //        viewModel.OrderAdvertisement = orderAdvertisement;
        //    }
        //    else
        //    {
        //        ViewBag.MODAL_BILLET = true;
        //    }
        //    return View(nameof(View), viewModel);
        //}
        //public IActionResult RegisterCancellationOrderGiveBack([FromForm]DisplayViewModel viewModel, int id)
        //{
        //    ModelState.Remove("Order");
        //    ModelState.Remove("NFE");
        //    ModelState.Remove("TrackingCode");
        //    ModelState.Remove("CardCredit");
        //    ModelState.Remove("Billet");
        //    ModelState.Remove("GiveBackReasonReject");

        //    if (ModelState.IsValid)
        //    {
        //        var loginUsuario = _loginCollaborator.GetCollaborator();
        //        OrderAdvertisement orderAdvertisement =
        //            _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //        orderAdvertisement.Order.Situation = OrderSituationConstant.DEVOLVER;

        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.Data = JsonConvert.SerializeObject(viewModel.GiveBack);
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.DEVOLVER;

        //        _orderSituationRepository.Create(orderSituation);

        //        _orderRepository.Update(orderAdvertisement.Order);
        //        viewModel.OrderAdvertisement = orderAdvertisement;
        //    }
        //    else
        //    {
        //        ViewBag.MODAL_GIVEBACK = true;
        //    }
        //    return View(nameof(View), viewModel);
        //}

        //public IActionResult RegisterRejectOrderGiveBack([FromForm]DisplayViewModel viewModel, int id)
        //{
        //    ModelState.Remove("Order");
        //    ModelState.Remove("NFE");
        //    ModelState.Remove("TrackingCode");
        //    ModelState.Remove("CardCredit");
        //    ModelState.Remove("Billet");
        //    ModelState.Remove("GiveBack");

        //    if (ModelState.IsValid)
        //    {
        //        var loginUsuario = _loginCollaborator.GetCollaborator();
        //        OrderAdvertisement orderAdvertisement =
        //            _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //        orderAdvertisement.Order.Situation = OrderSituationConstant.DEVOLUCAO_REJEITADA;

        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.Data = viewModel.GiveBackReasonReject;
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.DEVOLUCAO_REJEITADA;

        //        _orderSituationRepository.Create(orderSituation);

        //        _orderRepository.Update(orderAdvertisement.Order);
        //        viewModel.OrderAdvertisement = orderAdvertisement;
        //    }
        //    else
        //    {
        //        ViewBag.MODAL_GIVEBACK_REJECT = true;
        //    }
        //    return View(nameof(View), viewModel);
        //}
        //public IActionResult RegisterGiveBackOrderApproveCard(int id)
        //{
        //    var loginUsuario = _loginCollaborator.GetCollaborator();
        //    OrderAdvertisement orderAdvertisement =
        //        _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //    if (orderAdvertisement.Order.Situation == OrderSituationConstant.DEVOLVER_ENTREGUE)
        //    {
        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.DEVOLUCAO_ACEITA;
        //        _orderSituationRepository.Create(orderSituation);

        //        _gerenciarPagarMe.ReversalCardCredit(orderAdvertisement.Order.TransactionId);

        //        orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.ESTORNO;
        //        _orderSituationRepository.Create(orderSituation);

        //        ReturnProductsStock(orderAdvertisement.Order);

        //        orderSituation.Situation = OrderSituationConstant.ESTORNO;
        //        _orderRepository.Update(orderAdvertisement.Order);
        //    }

        //    DisplayViewModel viewModel = new DisplayViewModel();
        //    viewModel.OrderAdvertisement.Order = orderAdvertisement.Order;
        //    return View(nameof(View), viewModel);
        //}

        //public IActionResult RegisterGiveBackOrderApproveBillet([FromForm] DisplayViewModel viewModel, int id)
        //{
        //    ModelState.Remove("Order");
        //    ModelState.Remove("NFE");
        //    ModelState.Remove("TrackingCode");
        //    ModelState.Remove("CardCredit");
        //    ModelState.Remove("GiveBack");
        //    ModelState.Remove("GiveBackReasonReject");
        //    ModelState.Remove("Billet.Reason");

        //    if (ModelState.IsValid)
        //    {
        //        var loginUsuario = _loginCollaborator.GetCollaborator();
        //        OrderAdvertisement orderAdvertisement =
        //            _orderAdvertisementRepository.GetOrderAdvertisementByTecAssistance(loginUsuario.IdTecAssistance, id);

        //        var orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.DEVOLUCAO_ACEITA;
        //        _orderSituationRepository.Create(orderSituation);

        //        viewModel.Billet.PaymentForm = MethodPaymentConstant.Boleto;

        //        _gerenciarPagarMe.ReversalBillet(orderAdvertisement.Order.TransactionId, viewModel.Billet);

        //        orderAdvertisement.Order.Situation = OrderSituationConstant.ESTORNO;

        //        orderSituation = new OrderSituation();
        //        orderSituation.Date = DateTime.Now;
        //        orderSituation.Data = JsonConvert.SerializeObject(viewModel.Billet);
        //        orderSituation.IdOrder = id;
        //        orderSituation.Situation = OrderSituationConstant.ESTORNO;

        //        _orderSituationRepository.Create(orderSituation);
        //        _orderRepository.Update(orderAdvertisement.Order);

        //        ReturnProductsStock(orderAdvertisement.Order);
        //        viewModel.OrderAdvertisement = orderAdvertisement;
        //    }
        //    else
        //    {
        //        ViewBag.MODAL_DEVOLUTON_BILLET = true;
        //    }
        //    return View(nameof(View), viewModel);
        //}

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