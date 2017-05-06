using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeskip.ViewModel
{
    public class TimesheetViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        #region private shizzle
        private DateTime date;
        private int hours;
        #endregion

        #region Constructor
        public TimesheetViewModel()
        {
            date = DateTime.Today;
            hours = 8;//todo: wordt nog vervangen door de default uren in de db voor de specifieke dag
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
        #endregion

        protected virtual void OnPropertyChanged(string propertyName)
        {
            var changed = PropertyChanged;
            if (changed != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
