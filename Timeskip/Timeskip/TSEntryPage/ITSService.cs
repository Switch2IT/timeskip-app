using IO.Swagger.Model;
using System.Collections.Generic;

namespace Timeskip.TSEntryPage
{
    interface ITSService
    {
        List<OrganizationResponse> AllOrganisations();
        List<ProjectResponse> AllProjects(OrganizationResponse organization);
        List<ActivityResponse> Activities(OrganizationResponse organization, ProjectResponse project);
    }
}
