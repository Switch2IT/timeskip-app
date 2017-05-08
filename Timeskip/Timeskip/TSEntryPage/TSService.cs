using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.Model;

namespace Timeskip.TSEntryPage
{
    public class TSService : ITSService
    {
        List<Activity> activities;

        public TSService()
        {
            FillActivities();
        }

        public List<Activity> Activities(ProjectResponse project)
        {
            if (project != null)
                return activities.Where(a => a.ProjectId == project.Id).ToList();
            else
                return activities;
        }


        //public List<ProjectResponse> AllProjects() => App.OrganisationsApi.ListProjects("canguru");
        //todo: dummy
        public List<ProjectResponse> AllProjects() => new List<ProjectResponse>();
        //end dummy
        public List<OrganizationResponse> AllOrganisations() => App.OrganisationsApi.ListOrganizations();

        private void FillActivities()
        {
            activities = new List<Activity>();
            activities.Add(new Activity { ActivityId = 1, Name = "Ziek", ProjectId = 1 });
            activities.Add(new Activity { ActivityId = 2, Name = "Training", ProjectId = 1 });
            activities.Add(new Activity { ActivityId = 3, Name = "Test activity", ProjectId = 2 });
        }
    }
}
