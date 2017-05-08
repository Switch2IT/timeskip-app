using IO.Swagger.Model;
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
        List<OrganizationResponse> AllOrganisations();
        List<ProjectResponse> AllProjects();
        List<Activity> Activities(ProjectResponse project);
    }
}
