using Microsoft.AspNetCore.Http;

namespace NetParts.Libraries.Session
{
    public class Session
    {
        IHttpContextAccessor _context;
        public Session(IHttpContextAccessor context)
        {
            _context = context;
        }
        public void Create(string Key, string Value)
        {
            _context.HttpContext.Session.SetString(Key, Value);
        }
        public void Update(string Key, string Value)
        {
            if (Exist(Key))
            {
                _context.HttpContext.Session.Remove(Key);
            }
            
            _context.HttpContext.Session.SetString(Key, Value);
        }
        public void Remove(string Key)
        {
            _context.HttpContext.Session.Remove(Key);
        }
        public string Consult(string Key)
        {
            return _context.HttpContext.Session.GetString(Key);
        }
        public bool Exist(string Key)
        {
            if (_context.HttpContext.Session.GetString(Key) == null)
            {
                return false;
            }
            return true;
        }
        public void RemoveAll()
        {
            _context.HttpContext.Session.Clear();
        }
    }
}
