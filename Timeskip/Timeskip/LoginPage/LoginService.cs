using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Timeskip.LoginPage
{
    class LoginService : ILoginService
    {
        public bool Login(string username, string password)
        {
            try
            {
                var client = new HttpClient();
                var content = string.Format("grant_type=password&client_id=timeskip&username={0}&password={1}", username, password);
                client.Timeout = new TimeSpan(0, 0, 5);
                var response = client.PostAsync("http://10.3.50.37/auth/realms/canguru/protocol/openid-connect/token", new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Application.Current.MainPage.DisplayAlert("Login Error", "Username or password incorrect", "Cancel");
                    return false;
                }

                string result = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(result);
                App.SaveToken(json["access_token"].ToString());
                App.SaveRefreshToken(json["refresh_token"].ToString());
                App.LoginSuccess.Invoke();
                return true;
            }
            catch(AggregateException)
            {
                Application.Current.MainPage.DisplayAlert("Error", "Error when contacting login server", "Cancel");
                return false;
            }
            catch(WebException ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                return false;
            }
            catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                return false;
            }
        }
    }
}
