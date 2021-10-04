using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using NetParts.Models.ViewModels;
using NetParts.Repositories.Contracts;

namespace NetParts.Controllers
{
    public class AdvertisementController : Controller
    {
        private IAdvertisementRepository _advertisementRepository;
        private ITechnicalAssistanceRepository _technicalAssistanceRepository;
        private IProductRepository _productRepository;


        public AdvertisementController(IAdvertisementRepository advertisementRepository, ITechnicalAssistanceRepository technicalAssistanceRepository, IProductRepository productRepository)
        {
            _advertisementRepository = advertisementRepository;
            _technicalAssistanceRepository = technicalAssistanceRepository;
            _productRepository = productRepository;
        }
    }
}
