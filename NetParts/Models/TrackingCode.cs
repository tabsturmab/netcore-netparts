using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class TrackingCode
    {
        [Display(Name = "Code Tracking")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        [MinLength(5, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        public string CodeTracking { get; set; }
    }
}
