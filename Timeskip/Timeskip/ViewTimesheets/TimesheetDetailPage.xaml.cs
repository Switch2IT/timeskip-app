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
    public partial class TimesheetDetailPage : ContentPage
    {
        public TimesheetDetailPage(WorklogResponse worklog)
        {
            InitializeComponent();
            BindingContext = new TimeSheetDetailViewModel(worklog);
        }
    }
}
