﻿
using Android.App;
using Android.Content.PM;
using Android.OS;

namespace Timeskip.Droid
{
    [Activity(Label = "Timeskip", Icon = "@drawable/logoPNG", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(bundle);

            global::Xamarin.Forms.Forms.Init(this, bundle);
            LoadApplication(new App());
        }

        public override void OnBackPressed()
        {
            if (!App.LoggedIn) return;

            base.OnBackPressed();
        }
    }
}

