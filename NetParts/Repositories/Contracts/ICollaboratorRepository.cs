using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories
{
    public interface ICollaboratorRepository
    {
        Collaborator Login(string Email, string Password);
        void Create(Collaborator collaborator);
        void Update(Collaborator collaborator);
        void UpdatePassword(Collaborator collaborator);
        void Delete(int Id);
        Collaborator GetCollaborator(int Id);
        IPagedList<Collaborator> GetAllCollaborators(int? page, int IdTecAssistance);
        List<Collaborator> GetCollaboratorEmail(string email);
    }
}
