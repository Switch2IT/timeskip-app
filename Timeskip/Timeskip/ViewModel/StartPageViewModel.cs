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
            ViewTimesheetCommand = new Command(ViewTimesheet);
        }

        #region Properties
        public ICommand NewTimesheetCommand { get; private set; }
        public ICommand LogoutCommand { get; private set; }
        public ICommand ViewTimesheetCommand { get; private set; }
        #endregion

        #region Button logica
        private void NewTimesheet()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TSEntryPage.TsEntryPage());
        }

        private void ViewTimesheet()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ViewTimesheets.SearchTimesheet());
        }

        private void Logout()
        {
            App.SaveToken(null);
            App.SaveRefreshToken(null);
            Application.Current.MainPage.Navigation.PushModalAsync(new LoginPage.LoginPage());
        }
        #endregion
    }
}
