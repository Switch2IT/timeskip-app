using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Timeskip.Services.Login;
using Timeskip.Tools;

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
            if (string.IsNullOrEmpty(username))
                Popup.ShowPopupError("", "Empty username field");
            else if (string.IsNullOrEmpty(password))
                Popup.ShowPopupError("", "Empty password field");
            else if (loginService.Login(username, password))
                Popup.ShowPopupSuccess("", string.Format("{0} logged in", username), "OK");
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
