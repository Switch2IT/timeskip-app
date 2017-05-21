using IO.Swagger.Model;
using System;
using Timeskip.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timeskip.ViewTimesheets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TSOverviewPage : ContentPage
    {
        public TSOverviewPage(DateTime date, OrganizationResponse organization)
        {
            InitializeComponent();
            BindingContext = new TSOverviewViewModel(date, organization);
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