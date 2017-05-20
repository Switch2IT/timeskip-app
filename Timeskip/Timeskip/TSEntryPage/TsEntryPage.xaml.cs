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
