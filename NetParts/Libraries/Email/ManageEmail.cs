using System;
using System.Net.Mail;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NetParts.Libraries.Security;
using NetParts.Models;

namespace NetParts.Libraries.Email
{
    public class ManageEmail
    {
        private SmtpClient _smtp;
        private IConfiguration _configuration;
        private IHttpContextAccessor _httpContextAccessor;
        public ManageEmail(SmtpClient smtp, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _smtp = smtp;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
        public void SendContactEmail(Contact contact)
        {
            string corpMsg = string.Format("<h2>Contact - NetParts</h2>" + 
                                           "<b>Name: </b> {0} <br/>" +
                                           "<b>Email: </b> {1} <br/>" +
                                           "<b>Text: </b> {2} <br/>" +
                                           "<br/>E-mail enviado automaticamente do site NetParts.",
                                            contact.Name,
                                            contact.Email,
                                            contact.Text
            );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add("phoenix2020ifsp@gmail.com");
            message.Subject = "Contact - NetParts - Email: " + contact.Email;
            message.Body = corpMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }

        public void SendCollaboratorPassword(Collaborator collaborator, String senhaGerada)
        {
            string corpoMsg = string.Format("<h2>Colaborador - NetParts</h2>" +
                                            "Sua senha é:" +
                                            "<h3>{0}</h3>", senhaGerada);

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(collaborator.Email);
            message.Subject = "Colaborador - NetParts - Senha do colaborador - " + collaborator.FirstName + " " + collaborator.LastName;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }
        public void SendOrderData(Order order, TechnicalAssistance technicalAssistance)
        {
            string corpoMsg = string.Format("<h2>Pedido - NetParts</h2>" +

                                            "Pedido realizado com sucesso!<br />" +
                                            "<h3>Nº {0}</h3>" +
                                            "<br /> Faça o login em nosso E-Commerce NetParts e acompanhe o andamento de sua compra.",
                order.IdOrder + "-" + order.TransactionId

            );

            //StringBuilder corpoMsg = new StringBuilder();
            //corpoMsg.Append(" < h2 > Pedido - NetParts </ h2 > ");
            //corpoMsg.Append("Pedido realizado com sucesso!<br />");
            /*
             * MailMessage -> Construir a mensagem
             */
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technicalAssistance.EmailAta);
            message.Subject = "NetParts - Pedido - " + order.IdOrder + "-" + order.TransactionId;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }
        public void RegistrationAssistance(TechnicalAssistance technical, int prazoMinimo, int prazoMaximo)
        {
            string corpoMsg = string.Format("<h2> Novo cadastro - NetParts </h2>" +

                                            "Assistência ténica cadastrada com sucesso!<br/>" +
                                            "<h3>Razão Social: {0}</h3>" +
                                            "<br /> O Prazo para ativação da assistência técnica na plataforma é de {1} a {2} dias.",
                technical.SocialReason,
                prazoMinimo,
                prazoMaximo
             );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technical.EmailAta);
            message.Subject = "NetParts - Novo Cadastro - " + technical.SocialReason;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            //Enviar Mensagem via SMTP
            _smtp.Send(message);
        }
        public void EnabledAssistance(TechnicalAssistance technical)
        {
            String texto = null;

            if (technical.EnabledDisabled)
            {
                texto = "Ativada";
            }
            else
            {
                texto = "Desativada";
            }
            string corpoMsg = string.Format("<h2> Situação - NetParts </h2>" +

                                            "Assistência ténica {1} com sucesso!<br/>" +
                                            "<h3>Razão Social: {0}</h3>",
                technical.SocialReason,
                texto
            );
            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technical.EmailAta);
            message.Subject = "NetParts - Nova Ativação - " + technical.SocialReason;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }

        public void SendOrderNFe(OrderAdvertisement order, TechnicalAssistance technical)
        {
            string corpoMsg = string.Format("<h2>NetParts - NFe</h2>" +

                                            "Nota Fiscal enviada com sucesso!<br />" +
                                            "<h4>Link NFe: {0}</h4>" +
                                            "<h4>Assistência Técnica: {1}</h4>" +
                                            "<br />Faça o login em nosso E-Commerce NetParts e acompanhe o andamento de sua compra.",
                order.Order.NFe,
                technical.SocialReason
            );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technical.EmailAta);
            message.Subject = "NetParts - NFe " + order.Order.NFe + "-" + technical.SocialReason;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }

        public void SendOrderTrackingCode(OrderAdvertisement order, TechnicalAssistance technical)
        {
            string corpoMsg = string.Format("<h2>NetParts - Código Rastreamento</h2>" +

                                            "Código Rastreamento enviado com sucesso!<br />" +
                                            "<h4>Código: {0}</h4>" +
                                            "<h4>Assistência Técnica: {1}</h4>" +
                                            "<br />Faça o login em nosso E-Commerce NetParts e acompanhe o andamento de sua compra.",
                order.Order.FreightCodTracking,
                technical.SocialReason
            );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technical.EmailAta);
            message.Subject = "NetParts - Código " + order.Order.FreightCodTracking + "-" + technical.SocialReason;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }

        public void SendOrderDataSale(OrderAdvertisement orderAdvertisement, TechnicalAssistance technicalAssistance)
        {
            string corpoMsg = string.Format("<h2>NetParts - Venda</h2>" +

                                            "Venda realizada em {0} !<br/>" +
                                            "<h3>Pedido Nº {1} - {2}</h3>" +
                                            "<br /> Faça o login em nosso E-Commerce NetParts e acompanhe o andamento de sua venda.",
                orderAdvertisement.Order.DateRegisterOrder,
                orderAdvertisement.IdOrder + "-" + orderAdvertisement.Order.TransactionId,
                technicalAssistance.SocialReason

            );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(technicalAssistance.EmailAta);
            message.Subject = "NetParts - Venda - " + orderAdvertisement.Order.IdOrder + "-" + orderAdvertisement.Order.TransactionId;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }

        public void SendResetPassword(Collaborator collaborator, string idCrip)
        {
            var request = _httpContextAccessor.HttpContext.Request;
            string url = $"{request.Scheme}://{request.Host}/Collaborator/Home/CreateNewPassword/{idCrip}";

            string corpoMsg = string.Format(
                "<h2>Criar nova Senha para {1}({2})</h2>" +
                "Clique no link abaixo para criar uma nova senha!<br />" +
                "<a href='{0}' target='_blank'>{0}</a>",
                url,
                collaborator.FirstName,
                collaborator.Email
            );

            MailMessage message = new MailMessage();
            message.From = new MailAddress(_configuration.GetValue<string>("Email:Username"));
            message.To.Add(collaborator.Email);
            message.Subject = "LojaVirtual - Criar nova senha - " + collaborator.FirstName;
            message.Body = corpoMsg;
            message.IsBodyHtml = true;

            _smtp.Send(message);
        }
    }
}
