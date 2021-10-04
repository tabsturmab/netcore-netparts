using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Models;
using NetParts.Models.ProductAggregator;
using NetParts.Repositories.Contracts;

namespace NetParts.Libraries.Validation
{
    public class SinglePartNumberAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            IProductRepository _productRepository = (IProductRepository)validationContext.GetService(typeof(IProductRepository));
            Product product = (Product)validationContext.ObjectInstance;

            if (product.IdProduct == 0)
            {
                Product productDB = _productRepository.GetProductPartNumber(product.PartNumber);
                if (productDB == null)
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
                Product productDB = _productRepository.GetProductPartNumber(product.PartNumber);

                if (productDB == null)
                {
                    return ValidationResult.Success;
                }
                else if (productDB.IdProduct == product.IdProduct)
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
