using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using NetParts.Libraries.Lang;
using NetParts.Libraries.Validation;

namespace NetParts.Models
{
    public class Category
    {
        [Key]
        public int IdCategory { get; set; }

        [Display(Name = "Name Category")]
        [Required(ErrorMessage = "The Name Category is required.")]
        [MinLength(4, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [SingleCategory(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E011")]
        public string NameCategory { get; set; }

        [Display(Name = "Slug")]
        [Required(ErrorMessage = "The Slug is required.")]
        [MinLength(4, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [MaxLength(100, ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E002")]
        [SingleSlug(ErrorMessageResourceType = typeof(Msg), ErrorMessageResourceName = "MSG_E011")]
        public string Slug { get; set; }

        public int? CategoryMasterId { get; set; }

        [ForeignKey("CategoryMasterId")]
        public virtual Category CategoryMaster { get; set; }
    }
}
