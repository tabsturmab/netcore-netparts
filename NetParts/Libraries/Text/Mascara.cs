namespace NetParts.Libraries.Text
{
    public class Mascara
    {
        public static string Remover(string valor)
        {
            return valor.Replace("(", "").Replace(")", "").Replace("-", "").Replace(".", "").Replace("R$", "").Replace(",", "").Replace(" ", "");
        }
        public static int ConverterValorPagarMe(decimal valor)
        {
            string valorString = valor.ToString("C");
            valorString = Remover(valorString);
            int valorInt = int.Parse(valorString);

            return valorInt;
        }
        public static decimal ConverterPagarMeIntToDecimal(int valor)
        {
            string valorPagarMeString = valor.ToString();
            string valorDecimalString = valorPagarMeString.Substring(0, valorPagarMeString.Length - 2) + "," + valorPagarMeString.Substring(valorPagarMeString.Length - 2);

            return decimal.Parse(valorDecimalString);
        }
        public static int ExtractNumOrder(string numOrder, out string transactionId)
        {
            string[] resultSeparation = numOrder.Split("-");

            transactionId = resultSeparation[1];

            return int.Parse(resultSeparation[0]);
        }
    }
}
