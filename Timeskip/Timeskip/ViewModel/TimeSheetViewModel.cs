using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.Model;
using Timeskip.TSEntryPage;

namespace Timeskip.ViewModel
{
    public class TimesheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private shizzle
        private DateTime date;
        private int hours;
        private ITSService tsService;
        private Project selectedProject;
        private Activity selectedActivity;
        private List<Activity> activities;
        #endregion

        #region Constructor
        public TimesheetViewModel()
        {
            date = DateTime.Today;
            hours = 8;//todo: wordt nog vervangen door de default uren in de db voor de specifieke dag
            tsService = new TSService();
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

        public int Hours
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

        public List<Activity> Activities
        {
            get => activities ?? tsService.Activities(selectedProject);
            set
            {
                if (activities != value)
                {
                    activities = value;
                    OnPropertyChanged("Activities");
                }
            }
        }

        public Project SelectedProject
        {
            get => selectedProject;
            set
            {
                if (selectedProject != value)
                {
                    selectedProject = value;
                    OnPropertyChanged("SelectedProject");
                    Activities = tsService.Activities(selectedProject);
                }
            }
        }

        public Activity SelectedActivity
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
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        public List<Project> Projects => tsService.AllProjects();
    }
}
