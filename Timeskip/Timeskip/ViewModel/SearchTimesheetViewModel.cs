using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Windows.Input;
using Timeskip.Services.Timesheet;
using Timeskip.ViewTimesheets;
using Xamarin.Forms;

namespace Timeskip.ViewModel
{
    public class SearchTimesheetViewModel : INotifyPropertyChanged
    {
        #region Private shizzle
        private string[] months = new string[12];
        private int[] years = new int[5];
        private string month;
        private int year;
        private ITSService tsService;
        private OrganizationResponse selectedOrganization;
        #endregion

        #region Constructor
        public SearchTimesheetViewModel()
        {
            FillMonths();
            FillYears();
            SearchTimesheetCommand = new Command(SearchTimesheet);
            tsService = new TSService();
        }
        #endregion

        public event PropertyChangedEventHandler PropertyChanged;

        #region Properties
        public string Month
        {
            get => month;
            set
            {
                if (month != value)
                {
                    month = value;
                    OnPropertyChanged("Month");
                }
            }
        }

        public int Year
        {
            get => year;
            set
            {
                if (year != value)
                {
                    year = value;
                    OnPropertyChanged("Year");
                }
            }
        }

        public List<OrganizationResponse> Organizations => tsService.AllOrganisations();

        public OrganizationResponse SelectedOrganization
        {
            get => selectedOrganization;
            set
            {
                if (selectedOrganization != value)
                {
                    selectedOrganization = value;
                    OnPropertyChanged("SelectedOrganization");
                }
            }
        }

        public ICommand SearchTimesheetCommand { get; private set; }

        public string[] Months => months;
        public int[] Years => years;
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }

        #region Command functionality
        private void SearchTimesheet()
        {
            int monthLocal = DateTime.ParseExact(Month, "MMMM", CultureInfo.CurrentCulture).Month;
            DateTime date = new DateTime(Year, monthLocal, DateTime.Now.Day);
            Application.Current.MainPage.Navigation.PushAsync(new TSOverviewPage(date, selectedOrganization));
        }
        #endregion

        #region Fill arrays
        private void FillMonths()
        {
            DateTime date = DateTime.Now;
            Array.Clear(months, 0, 12);
            for (int i = 0; i < 12; i++)
            {
                months[i] = date.ToString("MMMM", CultureInfo.CurrentCulture);
                date = date.AddMonths(-1);
            }
        }

        private void FillYears()
        {
            DateTime date = DateTime.Now;
            Array.Clear(years, 0, 5);
            for (int i = 0; i < 5; i++)
            {
                years[i] = date.Year;
                date = date.AddYears(-1);
            }
        }
        #endregion
    }
}
