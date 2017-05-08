using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;
using IO.Swagger.Client;
using IO.Swagger.Api;

namespace Timeskip
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();

            ApiClient client = new ApiClient();
            Configuration config = new Configuration(client);

            OrganisationsApi = new OrganizationsApi(config);

            MainPage = new LoginPage.LoginPage();
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }

        #region App Properties
        public static OrganizationsApi OrganisationsApi { get; set; }
        #endregion
    }
}
