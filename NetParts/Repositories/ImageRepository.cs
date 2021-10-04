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
    public class ImageRepository : IImageRepository
    {
        private NetPartsContext _banco;
        private readonly StorageConfig _storageConfig = null;

        public ImageRepository(NetPartsContext banco)
        {
            _banco = banco;
        }

        public ImageRepository(StorageConfig storageConfig)
        {
            _storageConfig = storageConfig;
        }
        public void CreateImages(List<Image> ListImages, int IdProduct)
        {
            if (ListImages != null && ListImages.Count > 0)
            {
                foreach (var Image in ListImages)
                {
                    Create(Image);
                }
            }
        }
        public static async Task<string> UploadFileToStorage(int idProduto, Stream fileStream, string fileName, StorageConfig storageConfig)
        {
            StringBuilder way = new StringBuilder();
            way.Append(storageConfig.ImageContainer);
            way.Append("/product/");
            way.Append(idProduto);

            var storageCredentials = new StorageCredentials(storageConfig.AccountName, storageConfig.AccountKey);
            var storageAccount = new CloudStorageAccount(storageCredentials, true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            var container = blobClient.GetContainerReference(way.ToString());
            var blockBlob = container.GetBlockBlobReference(fileName);

            await blockBlob.UploadFromStreamAsync(fileStream);

            return blockBlob.SnapshotQualifiedStorageUri.PrimaryUri.ToString();
        }

        public async Task<Image> Upload(int idProduto, Stream stream, string nameFile)
        {
            var way = await UploadFileToStorage(idProduto, stream, nameFile, _storageConfig);

            return new Image()
            {
                Way = way,
                IdProduct = idProduto
            };
        }
        public bool IsImage(string nameFile)
        {
            string[] formats = new string[] { ".jpg", ".png", ".gif", ".jpeg" };
            return formats.Any(item => nameFile.EndsWith(item, StringComparison.OrdinalIgnoreCase));
        }

        public void Create(Image image)
        {
            _banco.Add(image);
            _banco.SaveChanges();
        }
        public void Delete(int Id)
        {
            Image image = _banco.Images.Find(Id);
            _banco.Remove(image);
            _banco.SaveChanges();
        }
        public void DeleteImagesProduct(int IdProduct)
        {
            List<Image> images = _banco.Images.Where(a => a.IdProduct == IdProduct).ToList();

            foreach (Image image in images)
            {
                _banco.Remove(image);
            }
            _banco.SaveChanges();
        }
    }
}
