using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Timeskip.Tools;
using Timeskip.ViewModel;
using Timeskip.API;

namespace Timeskip.Services.Login
{
    class LoginService : ILoginService
    {
        #region Private shizzle
        private int refreshExpire;
        #endregion
        #region Login + RefreshToken
        public string Login(string username, string password)
        {
            try
            {
                if (string.IsNullOrEmpty(username))
                    return "Empty username field";

                if (string.IsNullOrEmpty(password))
                    return "Empty password field";

                var client = new HttpClient();
                var content = string.Format("grant_type=password&client_id=timeskip&username={0}&password={1}", username, password);
                client.Timeout = new TimeSpan(0, 0, 5);
                var response = client.PostAsync(Properties.Resources.KeyCloakTokenUrl, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                if (!response.IsSuccessStatusCode)
                {
                    return "Username or password incorrect";
                }

                string result = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(result);
                refreshExpire = Convert.ToInt32(json["expires_in"].ToString());
                App.SaveToken(json["access_token"].ToString());
                App.SaveRefreshToken(json["refresh_token"].ToString());
                App.LoginSuccess.Invoke();

                //Refresh timer opnieuw laten lopen tot de user uitgelogd is 
                Device.StartTimer(TimeSpan.FromMilliseconds(refreshExpire), () =>
                {
                    if (App.LoggedIn)
                    {
                        RefreshToken();
                        return true;
                    }
                    else return false;
                });

                return string.Format("Hello {0}", UserApi.GetUserApi().GetCurrentUser().FirstName);
            }
            catch (AggregateException)
            {
                return "Could not connect to login server";
            }
            catch (WebException ex)
            {
                return ex.Message;
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }

        private void RefreshToken()
        {
            try
            {
                var client = new HttpClient();
                var content = string.Format("client_id=timeskip&grant_type=refresh_token&refresh_token={0}", App.RefreshToken);
                client.Timeout = new TimeSpan(0, 0, 5);

                var response = client.PostAsync(Properties.Resources.KeyCloakTokenUrl, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                string result = response.Content.ReadAsStringAsync().Result;
                var json = JObject.Parse(result);
                refreshExpire = Convert.ToInt32(json["expires_in"].ToString());
                App.SaveToken(json["access_token"].ToString());
                App.SaveRefreshToken(json["refresh_token"].ToString());
            }
            catch (Exception)
            {
                App.SaveToken(null);
                App.SaveRefreshToken(null);
                Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage.LoginPage());
                Popup.ShowPopupError("Error while refreshing the login token");
            }
        }
        #endregion
    }
}
