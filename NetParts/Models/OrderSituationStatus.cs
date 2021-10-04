using System;
using System.ComponentModel.DataAnnotations;

namespace NetParts.Models
{
    public class OrderSituationStatus
    {
        public string Situation { get; set; }

        [Display(Name = "Date Status")]
        public DateTime? DateStatus { get; set; }

        [Display(Name = "Concluded")]
        public bool Concluded { get; set; }
        public string Cor { get; set; }
    }
}
