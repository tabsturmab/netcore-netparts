using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using NetParts.Database;
using NetParts.Models;
using NetParts.Repositories.Contracts;

namespace NetParts.Repositories
{
    public class ArchiveRepository : IArchiveRepository
    {
        private NetPartsContext _banco;
        private readonly StorageConfig _storageConfig = null;
        public ArchiveRepository(NetPartsContext banco)
        {
            _banco = banco;
        }
        public ArchiveRepository(StorageConfig storageConfig)
        {
            _storageConfig = storageConfig; 
        }
        public void CreateArchives(List<Archive> ListArchives, int IdTecAssistance)
        {
            if (ListArchives != null && ListArchives.Count > 0)
            {
                foreach (var Archive in ListArchives)
                {
                    Create(Archive);
                }
            }
        }

        public static async Task<string> UploadFileToStorage(int idTecAssistance, Stream fileStream, string fileName, StorageConfig storageConfig)
        {
            StringBuilder way = new StringBuilder();
            way.Append(storageConfig.ImageContainer);
            way.Append("/Assistante/");
            way.Append(idTecAssistance);

            var storageCredentials = new StorageCredentials(storageConfig.AccountName, storageConfig.AccountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(way.ToString());
            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromStreamAsync(fileStream);

            return blockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }

        public async Task<Archive> Upload(int idTecAssistance, Stream stream, string nameFile)
        {
            var way = await UploadFileToStorage(idTecAssistance, stream, nameFile, _storageConfig);

            return new Archive
            {
                Way = way,
                IdTecAssistance = idTecAssistance
            };
        }
        public bool IsImage(string nameFile)
        {
            string[] formats = new string[] { ".pdf" };
            return formats.Any(item => nameFile.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public void Create(Archive archive)
        {
            _banco.Add(archive);
            _banco.SaveChanges();
        }

        public void Delete(int Id)
        {
            Archive archive = _banco.Archives.Find(Id);
            _banco.Remove(archive);
            _banco.SaveChanges();
        }

        public void DeleteArchivesAssistance(int IdTecAssistance)
        {
            List<Archive> archives = _banco.Archives.Where(a => a.IdTecAssistance == IdTecAssistance).ToList();

            foreach (Archive archive in archives)
            {
                _banco.Remove(archive);
            }
            _banco.SaveChanges();
        }
    }
}
