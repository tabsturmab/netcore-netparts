using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NetParts.Database;
using NetParts.Models;
using NetParts.Models.Constant;
using X.PagedList;

namespace NetParts.Repositories
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        IConfiguration _conf;
        NetPartsContext _banco;

        public CollaboratorRepository(NetPartsContext banco, IConfiguration configuration)
        {
            _banco = banco;
            _conf = configuration;
        }
        public void Create(Collaborator collaborator)
        {
            _banco.Add(collaborator);
            _banco.SaveChanges();
        }
        public void Update(Collaborator collaborator)
        {
            _banco.Update(collaborator);
            _banco.Entry(collaborator).Property(a => a.Password).IsModified = false;

            _banco.SaveChanges();
        }

        public void UpdatePassword(Collaborator collaborator)
        {
            _banco.Update(collaborator);
            _banco.Entry(collaborator).Property(a => a.FirstName).IsModified = false;
            _banco.Entry(collaborator).Property(a => a.LastName).IsModified = false;
            _banco.Entry(collaborator).Property(a => a.Cpf).IsModified = false;
            _banco.Entry(collaborator).Property(a => a.Email).IsModified = false;
            _banco.Entry(collaborator).Property(a => a.TypeCollaborator).IsModified = false;
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Collaborator collaborator = GetCollaborator(Id);
            _banco.Remove(collaborator);
            _banco.SaveChanges();
        }
        public Collaborator Login(string Email, string Password)
        {
            Collaborator collaborator = _banco.Collaborators.Include(t => t.TechnicalAssistance).Where(m => m.Email == Email && m.Password == Password).FirstOrDefault();
            return collaborator;
        }
        public Collaborator GetCollaborator(int Id)
        {
            return _banco.Collaborators.Find(Id);
        }

        public IPagedList<Collaborator> GetAllCollaborators(int? page, int IdTecAssistance)
        {
            int recordPage = _conf.GetValue<int>("recordPage");
            int numberPage = page ?? 1;
            return _banco.Collaborators.Where(a=>a.IdTecAssistance == IdTecAssistance).ToPagedList<Collaborator>(numberPage, recordPage);
        }

        public List<Collaborator> GetCollaboratorEmail(string email)
        {
            return _banco.Collaborators.Where(a => a.Email == email).AsNoTracking().ToList();
        }
    }
}
