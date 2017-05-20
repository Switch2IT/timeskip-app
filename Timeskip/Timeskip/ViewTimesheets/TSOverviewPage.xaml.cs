using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timeskip.ViewTimesheets
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TSOverviewPage : ContentPage
    {
        public TSOverviewPage(DateTime date)
        {
            InitializeComponent();
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