using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.Email;
using NetParts.Models;

namespace NetParts.Controllers
{
    public class HomeController : Controller
    {
        private ManageEmail _manageEmail;
        private ILogger<HomeController> _logger;
        public HomeController(ManageEmail manageEmail, ILogger<HomeController> logger)
        {
            _manageEmail = manageEmail;
            _logger = logger;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Category()
        {
            return View();
        }
        public IActionResult Contact()
        {
            return View();
        }
        public IActionResult Register()
        {
            return View();
        }
        public IActionResult ContactAction()
        {
            try
            {
                Contact contact = new Contact
                {
                    Name = HttpContext.Request.Form["name"],
                    Email = HttpContext.Request.Form["email"],
                    Text = HttpContext.Request.Form["text"]
                };

                var ListMsg = new List<ValidationResult>();
                var context = new ValidationContext(contact);
                bool IsValid = Validator.TryValidateObject(contact, context, ListMsg, true);

                if (IsValid)
                {
                    _manageEmail.SendContactEmail(contact);
                    _logger.LogInformation("Contato: E-mail enviado com sucesso");
                    ViewData["MSG_S"] = "Mensagem de contato enviada com sucesso!";
                }
                else
                {
                    StringBuilder sb = new StringBuilder();
                    foreach (var text in ListMsg)
                    {
                        sb.Append(text.ErrorMessage + "<br/>");
                    }

                    ViewData["MSG_E"] = sb.ToString();
                    ViewData["CONTACT"] = contact;
                }
            }
            catch (Exception)
            {
                _logger.LogError("Contato: Erro ao enviar o e-mail");
                ViewData["MSG_E"] = "Identificamos um erro, tente novamente mais tarde!";
            }
            return View("Contact");
        }

        public IActionResult SetCulture(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions { Expires = DateTimeOffset.UtcNow.AddYears(1) }
            );

            return LocalRedirect(returnUrl);
        }
    }
}