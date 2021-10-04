using System.Collections.Generic;

namespace NetParts.Models
{
    public class Frete
    {
        public int CEP { get; set; }
        public string CodCart { get; set; }
        public List<ValorPrazoFrete> ListValues { get; set; }
    }
}
