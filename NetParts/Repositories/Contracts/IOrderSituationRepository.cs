using NetParts.Models;

namespace NetParts.Repositories.Contracts
{
    public interface IOrderSituationRepository
    {
        void Create(OrderSituation orderSituation);
        void Update(OrderSituation orderSituation);
        void Delete(int id);
        OrderSituation GetOrderSituation(int id);
    }
}
