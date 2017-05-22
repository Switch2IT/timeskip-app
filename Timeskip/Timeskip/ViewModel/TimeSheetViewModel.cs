using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Timeskip.Services.Timesheet;
using Timeskip.Tools;
using Timeskip.API;

namespace Timeskip.ViewModel
{
    public class TimesheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private shizzle
        private DateTime? date;
        private decimal hours;
        private ITSService tsService;
        private IUserService userService;
        private ProjectResponse selectedProject;
        private ActivityResponse selectedActivity;
        private List<ActivityResponse> activities;
        private OrganizationResponse selectedOrganization;
        private List<ProjectResponse> projects;
        private bool update = false, post = false;
        private WorklogResponse worklog;
        #endregion

        #region Constructor
        public TimesheetViewModel()
        {
            tsService = new TSService();
            userService = new UserService();
            date = DateTime.Today;
            Hours = DefaultHours();
            PostTimesheetCommand = new Command(PostTimesheet);
            post = true;
        }

        public TimesheetViewModel(WorklogResponse worklog)
        {
            tsService = new TSService();
            userService = new UserService();
            date = worklog.Day;
            Hours = Math.Round(Convert.ToDecimal(worklog.LoggedMinutes) / 60, 2);
            UpdateTimesheetCommand = new Command(UpdateTimesheet);
            update = true;
            SelectedOrganization = worklog.Activity.Project.Organization;
            SelectedProject = worklog.Activity.Project;
            selectedActivity = worklog.Activity;
            this.worklog = worklog;
        }
        #endregion

        #region Properties
        public DateTime? Date
        {
            get => date;
            set
            {
                if (date != value)
                {
                    date = value;
                    OnPropertyChanged("Date");
                }
            }
        }

        public decimal Hours
        {
            get => hours;
            set
            {
                if (hours != value)
                {
                    hours = value;
                    OnPropertyChanged("Hours");
                }
            }
        }

        public List<ActivityResponse> Activities
        {
            get => activities ?? tsService.Activities(selectedOrganization, selectedProject);
            set
            {
                if (activities != value)
                {
                    activities = value;
                    OnPropertyChanged("Activities");
                }
            }
        }

        public ProjectResponse SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    OnPropertyChanged("SelectedProject");
                    Activities = tsService.Activities(selectedOrganization, selectedProject);
                }
            }
        }

        public ActivityResponse SelectedActivity
        {
            get => selectedActivity;
            set
            {
                if (selectedActivity != value)
                {
                    selectedActivity = value;
                    OnPropertyChanged("SelectedActivity");
                }
            }
        }

        public List<ProjectResponse> Projects
        {
            get => projects ?? tsService.AllProjects(selectedOrganization);
            set
            {
                if (projects != value)
                {
                    projects = value;
                    OnPropertyChanged("Projects");
                }
            }
        }

        public OrganizationResponse SelectedOrganization
        {
            get => selectedOrganization;
            set
            {
                if (selectedOrganization != value)
                {
                    selectedOrganization = value;
                    OnPropertyChanged("SelectedOrganization");
                    Projects = tsService.AllProjects(selectedOrganization);
                }
            }
        }
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
        public bool Update => update;
        public bool Post => post;
        public List<OrganizationResponse> Organisations => tsService.AllOrganisations();

        public ICommand PostTimesheetCommand { get; private set; }
        public ICommand UpdateTimesheetCommand { get; private set; }

        private void PostTimesheet()
        {
            try
            {
                bool valid = CheckMandatoryFields() && CheckOvertime();
                var minutes = (long)Math.Round(hours * 60, 0);
                if (valid && tsService.PostWorklog(selectedOrganization, selectedProject, selectedActivity, minutes, string.Format("{0:yyyy-MM-dd}", date)))
                {
                    Popup.ShowPopupSuccess(hours + " hours logged for project: " + selectedProject.Name);
                    Application.Current.MainPage = new NavigationPage(new StartPage.StartPage());
                }
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
            }
        }

        private void UpdateTimesheet()
        {
            try
            {
                bool valid = CheckMandatoryFields() && CheckOvertime();
                var minutes = (long)Math.Round(hours * 60, 0);
                if (valid && tsService.UpdateWorklog(worklog, minutes, string.Format("{0:yyyy-MM-dd}", date), selectedOrganization, selectedProject, selectedActivity))
                {
                    Popup.ShowPopupSuccess("Worklog updated");
                    Application.Current.MainPage = new NavigationPage(new StartPage.StartPage());
                }
            }
            catch(Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
            }
        }

        private decimal DefaultHours()
        {
            try
            {
                return Convert.ToDecimal(userService.CurrentUserInfo().DefaultHoursPerDay);
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return 8;
            }
        }

        private bool CheckOvertime()
        {
            if (selectedProject.AllowOvertime == false && hours > 8)
            {
                Popup.ShowPopupError("No overtime allowed for selected project");
                Hours = DefaultHours();
                return false;
            }

            return true;
        }

        private bool CheckMandatoryFields()
        {
            if(selectedOrganization == null || selectedProject == null || selectedActivity == null)
            {
                Popup.ShowPopupError("Please fill in all fields");
                return false;
            }

            return true;
        }
    }
}
