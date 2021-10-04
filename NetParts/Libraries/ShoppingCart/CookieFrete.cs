using System.Collections.Generic;
using System.Linq;
using NetParts.Models;
using Newtonsoft.Json;

namespace NetParts.Libraries.ShoppingCart
{
    public class CookieFrete
    {
        private string Key = "Cart.ValorFrete";
        private Cookie.Cookie _cookie;
        public CookieFrete(Cookie.Cookie cookie)
        {
            _cookie = cookie;
        }
        public void Create(Frete item)
        {
            List<Frete> List;
            if (_cookie.Exist(Key))
            {
                List = Consult();
                var ItemLocated = List.SingleOrDefault(a => a.CEP == item.CEP);

                if (ItemLocated == null)
                {
                    List.Add(item);
                }
                else
                {
                    ItemLocated.CodCart = item.CodCart;
                    ItemLocated.ListValues = item.ListValues;
                }
            }
            else
            {
                List = new List<Frete>();
                List.Add(item);
            }
            Save(List);
        }
        public void Update(Frete item)
        {
            var List = Consult();
            var ItemLocated = List.SingleOrDefault(a => a.CEP == item.CEP);

            if (ItemLocated != null)
            {
                ItemLocated.CodCart = item.CodCart;
                ItemLocated.ListValues = item.ListValues;
                Save(List);
            }
        }
        public void Remove(Frete item)
        {
            var List = Consult();
            var ItemLocated = List.SingleOrDefault(a => a.CEP == item.CEP);

            if (ItemLocated != null)
            {
                List.Remove(ItemLocated);
                Save(List);
            }
        }
        public List<Frete> Consult()
        {
            if (_cookie.Exist(Key))
            {
                string valor = _cookie.Consult(Key);
                return JsonConvert.DeserializeObject<List<Frete>>(valor);
            }
            else
            {
                return new List<Frete>();
            }
        }
        public void Save(List<Frete> List)
        {
            string Valor = JsonConvert.SerializeObject(List);
            _cookie.Create(Key, Valor);
        }
        public bool Exist(string Key)
        {
            if (_cookie.Exist(Key))
            {
                return false;
            }
            return true;
        }
        public void RemoverAll()
        {
            _cookie.Remove(Key);
        }
    }
}
