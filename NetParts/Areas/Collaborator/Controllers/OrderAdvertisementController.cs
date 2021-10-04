using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Login;
using NetParts.Models;
using NetParts.Repositories.Contracts;

namespace NetParts.Areas.Collaborator.Controllers
{
    public class OrderAdvertisementController : Controller
    {
        private LoginCollaborator _loginCollaborator;
        private IOrderRepository _orderRepository;
        private IAdvertisementRepository _advertisementRepository;
        private IOrderAdvertisementRepository _orderAdvertisementRepository;
        private ILogger<OrderAdvertisementController> _logger;
        public OrderAdvertisementController(LoginCollaborator loginCollaborator, IOrderRepository orderRepository, IAdvertisementRepository advertisementRepository, IOrderAdvertisementRepository orderAdvertisementRepository, ILogger<OrderAdvertisementController> logger)
        {
            _orderRepository = orderRepository;
            _advertisementRepository = advertisementRepository;
            _orderAdvertisementRepository = orderAdvertisementRepository;
            _logger = logger;
        }
    }
}