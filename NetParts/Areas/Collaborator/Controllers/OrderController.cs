using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Filter;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Administrador })]
    public class OrderController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private IOrderRepository _orderRepository;
        private ILogger<OrderController> _logger;

        public OrderController(LoginCollaborator loginCollaborator, IOrderRepository orderRepository, ILogger<OrderController> logger)
        {
            _loginCollaborator = loginCollaborator;
            _orderRepository = orderRepository;
            _logger = logger;
        }
        public IActionResult Index(int? page)
        {
            Models.Collaborator collaborator = _loginCollaborator.GetCollaborator();
            var orders = _orderRepository.GetAllOrderTechnical(page, collaborator.IdTecAssistance);
            _logger.LogInformation("Listando pedido(s) de compra(s)");

            return View(orders);
        }
        public IActionResult View(int id)
        {
            var loginUsuario = _loginCollaborator.GetCollaborator();
            Order order = _orderRepository.GetOrder(id, loginUsuario.IdTecAssistance);
            _logger.LogInformation("Exibindo compra(s)");

            return View(order);
        }
    }
}