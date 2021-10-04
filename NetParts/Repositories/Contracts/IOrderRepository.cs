using System.Collections.Generic;
using NetParts.Libraries.Login;
using NetParts.Models;
using X.PagedList;

namespace NetParts.Repositories.Contracts
{
    public interface IOrderRepository
    {
        void Create(Order order);
        void Update(Order order);
        Order GetOrder(int Id, int idTechnicalAssistance);
        IPagedList<Order> GetAllOrderTechnical(int? page, int idTechnical);
        IEnumerable<Order> GetAllOrders();
        IPagedList<Order> GetAllOrder(int? page, string numberOrder, string cnpjAta, LoginCollaborator loginCollaborator);
        List<Order> GetAllOrderSituation(string status);
    }
}
