using System.Security.Cryptography;
using System.Text;

namespace NetParts.Libraries.Security
{
    public class Hash
    {
        private HashAlgorithm _algoritmo;
        public Hash()
        {
            _algoritmo = SHA512.Create();
        }
        public string CriptografarSenha(string password)
        {
            if (password != null)
            {
                var encodedValue = Encoding.UTF8.GetBytes(password);
                var encryptedPassword = _algoritmo.ComputeHash(encodedValue);

                var sb = new StringBuilder();
                foreach (var caracter in encryptedPassword)
                {
                    sb.Append(caracter.ToString("X2"));
                }
                return sb.ToString();
            }

            return null;
        }
    }
}

