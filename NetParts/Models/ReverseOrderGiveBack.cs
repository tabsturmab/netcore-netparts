using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class ReverseOrderGiveBack
    {
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string Reason { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(5, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string TrackingCode { get; set; }
    }
}
