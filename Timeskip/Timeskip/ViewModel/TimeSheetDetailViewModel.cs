using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Timeskip.API;
using Timeskip.Services.Timesheet;
using Timeskip.Tools;
using Xamarin.Forms;

namespace Timeskip.ViewModel
{
    public class TimeSheetDetailViewModel
    {
        #region Private shizzle
        private WorklogResponse worklog;
        private UserResponse user = UserApi.GetUserApi().GetCurrentUser();
        private bool confirmed;
        ITSService tsService;
        #endregion

        #region Constructor
        public TimeSheetDetailViewModel(WorklogResponse worklog)
        {
            this.worklog = worklog;
            UpdateTimesheetCommand = new Command(UpdateTimesheet);
            ConfirmTimesheetCommand = new Command(ConfirmTimesheet);
            confirmed = Convert.ToBoolean(worklog.Confirmed);
        }
        #endregion

        #region Properties
        public WorklogResponse WorkLog => worklog;
        public string User => user.FirstName + " " + user.LastName;
        public bool ShowConfirm => !confirmed;
        public ICommand UpdateTimesheetCommand { get; private set; }
        public ICommand ConfirmTimesheetCommand { get; private set; }
        public string Date => string.Format("{0:dd/MM/yyyy}", worklog.Day);
        public string WorklogConfirmed => confirmed == true ? "Yes" : "No";
        #endregion

        private void UpdateTimesheet()
        {
            Application.Current.MainPage.Navigation.PushAsync(new TSEntryPage.TsEntryPage(worklog));
        }

        private async void ConfirmTimesheet()
        {
            try
            {
                tsService = new TSService();
                var confirmTimesheet = await Application.Current.MainPage.DisplayActionSheet("Confirm timesheet?", "Cancel", null, "Yes", "No");
                if (worklog.Activity.Project == null || worklog.Activity.Project.Organization == null)
                    Popup.ShowPopupError("Please fill in all fields");
                else if (confirmTimesheet == "Yes")
                {
                    tsService.UpdateWorklog(worklog, worklog.LoggedMinutes, string.Format("{0:yyyy-MM-dd}", worklog.Day), worklog.Activity.Project.Organization, worklog.Activity.Project, worklog.Activity);
                    await Application.Current.MainPage.Navigation.PopAsync();
                }
            }
            catch(Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
            }
        }
    }
}
