using System.Collections.Generic;
using NetParts.Models;

namespace NetParts.Repositories.Contracts
{
    public interface IImageRepository
    {
        void CreateImages(List<Image> ListImages, int IdProduct);
        void Create(Image image);
        void Delete(int Id);
        void DeleteImagesProduct(int IdProduct);
    }
}
