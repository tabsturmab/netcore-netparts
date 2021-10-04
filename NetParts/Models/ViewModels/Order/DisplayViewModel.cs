using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Libraries.Lang;

namespace NetParts.Models.ViewModels.Order
{
    public class DisplayViewModel
    {
        public Models.OrderAdvertisement OrderAdvertisement { get; set; }
        public NFE NFE { get; set; }
        public TrackingCode TrackingCode { get; set; }
        public ReverseOrderCard CardCredit { get; set; }
        public ReverseOrderBillet Billet { get; set; }
        public ReverseOrderGiveBack GiveBack { get; set; }

        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string GiveBackReasonReject { get; set; }
    }
}
