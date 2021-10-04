using System.Collections.Generic;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IOrderAdvertisementRepository
    {
        void Create(OrderAdvertisement orderAdvertisement);
        OrderAdvertisement GetOrderAdvertisementByAdvertisement(int idAdvert, int idOrder);
        OrderAdvertisement GetOrderAdvertisementByTecAssistance(int idTecAssistance, int idOrder);
        IPagedList<OrderAdvertisement> GetAllOrderAdvertisements(int idTecAssistance, int? page, string search);
        IEnumerable<OrderAdvertisement> GetAllOrderAdvertisements();
    }
}
