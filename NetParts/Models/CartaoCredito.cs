using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class CartaoCredito
    {
        [Display(Name = "Number Card")]
        [Required(ErrorMessage = "The Number Card is required.")]
        [CreditCard(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [MinLength(15, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(16, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E003")]
        public string NumberCard{ get; set; }

        [Display(Name = "Name On Card")]
        [Required(ErrorMessage = "The Name On Card is required.")]
        public string NameOnCard { get; set; }

        [Display(Name = "Expiration MM")]
        [Required(ErrorMessage = "The Expiration MM is required.")]
        public string ExpirationMM { get; set; }

        [Display(Name = "Expiration YY")]
        [Required(ErrorMessage = "The Expiration YY is required.")]
        public string ExpirationYY { get; set; }

        [Display(Name = "Security Code")]
        [Required(ErrorMessage = "The Security Code is required.")]
        public string SecurityCode { get; set; }
    }
}
