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
        List<Project> projects;
        List<Activity> activities;

        public TSService()
        {
            FillProjects();
            FillActivities();
        }

        public List<Activity> Activities(Project project)
        {
            if (project != null)
                return activities.Where(a => a.ProjectId == project.ProjectID).ToList();
            else
                return activities;
        }

        public List<Project> AllProjects() => projects;

        private void FillProjects()
        {
            projects = new List<Project>();
            projects.Add(new Project { ProjectID = 1, Name = "Canguru" });
            projects.Add(new Project { ProjectID = 2, Name = "Test project" });
        }

        private void FillActivities()
        {
            activities = new List<Activity>();
            activities.Add(new Activity { ActivityId = 1, Name = "Ziek", ProjectId = 1 });
            activities.Add(new Activity { ActivityId = 2, Name = "Training", ProjectId = 1 });
            activities.Add(new Activity { ActivityId = 3, Name = "Test activity", ProjectId = 2 });
        }
    }
}
