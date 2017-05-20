using Newtonsoft.Json.Linq;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using Xamarin.Forms;
using Timeskip.Tools;

namespace Timeskip.Services.Login
{
    class LoginService : ILoginService
    {
        #region Private shizzle
        private int refreshExpire;
        #endregion
        #region Login + RefreshToken
        public bool Login(string username, string password)
        {
            try
            {
                var client = new HttpClient();
                var content = string.Format("grant_type=password&client_id=timeskip&username={0}&password={1}", username, password);
                client.Timeout = new TimeSpan(0, 0, 5);
                var response = client.PostAsync(Properties.Resources.KeyCloakTokenUrl, new StringContent(content, Encoding.UTF8, "application/x-www-form-urlencoded")).Result;

                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    Popup.ShowPopupError("Login error", "Username or password incorrect");
                    return false;
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
                return true;
            }
            catch (AggregateException)
            {
                Popup.ShowPopupError("Could not connect to login server");
                return false;
            }
            catch (WebException ex)
            {
                Popup.ShowPopupError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return false;
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
