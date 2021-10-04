using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class ReverseOrderCard
    {
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string Reason { get; set; }

        public string PaymentForm { get; set; }
    }
}
