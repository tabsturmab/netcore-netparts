using System.Collections.Generic;
using NetParts.Models;

namespace NetParts.Repositories.Contracts
{
    public interface IArchiveRepository
    {
        void CreateArchives(List<Archive> ListArchives, int IdTecAssistance);
        void Create(Archive archive);
        void Delete(int Id);
        void DeleteArchivesAssistance(int IdTecAssistance);
    }
}
