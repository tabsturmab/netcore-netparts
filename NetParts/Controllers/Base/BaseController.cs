using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using NetParts.Libraries.Login;
using NetParts.Libraries.Manager.Frete;
using NetParts.Libraries.Security;
using NetParts.Libraries.ShoppingCart;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using NetParts.Repositories.Contracts;
using Newtonsoft.Json;

namespace NetParts.Controllers.Base
{
    public class BaseController : Controller
    {
        protected CookieShoppingCart _cookieShoppingCart;
        protected IAdvertisementRepository _advertisementRepository;
        protected IMapper _mapper;
        protected WSCorreiosCalcularFrete _wscorreios;
        protected CookieFrete _cookieFrete;
        protected IAddressRepository _addressRepository;
        protected LoginCollaborator _loginCollaborator;

        public BaseController(LoginCollaborator loginCollaborator, CookieShoppingCart shoppingCart, IAddressRepository addressRepository, IAdvertisementRepository advertisementRepository, IMapper mapper, WSCorreiosCalcularFrete wscorreios, CookieFrete cookieFrete)
        {
            _cookieShoppingCart = shoppingCart;
            _advertisementRepository = advertisementRepository;
            _mapper = mapper;
            _wscorreios = wscorreios;
            _cookieFrete = cookieFrete;
            _addressRepository = addressRepository;
            _loginCollaborator = loginCollaborator;
        }
        protected List<ProductItem> LoadProductDb()
        {
            List<ProductItem> productItemCart = _cookieShoppingCart.Consult();

            List<ProductItem> productItemComplete = new List<ProductItem>();

            foreach (var item in productItemCart)
            {
                Advertisement advertisement = _advertisementRepository.GetAdvertisement(item.IdAdvert);

                ProductItem productItem = _mapper.Map<ProductItem>(advertisement);

                productItem.QuantityProduct = item.QuantityProduct;

                productItemComplete.Add(productItem);
            }
            return productItemComplete;
        }

        protected string GerarHash(object obj)
        {
            return StringMD5.MD5Hash(JsonConvert.SerializeObject(obj));
        }
    }
}