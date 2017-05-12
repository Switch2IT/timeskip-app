using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
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
                var client = new HttpClient();
                var content = string.Format("grant_type=password&client_id=timeskip&username={0}&password={1}", username, password);
                var response = client.PostAsync("http://10.3.50.37/auth/realms/canguru/protocol/openid-connect/token", new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                    return false;

                string result = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(result);
                App.SaveToken(json["access_token"].ToString());
                App.LoginSuccess.Invoke();
                return true;
            }
            catch(Exception ex)
            {
                App.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
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
