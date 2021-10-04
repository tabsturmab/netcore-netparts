using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Models;
using NetParts.Repositories;
using NetParts.Repositories.Contracts;

namespace NetParts.Libraries.Validation
{
    public class SingleEmailTechnicalAssistanceAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Digite o e-mail!");
            }

            string Email = (value as string).Trim();

            ITechnicalAssistanceRepository _technicalAssistanceRepository =
                (ITechnicalAssistanceRepository) validationContext.GetService(typeof(ITechnicalAssistanceRepository));
            List<TechnicalAssistance> technical = _technicalAssistanceRepository.GetTechnicalAssistanceEmail(Email);

            TechnicalAssistance objTechnical = (TechnicalAssistance) validationContext.ObjectInstance;

            if (technical.Count > 1)
            {
                return new ValidationResult("E-mail já existente!");
            }

            if (technical.Count == 1 && objTechnical.IdTecAssistance != technical[0].IdTecAssistance)
            {
                return new ValidationResult("E-mail já existente!");
            }

            return ValidationResult.Success;
        }
    }
}