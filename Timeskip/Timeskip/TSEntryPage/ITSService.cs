using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Timeskip.Model;

namespace Timeskip.TSEntryPage
{
    interface ITSService
    {
        List<Project> AllProjects();
        List<Activity> Activities(Project project);
    }
}
