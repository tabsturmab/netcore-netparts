using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Validation;
using Newtonsoft.Json;

namespace NetParts.Models
{
    public class Collaborator
    {
        [Key]
        public int? IdCollaborator { get; set; }

        [Display(Name = "Name")]
        [Required(ErrorMessage = "The First Name is required.")]
        [MinLength(4, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(50, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string FirstName  { get; set; }

        [Display(Name = "Last Name")]
        [Required(ErrorMessage = "The Last Name is required.")]
        [MinLength(4, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string LastName { get; set; }

        [Display(Name = "CPF")]
        [Required(ErrorMessage = "The CPF is required.")]
        [CPF(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [MaxLength(14, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string Cpf { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [SingleEmailCollaborator]
        public string Email { get; set; }

        [Display(Name = "Password")]
        [JsonIgnore]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(6, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string Password { get; set; }

        [NotMapped]
        [Display(Name = "Confirm Password")]
        [Compare("Password", ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E005")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Type Collaborator")]
        [Required(ErrorMessage = "The Type Collaborator is required.")]
        public string TypeCollaborator { get; set; }

        public int IdTecAssistance { get; set; }

        public virtual TechnicalAssistance TechnicalAssistance { get; set; }

    }
}
