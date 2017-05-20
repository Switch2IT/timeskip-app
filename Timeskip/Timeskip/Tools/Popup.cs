using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Timeskip.Tools
{
    public class Popup
    {
        public static void ShowPopupError(string message)
        {
            Application.Current.MainPage.DisplayAlert("Error", message, "OK");
        }

        public static void ShowPopupError(string title, string message)
        {
            Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }
        public static void ShowPopupError(string title, string message, string cancel)
        {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }

        public static void ShowPopupSuccess(string title, string message)
        {
            Application.Current.MainPage.DisplayAlert(title, message, "OK");
        }

        public static void ShowPopupSuccess(string message)
        {
            Application.Current.MainPage.DisplayAlert("", message, "OK");
        }

        public static void ShowPopupSuccess(string title, string message, string cancel)
        {
            Application.Current.MainPage.DisplayAlert(title, message, cancel);
        }
    }
}
