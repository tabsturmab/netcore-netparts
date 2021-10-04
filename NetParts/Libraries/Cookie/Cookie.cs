using System;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using NetParts.Libraries.Security;

namespace NetParts.Libraries.Cookie
{
    public class Cookie
    {
        private IHttpContextAccessor _context;
        private IConfiguration _configuration;
        public Cookie(IHttpContextAccessor context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }
        public void Create(string Key, string Value)
        {
            CookieOptions Options = new CookieOptions();
            Options.Expires = DateTime.Now.AddDays(5);
            Options.IsEssential = true;

            var valueCrypt = StringCipher.Encrypt(Value, _configuration.GetValue<string>("KeyCrypt"));

            _context.HttpContext.Response.Cookies.Append(Key, valueCrypt, Options);
        }
        public void Update(string Key, string Value)
        {
            if (Exist(Key))
            {
                Remove(Key);
            }
            Create(Key, Value);
        }
        public void Remove(string Key)
        {
            _context.HttpContext.Response.Cookies.Delete(Key);
        }
        public string Consult(string Key, bool Cript = true)
        {
            var valor = _context.HttpContext.Request.Cookies[Key];
            if (Cript)
            {
                if(valor != null)
                    valor = StringCipher.Decrypt(valor, _configuration.GetValue<string>("KeyCrypt"));
            }
            return valor;
        }
        public bool Exist(string Key)
        {
            if (_context.HttpContext.Request.Cookies[Key] == null)
            {
                return false;
            }

            return true;
        }
        public void RemoveAll()
        {
            var ListCookie = _context.HttpContext.Request.Cookies.ToList();
            foreach (var cookie in ListCookie)
            {
                Remove(cookie.Key);
            }

        }
    }
}
