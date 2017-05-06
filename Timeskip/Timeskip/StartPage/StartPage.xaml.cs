using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Timeskip.StartPage
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
            BindingContext = new StartPageViewModel();
        }
    }
}
