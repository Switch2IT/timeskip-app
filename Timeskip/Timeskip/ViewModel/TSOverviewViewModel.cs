using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using Timeskip.Services.Timesheet;
using Xamarin.Forms;
using Timeskip.Tools;
using System.ComponentModel;
using System.Windows.Input;

namespace Timeskip.ViewModel
{
    public class TSOverviewViewModel : INotifyPropertyChanged
    {
        #region Private shizzle
        private DateTime date;
        private ITSService tsService;
        private OrganizationResponse organisation;
        private bool showWorklogList = false, showLabel = true;
        private List<WorklogResponse> worklogList;
        private WorklogResponse selectedWorklog;
        #endregion

        #region Constructor
        public TSOverviewViewModel(DateTime date, OrganizationResponse organisation)
        {
            this.date = date;
            tsService = new TSService();
            this.organisation = organisation;
            FillWorklogList();
            ViewDetailCommand = new Command(ViewDetail);
        }
        #endregion

        #region Properties
        public string Title => "Work logged for " + organisation.Name + " for " + date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year.ToString();
        public bool ShowWorklogList => showWorklogList;
        public bool ShowLabel => showLabel;
        public List<WorklogResponse> WorklogList => worklogList;
        public WorklogResponse SelectedWorklog
        {
            get => selectedWorklog;
            set
            {
                if (selectedWorklog != value)
                {
                    selectedWorklog = value;
                    ViewDetailCommand.Execute(selectedWorklog);
                    OnPropertyChanged("SelectedWorklog");
                }
            }
        }
        
        public ICommand ViewDetailCommand { get; private set; }

        public event PropertyChangedEventHandler PropertyChanged;
        #endregion

        private List<WorklogResponse> FillWorklogList()
        {
            try
            {
                worklogList = tsService.WorklogsForPeriod(organisation, new DateTime(date.Year, date.Month, 1), new DateTime(date.Year, date.Month, DateTime.DaysInMonth(date.Year, date.Month)));
                if(worklogList.Count > 0)
                {
                    showWorklogList = true;
                    showLabel = false;
                }

                return worklogList;
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<WorklogResponse>();
            }
        }

        private void ViewDetail()
        {
            Application.Current.MainPage.Navigation.PushAsync(new ViewTimesheets.TimesheetDetailPage(selectedWorklog));
        }

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
