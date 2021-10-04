using System.Collections.Generic;
using System.Linq;
using NetParts.Models.ProductAggregator;
using Newtonsoft.Json;

namespace NetParts.Libraries.ShoppingCart
{
    public class CookieShoppingCart
    {
        private string Key = "Cart.Shopping";
        private Cookie.Cookie _cookie;
        public CookieShoppingCart(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }
        public void Create(ProductItem item)
        {
            List<ProductItem> List;
            if (_cookie.Exist(Key))
            {
                List = Consult();
                var ItemLocated = List.SingleOrDefault(a => a.IdAdvert == item.IdAdvert);
                
                if (ItemLocated == null)
                {
                    List.Add(item);
                }
                else
                {
                    ItemLocated.QuantityProduct = ItemLocated.QuantityProduct + 1;
                }
            }
            else
            {
                List = new List<ProductItem>();
                List.Add(item);
            }
            Save(List);
        }
        public void Update(ProductItem item)
        {
            var List = Consult();
            var ItemLocated = List.SingleOrDefault(a => a.IdAdvert == item.IdAdvert);

            if (ItemLocated != null)
            {
                ItemLocated.QuantityProduct = item.QuantityProduct;
                Save(List);
            }
        }
        public void Remove(ProductItem item)
        {
            var List = Consult();
            var ItemLocated = List.SingleOrDefault(a => a.IdAdvert == item.IdAdvert);

            if (ItemLocated != null)
            {
                List.Remove(ItemLocated);
                Save(List);
            }
        }
        public List<ProductItem> Consult()
        {
            if (_cookie.Exist(Key))
            {
                string valor = _cookie.Consult(Key);
                return JsonConvert.DeserializeObject<List<ProductItem>>(valor);
            }
            else
            {
                return new List<ProductItem>();
            }
        }
        public void Save(List<ProductItem> List)
        {
            string Valor = JsonConvert.SerializeObject(List);
            _cookie.Update(Key, Valor);
        }

        public bool Exist(string Key)
        {
            if (_cookie.Exist(Key))
            {
                return false;
            }

            return true;
        }
        public void RemoveAll()
        {
            _cookie.Remove(Key);
        }

    }
}
