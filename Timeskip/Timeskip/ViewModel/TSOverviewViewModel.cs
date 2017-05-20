using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using Timeskip.Services.Timesheet;
using Xamarin.Forms;
using Timeskip.Tools;

namespace Timeskip.ViewModel
{
    public class TSOverviewViewModel
    {
        #region Private shizzle
        private DateTime date;
        private ITSService tsService;
        private OrganizationResponse organisation;
        private bool showWorklogList = false, showLabel = true;
        private List<WorklogResponse> worklogList;
        #endregion

        #region Constructor
        public TSOverviewViewModel(DateTime date, OrganizationResponse organisation)
        {
            this.date = date;
            tsService = new TSService();
            this.organisation = organisation;
            FillWorklogList();
        }
        #endregion

        #region Properties
        public string Title => "Work logged for " + organisation.Name + " for " + date.ToString("MMMM", CultureInfo.CurrentCulture) + " " + date.Year.ToString();
        public bool ShowWorklogList => showWorklogList;
        public bool ShowLabel => showLabel;
        public List<WorklogResponse> WorklogList => worklogList;
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
    }
}
