using NetParts.Models;
using Newtonsoft.Json;

namespace NetParts.Libraries.Login
{
    public class LoginCollaborator
    {
        private string Key = "Login.Collaborator";
        private Session.Session _session;

        public LoginCollaborator(Session.Session session)
        {
            _session = session;
        }
        public void Login(Collaborator collaborator)
        {
            string collaboratorJSONString = JsonConvert.SerializeObject(collaborator);
            _session.Create(Key, collaboratorJSONString);
        }
        public Collaborator GetCollaborator()
        {
            if (_session.Exist(Key))
            {
                string collaboratorJSONString = _session.Consult(Key);
                return JsonConvert.DeserializeObject<Collaborator>(collaboratorJSONString);
            }
            else
            {
                return null;
            }
        }
        public void Logout()
        {
            _session.RemoveAll();
        }
    }
}
