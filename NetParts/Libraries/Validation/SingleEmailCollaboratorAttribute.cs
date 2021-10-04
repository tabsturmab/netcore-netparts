using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using NetParts.Models;
using NetParts.Repositories;

namespace NetParts.Libraries.Validation
{
    public class SingleEmailCollaboratorAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult("Digite o e-mail!");
            }
            string Email = (value as string).Trim();

            ICollaboratorRepository _collaboratorRepository = (ICollaboratorRepository)validationContext.GetService(typeof(ICollaboratorRepository));
            List<Collaborator> collaborators = _collaboratorRepository.GetCollaboratorEmail(Email);

            Collaborator objCollaborator = (Collaborator)validationContext.ObjectInstance;

            if (collaborators.Count > 1)
            {
                return new ValidationResult("E-mail já existente!");
            }
            if (collaborators.Count == 1 && objCollaborator.IdCollaborator != collaborators[0].IdCollaborator)
            {
                return new ValidationResult("E-mail já existente!");
            }

            return ValidationResult.Success;
        }
    }
}
