using System.Collections.Generic;
using NetParts.Models.ProductAggregator;

namespace NetParts.Models
{
    public class TransactionAdvertisement
    {
        public TransacaoPagarMe Transaction { get; set; }
        public List<ProductItem> Advertisement { get; set; }
    }
}
