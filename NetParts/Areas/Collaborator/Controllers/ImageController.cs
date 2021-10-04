using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using NetParts.Libraries.File;
using NetParts.Libraries.Filter;
using NetParts.Models;
using NetParts.Models.Constant;
using NetParts.Repositories;

namespace NetParts.Areas.Collaborator.Controllers
{
    [Area("Collaborator")]
    [CollaboratorAultorization(new String[] { CollaboratorTypeConstant.Gerente, CollaboratorTypeConstant.Comum, CollaboratorTypeConstant.Administrador })]
    public class ImageController : Controller
    {
        private ILogger<ImageController> _logger;
        private static IConfiguration _configuration;
        private ManagerFile _managerFile;
        protected static StorageConfig _storageConfig;

        public ImageController(IConfiguration configuration)
        {
            _configuration = configuration;
            _managerFile = new ManagerFile(_configuration);
            _storageConfig = new StorageConfig();
            _storageConfig.AccountKey = configuration.GetValue<string>("StorageConfig:AccountKey");
            _storageConfig.AccountName = configuration.GetValue<string>("StorageConfig:AccountName");
            _storageConfig.ImageContainer = configuration.GetValue<string>("StorageConfig:ImageContainer");
        }

        [HttpPost]
        public async Task<Image> Store(IFormFile file, int idProduto)
        {
            var upload = new ImageRepository(_storageConfig);
            
            var format = file.FileName.Trim('\"');

            if (upload.IsImage(format))
            {
                if (file.Length > 0)
                {
                    var stream = file.OpenReadStream();
                    var image = await upload.Upload(idProduto, stream, file.FileName);
                    return image;
                }
            }
            throw new Exception("Erro ao salvar a imagem");
        }

        public IActionResult Delete(string way)
        {
            _logger.LogInformation("Excluíndo imagem do produto");
            if (ManagerFile.DeleteImageProduct(way))
            {
                return Ok();
            }
            else
            {
                _logger.LogError("Erro ao exluir a imagem do produto");
                return BadRequest();
            }
        }
    }
}