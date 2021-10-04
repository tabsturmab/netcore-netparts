using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;
using NetParts.Models.ProductAggregator;

namespace NetParts.Models
{
    public class Manufacturer
    {
        [Key]
        public int IdManufacturer { get; set; }

        [Display(Name = "Name Manufacturer")]
        [Required(ErrorMessage = "The Name Manufacturer is required.")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string NameManufacturer { get; set; }
        public virtual ICollection<TechnicalAssistanceManufacturer> TechnicalAssistanceManufacturer { get; set; }
        public virtual ICollection<Product> Product { get; set; }
    }
}
