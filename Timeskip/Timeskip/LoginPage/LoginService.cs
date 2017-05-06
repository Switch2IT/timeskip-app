using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeskip.LoginPage
{
    //todo: login dummy, wordt vervangen door echte login
    class LoginService : ILoginService
    {
        private Dictionary<string, string> users;

        public LoginService()
        {
            users = new Dictionary<string, string>();
            FillDummyUsersDB();
        }

        public bool Login(string username, string password)
        {
            try
            {
                if (users.ContainsKey(username) && users[username] == password)
                    return true;
                else
                    return false;
            }
            catch
            {
                return false;
            }
        }

        private void FillDummyUsersDB()
        {
            users.Add("otje", "banaan");
            users.Add("obi", "wan");
            users.Add("jef", "patat");
        }
    }
}
