using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class Contact
    {
        [Display(Name = "Name")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(5, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string Name { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [EmailAddress(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        public string Email { get; set; }

        [Display(Name = "Text")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(10, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(150, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E003")]
        public string Text { get; set; }
    }
}
