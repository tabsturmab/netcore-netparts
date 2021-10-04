using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class ReverseOrderBillet
    {
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string Reason { get; set; }
        public string PaymentForm { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MaxLength(3, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E003")]
        [MinLength(3, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string BankCode { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string Agency { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string AgencyDv { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string Account { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string AccountDv { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string CNPJ { get; set; }

        [MinLength(4, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string LegalName { get; set; }
    }
}
