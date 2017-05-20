using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using Timeskip.TSEntryPage;
using Xamarin.Forms;
using Timeskip.Services.Timesheet;

namespace Timeskip.ViewModel
{
    public class TimesheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private shizzle
        private DateTime date;
        private decimal hours;
        private ITSService tsService;
        private ProjectResponse selectedProject;
        private ActivityResponse selectedActivity;
        private List<ActivityResponse> activities;
        private OrganizationResponse selectedOrganization;
        private List<ProjectResponse> projects;
        #endregion

        #region Constructor
        public TimesheetViewModel()
        {
            date = DateTime.Today;
            hours = 8;//todo: wordt nog vervangen door de default uren in de db voor de specifieke dag
            tsService = new TSService();
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
                var minutes = (long)Math.Round(hours * 60, 0);
                if (tsService.PostWorklog(selectedOrganization, selectedProject, selectedActivity, minutes, string.Format("{0:yyyy-MM-dd}", date)))
                    Application.Current.MainPage.DisplayAlert("", "Work logged", "OK");
                else
                    Application.Current.MainPage.DisplayAlert("Error", "Work not logged", "OK");
            }
            catch(Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
            }
        }
    }
}
