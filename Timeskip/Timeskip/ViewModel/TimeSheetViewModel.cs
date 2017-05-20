using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using Timeskip.Services.Timesheet;
using Timeskip.Tools;

namespace Timeskip.ViewModel
{
    public class TimesheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private shizzle
        private DateTime date;
        private decimal hours;
        private ITSService tsService;
        private IUserService userService;
        private ProjectResponse selectedProject;
        private ActivityResponse selectedActivity;
        private List<ActivityResponse> activities;
        private OrganizationResponse selectedOrganization;
        private List<ProjectResponse> projects;

        #endregion

        #region Constructor
        public TimesheetViewModel()
        {
            tsService = new TSService();
            userService = new UserService();
            date = DateTime.Today;
            Hours = DefaultHours();
            PostTimesheetCommand = new Command(PostTimesheet);
        }
        #endregion

        #region Properties
        public DateTime Date
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

        public List<OrganizationResponse> Organisations => tsService.AllOrganisations();

        public ICommand PostTimesheetCommand { get; private set; }

        private void PostTimesheet()
        {
            try
            {
                bool valid = true;

                if (selectedProject.AllowOvertime == false && hours > 8)
                {
                    valid = false;
                    Popup.ShowPopupError("No overtime allowed for selected project");
                    Hours = DefaultHours();
                }

                var minutes = (long)Math.Round(hours * 60, 0);
                if (valid && tsService.PostWorklog(selectedOrganization, selectedProject, selectedActivity, minutes, string.Format("{0:yyyy-MM-dd}", date)))
                    Popup.ShowPopupSuccess(hours + " logged for project: " + selectedProject);
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
            }
        }

        private decimal DefaultHours()
        {
            //todo: nog checken of de user op de huidige dag werkt
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
    }
}
