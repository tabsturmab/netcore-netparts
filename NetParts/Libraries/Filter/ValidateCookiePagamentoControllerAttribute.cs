using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetParts.Libraries.Lang;

namespace NetParts.Libraries.Filter
{
    public class ValidateCookiePagamentoControllerAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var _cookie = (Cookie.Cookie)context.HttpContext.RequestServices.GetService(typeof(Cookie.Cookie));

            var tipoFreteUsuario = _cookie.Consult("Cart.TipoFrete", false);
            var valorFrete = _cookie.Consult("Cart.ValorFrete", true);
            var cartShopping = _cookie.Consult("Cart.Shopping", true);

            if (cartShopping == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Msg.MSG_E010;
                context.Result = new RedirectToActionResult("Index", "ShoppingCart", null);
            }

            if (tipoFreteUsuario == null || valorFrete == null)
            {
                ((Controller)context.Controller).TempData["MSG_E"] = Msg.MSG_E009;
                context.Result = new RedirectToActionResult("AddAddress", "ShoppingCart", null);
            }
        }
        public void OnActionExecuted(ActionExecutedContext context)
        {

        }
    }
}
