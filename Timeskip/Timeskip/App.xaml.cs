using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Xamarin.Forms;
using IO.Swagger.Client;
using IO.Swagger.Api;

namespace Timeskip
{
    public partial class App : Application
    {
        #region Private shizzle
        static NavigationPage navigationPage;
        static string token;
        static string refreshToken;
        #endregion

        public App()
        {
            InitializeComponent();

            navigationPage = new NavigationPage(new StartPage.StartPage());
            
            ApiClient client = new ApiClient();
            Configuration config = new Configuration(client);

            OrganisationsApi = new OrganizationsApi(config);

            MainPage = navigationPage;
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

        #region Login operations
        public static bool LoggedIn => !string.IsNullOrWhiteSpace(token);
        public static string Token => token;
        public static void SaveToken(string _token)
        {
            token = _token;
        }
        public static string RefreshToken => refreshToken;
        public static void SaveRefreshToken(string _token)
        {
            refreshToken = _token;
        }
        public static Action LoginSuccess => new Action(() => navigationPage.Navigation.PopModalAsync());

        #endregion

        #region App Properties
        public static OrganizationsApi OrganisationsApi { get; set; }
        #endregion
    }
}
