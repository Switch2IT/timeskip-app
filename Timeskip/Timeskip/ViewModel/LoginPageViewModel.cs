using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Timeskip.LoginPage;
using Xamarin.Forms;

namespace Timeskip.ViewModel
{
    public class LoginPageViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region Private shizzle
        private string username;
        private string password;
        ILoginService loginService;
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

        public ICommand LoginCommand { get; private set; }
        #endregion

        private void Login()
        {
            if (!loginService.Login(username, password))
                App.Current.MainPage.DisplayAlert("Error", "Login error", "Cancel");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
