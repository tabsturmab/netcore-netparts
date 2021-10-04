using NetParts.Models;
using NetParts.Repositories.Contracts;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;


namespace NetParts.Libraries.Validation
{
    public class SingleSlugAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            ICategoryRepository _categoryRepository = (ICategoryRepository)validationContext.GetService(typeof(ICategoryRepository));
            Category category = (Category)validationContext.ObjectInstance;

            if (category.IdCategory == 0)
            {
                Category categoryDB = _categoryRepository.GetCategory(category.Slug);
                if (categoryDB == null)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
            else
            {
                Category categoryDB = _categoryRepository.GetCategory(category.Slug);

                if (categoryDB == null)
                {
                    return ValidationResult.Success;
                }
                else if (categoryDB.IdCategory == category.IdCategory)
                {
                    return ValidationResult.Success;
                }
                else
                {
                    return new ValidationResult(FormatErrorMessage(validationContext.DisplayName));
                }
            }
        }
    }
}
