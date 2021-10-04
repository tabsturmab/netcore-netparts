using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetParts.Models
{
    public class OrderSituation
    {
        [Key]
        public int IdOrderSituation { get; set; }

        public DateTime Date { get; set; }
        public string Situation { get; set; }
        public string Data { get; set; } //JSON - Pagar.ME

        [ForeignKey("Order")]
        public int? IdOrder { get; set; }
        public virtual Order Order { get; set; }
    }
}
