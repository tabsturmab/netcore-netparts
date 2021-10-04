using System.ComponentModel.DataAnnotations;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class NFE
    {
        [Url(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E004")]
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public string NFE_URL { get; set; }
    }
}
