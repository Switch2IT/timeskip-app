using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Timeskip.API;
using Xamarin.Forms;

namespace Timeskip.ViewModel
{
    public class TimeSheetDetailViewModel
    {
        #region Private shizzle
        private WorklogResponse worklog;
        private UserResponse user = UserApi.GetUserApi().GetCurrentUser();
        #endregion

        #region Constructor
        public TimeSheetDetailViewModel(WorklogResponse worklog)
        {
            this.worklog = worklog;
            UpdateTimesheetCommand = new Command(UpdateTimesheet);
        }
        #endregion

        #region Properties
        public WorklogResponse WorkLog => worklog;
        public string User => user.FirstName + " " + user.LastName;
        public ICommand UpdateTimesheetCommand { get; private set; }
        #endregion

        private void UpdateTimesheet()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TSEntryPage.TsEntryPage(worklog));
        }
    }
}
