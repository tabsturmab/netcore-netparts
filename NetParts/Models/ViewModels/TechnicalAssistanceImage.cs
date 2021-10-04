using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using NetParts.Libraries.Lang;

namespace NetParts.Models.ViewModels
{
    public class TechnicalAssistanceArchive
    {
        public TechnicalAssistance technicalAssistance { get; set; }
        [Required(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E001")]
        public List<IFormFile> ImageFile { get; set; }
    }
}
