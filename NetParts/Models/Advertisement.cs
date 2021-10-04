using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetParts.Libraries.Lang;
using NetParts.Models.ProductAggregator;
using Newtonsoft.Json;

namespace NetParts.Models
{
    public class Advertisement
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int IdAdvert { get; set; }

        [Display(Name = "Qtd Stock")]
        [Range(1, 10000, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public int Amount { get; set; }

        [Display(Name = "Price")]
        [Range(0.001, 1000000, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E006")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [JsonIgnore]
        public double Price { get; set; }

        [JsonIgnore]
        public int IdTecAssistance { get; set; }

        [JsonIgnore]
        public virtual TechnicalAssistance TechnicalAssistance { get; set; }

        [JsonIgnore]
        public int IdProduct { get; set; }

        [JsonIgnore]
        public virtual Product Product { get; set; }

        public virtual ICollection<OrderAdvertisement> OrderAdvertisement { get; set; }
    }
}