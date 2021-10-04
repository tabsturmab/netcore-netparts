using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NetParts.Models
{
    public class Order
    {
        [Key]
        public int IdOrder { get; set; }
        public string TransactionId { get; set; } //PagarMe - Transaction -> ID.
        public string FreightCompany { get; set; } //ECT - Correios
        public string FreightCodTracking { get; set; }
        public string FormPayment { get; set; } //Boleto-Cartão Credito
        public decimal ValueTotal { get; set; }
        public string DataTransaction { get; set; } //Transaction - JSON
        public string DataProducts { get; set; } //ProductItem - JSON
        public DateTime DateRegisterOrder { get; set; }
        public string Situation { get; set; }
        public string NFe { get; set; }


        [ForeignKey("TechnicalAssistance")]
        public int? IdTecAssistance { get; set; }
        public virtual TechnicalAssistance TechnicalAssistance { get; set; }

        [ForeignKey("IdOrder")]
        public virtual ICollection<OrderSituation> OrderSituation { get; set; }

        public virtual ICollection<OrderAdvertisement> OrderAdvertisement { get; set; }
    }
}
