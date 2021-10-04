using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.File;
using NetParts.Models;
using NetParts.Repositories;

namespace NetParts.Controllers
{
    public class ArchiveController : Controller
    {
        private static IConfiguration _configuration;
        private ManagerFile _managerFile;
        protected static StorageConfig _storageConfig;
        public ArchiveController(IConfiguration configuration)
        {
            _configuration = configuration;
            _managerFile = new ManagerFile(_configuration);
            _storageConfig = new StorageConfig();
            _storageConfig.AccountKey = configuration.GetValue<string>("StorageConfig:AccountKey");
            _storageConfig.AccountName = configuration.GetValue<string>("StorageConfig:AccountName");
            _storageConfig.ImageContainer = configuration.GetValue<string>("StorageConfig:ImageContainer");
        }

        [HttpPost]
        public async Task<Archive> Store(IFormFile file, int idTecAssistance)
        {
            var upload = new ArchiveRepository(_storageConfig);

            var format = file.FileName.Trim('\"');

            if (upload.IsImage(format))
            {
                if (file.Length > 0)
                {
                    var stream = file.OpenReadStream();
                    var archive = await upload.Upload(idTecAssistance, stream, file.FileName);
                    return archive;
                }
            }
            throw new Exception("Erro ao salvar o arquivo");
        }
        public IActionResult Delete(string way)
        {
            if (ManagerFile.DeleteArchiveAssistance(way))
            {
                return Ok();
            }
            else
            {
                return BadRequest();
            }
        }
    }
}