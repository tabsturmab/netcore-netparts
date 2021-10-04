using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class Address
    {
        [Key]
        public int? IdAddress { get; set; }

        [Display(Name = "Address")]
        [Required(ErrorMessage = "The Address is required.")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string Address1 { get; set; }

        [Display(Name = "Number")]
        [Required(ErrorMessage = "The Number is required.")]
        [MaxLength(10, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string NumberAta { get; set; }

        [Display(Name = "Complement")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string Complement { get; set; }

        [Display(Name = "Zip Code")]
        [Required(ErrorMessage = "The Zip Code is required.")]
        [MaxLength(9, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string ZipCode { get; set; }

        [Display(Name = "Neighborhood")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string District { get; set; }

        [Display(Name = "City")]
        [MaxLength(60, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string City { get; set; }

        [Display(Name = "State")]
        [MaxLength(2, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string State1 { get; set; }

        public int IdTecAssistance { get; set; }
        public virtual TechnicalAssistance TechnicalAssistance { get; set; }

    }
}
