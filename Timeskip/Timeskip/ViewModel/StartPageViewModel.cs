using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace Timeskip.ViewModel
{
    public class StartPageViewModel
    {
        public StartPageViewModel()
        {
            NewTimesheetCommand = new Command(NewTimesheet);
            LogoutCommand = new Command(Logout);
        }

        #region Properties
        public ICommand NewTimesheetCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        #endregion

        private void NewTimesheet()
        {
            App.Current.MainPage.Navigation.PushAsync(new TSEntryPage.TsEntryPage());
        }

        private void Logout()
        {
            App.SaveToken(null);
            App.Current.MainPage.Navigation.PushModalAsync(new LoginPage.LoginPage());
        }
    }
}
