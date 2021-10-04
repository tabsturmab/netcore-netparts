using NetParts.Libraries.Lang;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetParts.Libraries.Validation;
using Newtonsoft.Json;

namespace NetParts.Models
{
    public class TechnicalAssistance
    {
        [Key]
        public int? IdTecAssistance { get; set; }

        [Display(Name = "Corporate Name")]
        [Required(ErrorMessage = "The Corporate Name is required.")]
        [MinLength(10, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string SocialReason { get; set; }

        [CNPJ(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "The CNPJ is required.")]
        public string Cnpj { get; set; }

        [Display(Name = "State Registration")]
        [Required(ErrorMessage = "The State Registration is required.")]
        [MaxLength(14, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string StateInscription { get; set; }

        [Display(Name = "Email")]
        [Required(ErrorMessage = "The Email is required.")]
        [EmailAddress(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [SingleEmailTechnicalAssistance]
        public string EmailAta { get; set; }

        [Display(Name = "Phone")]
        [Required(ErrorMessage = "The Phone is required.")]
        public string Phone { get; set; }

        [Display(Name = "Date Register")]
        [Required(ErrorMessage = "The Date Register is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy")]
        public DateTime DateRegister { get; set; }
        public bool EnabledDisabled { get; set; }
        public virtual ICollection<Advertisement> Advertisement { get; set; }
        public virtual ICollection<Address> Address { get; set; }
        [JsonIgnore]
        public virtual ICollection<Collaborator> Collaborator { get; set; }
        public virtual ICollection<TechnicalAssistanceManufacturer> TechnicalAssistanceManufacturer { get; set; }

        [ForeignKey("IdTecAssistance")]
        public virtual ICollection<Order> Order { get; set; }
        [JsonIgnore]
        
        public virtual ICollection<Archive> Archives { get; set; }
    }
}
