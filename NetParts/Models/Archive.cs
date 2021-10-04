using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetParts.Libraries.Lang;

namespace NetParts.Models
{
    public class Archive
    {
        [Key]
        public int IdArchive { get; set; }
        public string Way { get; set; }
        public int IdTecAssistance { get; set; }
        [ForeignKey("IdTecAssistance")]
        public virtual TechnicalAssistance TechnicalAssistance { get; set; }
    }
}
