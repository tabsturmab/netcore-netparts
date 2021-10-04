using System.ComponentModel.DataAnnotations;
using NetParts.Models.ProductAggregator;

namespace NetParts.Models
{
    public class Image
    {
        [Key]
        public int IdImage { get; set; }
        public string Way { get; set; }
        public int IdProduct { get; set; }
        public virtual Product Product { get; set; }
    }
}
