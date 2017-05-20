using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timeskip.ViewTimesheets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SearchTimesheet : ContentPage
    {
        public SearchTimesheet()
        {
            InitializeComponent();
            BindingContext = new SearchTimesheetViewModel();
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