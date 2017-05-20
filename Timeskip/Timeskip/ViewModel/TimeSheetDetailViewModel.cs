using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.API;

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
        }
        #endregion

        #region Properties
        public WorklogResponse WorkLog => worklog;
        public string User => user.FirstName + " " + user.LastName;
        #endregion
    }
}
