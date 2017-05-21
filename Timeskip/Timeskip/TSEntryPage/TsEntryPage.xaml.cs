using IO.Swagger.Model;
using Timeskip.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timeskip.TSEntryPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TsEntryPage : ContentPage
    {
        public TsEntryPage()
        {
            InitializeComponent();
            BindingContext = new TimesheetViewModel();
        }

        public TsEntryPage(WorklogResponse worklog)
        {
            InitializeComponent();
            BindingContext = new TimesheetViewModel(worklog);
        }
        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (!App.LoggedIn)
            {
                Navigation.PushModalAsync(new LoginPage.LoginPage());
            }
        }
    }
}
