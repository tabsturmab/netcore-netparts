using System.ComponentModel.DataAnnotations;

namespace NetParts.Models
{
    public class TechnicalAssistanceManufacturer
    {

        [Required(ErrorMessage = "The Technical Assistance is required.")]
        public int IdTecAssistance { get; set; }
        public virtual TechnicalAssistance TechnicalAssistance { get; set; }

        [Required(ErrorMessage = "The Manufacturer is required.")]
        public int IdManufacturer { get; set; }
        public virtual Manufacturer Manufacturer { get; set; }

    }
}
