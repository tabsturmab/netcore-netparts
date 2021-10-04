using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;
using PagarMe;

namespace NetParts.Controllers
{
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class OrderController : Controller
    {
        private IOrderRepository _orderRepository;
        private LoginCollaborator _loginCollaborator;
        private ILogger<OrderController> _logger;
        public OrderController(IOrderRepository orderRepository, LoginCollaborator loginCollaborator, ILogger<OrderController> logger)
        {
            _orderRepository = orderRepository;
            _loginCollaborator = loginCollaborator;
            _logger = logger;
        }
        public IActionResult Index(int id)
        {
            Collaborator collaborator = _loginCollaborator.GetCollaborator();
            Order order = _orderRepository.GetOrder(id, collaborator.IdTecAssistance);

            TransacaoPagarMe transacao = JsonConvert.DeserializeObject<TransacaoPagarMe>(order.DataTransaction);
            _logger.LogInformation("Deserializando dados da transação do Pagar Me");

            if (order.IdTecAssistance != _loginCollaborator.GetCollaborator().IdTecAssistance)
            {
                _logger.LogError("Erro ao localizar o colaborador");
                return new StatusCodeResult(403);
            } 

            ViewBag.Item = transacao.Item;

            ViewBag.Transacao = transacao;

            return View(order);
        }
    }
}