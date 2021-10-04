using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Validation;
using NetParts.Models.ViewModels;
using Newtonsoft.Json;

namespace NetParts.Models.ProductAggregator
{
    public class Product
    {
        [Key]
        public int IdProduct { get; set; }

        [Display(Name = "Part Number")]
        [Required(ErrorMessage = "The Part Number is required.")]
        [SinglePartNumber(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E011")]
        [JsonIgnore]
        public string PartNumber { get; set; }

        [Display(Name = "Description")]
        [Required(ErrorMessage = "The Description is required.")]
        [JsonIgnore]
        public string Description { get; set; }

        [Display(Name = "Weight")]
        [Required(ErrorMessage = "The Weight is required.")]
        [JsonIgnore]
        public double Weight { get; set; } //peso

        [Display(Name = "Width")]
        [Required(ErrorMessage = "The Width is required.")]
        [JsonIgnore]
        public int Width1 { get; set; } //largura

        [Display(Name = "Height")]
        [Required(ErrorMessage = "The Height is required.")]
        [JsonIgnore]
        public int Height { get; set; } //altura

        [Display(Name = "Length")]
        [Required(ErrorMessage = "The Length is required.")]
        [JsonIgnore]
        public int Length { get; set; } //Comprimento

        [JsonIgnore]
        public int IdCategory { get; set; }

        [JsonIgnore]
        public virtual Category Category { get; set; }

        public int IdManufacturer { get; set; }

        public virtual Manufacturer Manufacturer { get; set; }

        public virtual ICollection<Advertisement> Advertisement { get; set; }

        [JsonIgnore]
        public virtual ICollection<Image> Images { get; set; }
    }
}
