using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Timeskip.Services.Login;
using Timeskip.Tools;
using System;
using System.Threading.Tasks;
using Timeskip.API;

namespace Timeskip.ViewModel
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private shizzle
        private string username;
        private string password;
        ILoginService loginService;
        private bool loading = false;
        #endregion

        public LoginPageViewModel()
        {
            loginService = new LoginService();
            LoginCommand = new Command(Login);
        }

        #region Properties
        public string UserName
        {
            get => username;
            set
            {
                if (username != value)
                {
                    username = value;
                    OnPropertyChanged("UserName");
                }
            }
        }

        public string Password
        {
            get => password;
            set
            {
                if (password != value)
                {
                    password = value;
                    OnPropertyChanged("Password");
                }
            }
        }

        public bool Loading
        {
            get => loading;
            set
            {
                if (loading != value)
                {
                    loading = value;
                    OnPropertyChanged("Loading");
                }
            }
        }

        public ICommand LoginCommand { get; private set; }
        #endregion

        private async void Login()
        {
            try
            {
                Loading = true;
                string message = await Task.Run(() => loginService.Login(username, password));
                if (!message.StartsWith("Hello"))
                    Popup.ShowPopupError("Login Error", message);
                else
                    Popup.ShowPopupSuccess(message);
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
            }
            finally
            {
                Loading = false;
            }
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
