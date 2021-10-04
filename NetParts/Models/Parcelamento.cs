namespace NetParts.Models
{
    public class Parcelamento
    {
        public int Numero { get; set; }
        public decimal Valor { get; set; }
        public decimal ValorPorParcela { get; set; }
        public bool Juros { get; set; }
    }
}
