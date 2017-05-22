using IO.Swagger.Model;
using System;
using System.Collections.Generic;

namespace Timeskip.Services.Timesheet
{
    interface ITSService
    {
        List<OrganizationResponse> AllOrganisations();
        List<ProjectResponse> AllProjects(OrganizationResponse organization);
        List<ActivityResponse> Activities(OrganizationResponse organization, ProjectResponse project);
        bool PostWorklog(OrganizationResponse organization, ProjectResponse project, ActivityResponse activity, long minutes, string day);
        List<WorklogResponse> WorklogsForPeriod(OrganizationResponse organisation, DateTime from, DateTime to);
        bool UpdateWorklog(WorklogResponse worklog, long? loggedMinutes, string day, OrganizationResponse organization, ProjectResponse project, ActivityResponse activity);
    }
}
