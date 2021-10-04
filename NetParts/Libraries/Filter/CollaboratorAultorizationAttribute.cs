using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using NetParts.Libraries.Login;
using NetParts.Models.Constant;

namespace NetParts.Libraries.Filter
{
    public class CollaboratorAultorizationAttribute : Attribute, IAuthorizationFilter
    {
        LoginCollaborator _loginCollaborator;
        private String[] _typeCollaboratorAutorization;

        public CollaboratorAultorizationAttribute(String[] TypeCollaboratorAutorization)
        {
            _typeCollaboratorAutorization = TypeCollaboratorAutorization;
        }

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            _loginCollaborator = (LoginCollaborator)context.HttpContext.RequestServices.GetService(typeof(LoginCollaborator));
            Models.Collaborator collaborator = _loginCollaborator.GetCollaborator();
            if (collaborator == null)
            {
                context.Result = new RedirectToActionResult("Login", "Home", null);
            }

            if (_typeCollaboratorAutorization == null || !_typeCollaboratorAutorization.Contains(_loginCollaborator.GetCollaborator().TypeCollaborator))
            { 
                context.Result = new StatusCodeResult(403);
            }

        }
    }
}
