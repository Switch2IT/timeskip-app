using IO.Swagger.Client;
using IO.Swagger.Model;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using Timeskip.API;
using Xamarin.Forms;
using Timeskip.Tools;
using System.Linq;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;

namespace Timeskip.Services.Timesheet
{
    public class TSService : ITSService
    {
        public List<ActivityResponse> Activities(OrganizationResponse organization, ProjectResponse project)
        {
            try
            {
                if (project == null || organization == null)
                    return new List<ActivityResponse>();
                var response = OrgApi.GetOrgApi().ListProjectActivitiesWithHttpInfo(organization.Id, project.Id);
                if (response.StatusCode == 200)
                    return response.Data;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<ActivityResponse>();
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<ActivityResponse>();
            }
        }

        public List<ProjectResponse> AllProjects(OrganizationResponse organisation)
        {
            try
            {
                if (organisation == null)
                    return new List<ProjectResponse>();
                var response = OrgApi.GetOrgApi().ListProjectsWithHttpInfo(organisation.Id);
                if (response.StatusCode == 200)
                    return response.Data;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 403)
                {
                    var json = JObject.Parse(ex.ErrorContent);
                    string errorMessage = json["message"];
                    Popup.ShowPopupError(errorMessage);
                    return new List<ProjectResponse>();
                }

                Popup.ShowPopupError(ex.Message);
                return new List<ProjectResponse>();
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<ProjectResponse>();
            }
        }

        public List<OrganizationResponse> AllOrganisations()
        {
            try
            {
                var response = OrgApi.GetOrgApi().ListOrganizationsWithHttpInfo();
                if (response.StatusCode == 200)
                    return response.Data;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<OrganizationResponse>();
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<OrganizationResponse>();
            }
        }

        public bool PostWorklog(OrganizationResponse organization, ProjectResponse project, ActivityResponse activity, long minutes, string day)
        {
            try
            {
                if (project == null || organization == null || activity == null)
                {
                    Popup.ShowPopupError("Please fill in all the fields");
                    return false;
                }
                var response = OrgApi.GetOrgApi().LogWorkForCurrentUserWithHttpInfo(organization.Id, project.Id, activity.Id, new NewWorklogRequest(day, minutes, true));
                if (response.StatusCode == 201)
                    return true;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                if (ex.ErrorCode == 412)
                {
                    var json = JObject.Parse(ex.ErrorContent);
                    string errorMessage = json["message"];
                    Popup.ShowPopupError("User not assigned to project: " + errorMessage);
                    return false;
                }
                if(ex.ErrorCode == 400)
                {
                    var json = JObject.Parse(ex.ErrorContent);
                    string errorCode = json["errorCode"];
                    if (errorCode == "3000")
                    {
                        Popup.ShowPopupError("No overtime allowed for project: " + project.Name);
                        return false;
                    }
                    else
                    {
                        string message = json["message"];
                        Popup.ShowPopupError(message);
                        return false;
                    }
                }

                Popup.ShowPopupError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return false;
            }
        }

        public bool UpdateWorklog(WorklogResponse worklog, long? loggedMinutes, string day, OrganizationResponse organization, ProjectResponse project, ActivityResponse activity)
        {
            try
            {
                var updateWorklogrequest = new UpdateWorklogRequest(worklog.Id, UserApi.GetUserApi().GetCurrentUser().Id, day, loggedMinutes, true);
                var response = OrgApi.GetOrgApi().UpdateWorklogWithHttpInfo(organization.Id, project.Id, activity.Id, worklog.Id, updateWorklogrequest);
                if (response.StatusCode == 200)
                    return true;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                Popup.ShowPopupError(ex.Message);
                return false;
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return false;
            }
        }

        public List<WorklogResponse> WorklogsForPeriod(OrganizationResponse organization, DateTime from, DateTime to)
        {
            try
            {
                if (organization != null)
                {
                    List<WorklogResponse> worklogs = new List<WorklogResponse>();

                    var client = new HttpClient();
                    var path = string.Format(Properties.Resources.SwaggerUrl + "/users/current/worklogs?from={0}&to={1}", string.Format("{0:yyyy-MM-dd}", from), string.Format("{0:yyyy-MM-dd}", to));
                    client.Timeout = new TimeSpan(0, 0, 5);
                    client.DefaultRequestHeaders.Add("Authorization", "Bearer " + App.Token);
                    var request = client.GetAsync(path);
                    var result = request.Result;
                    string content = result.Content.ReadAsStringAsync().Result;
                    var json = JArray.Parse(content);
                    foreach (var worklog in json)
                    {
                        var organizationLocal = OrgApi.GetOrgApi().GetOrganization(worklog["activity"]["project"]["organization"]["id"].ToString());
                        if (organizationLocal.Id == organization.Id)
                        {
                            var project = AllProjects(organization).Where(p => p.Id == Convert.ToInt64(worklog["activity"]["project"]["id"])).FirstOrDefault();
                            if (project != null)
                            {
                                var activity = new ActivityResponse(Convert.ToInt64(worklog["activity"]["id"]), worklog["activity"]["name"].ToString(), worklog["activity"]["description"].ToString(), Convert.ToBoolean(worklog["activity"]["billable"].ToString()), project);
                                var workLogLocal = new WorklogResponse(Convert.ToInt32(worklog["id"]), worklog["userId"].ToString(), activity, Convert.ToDateTime(worklog["day"]), Convert.ToInt32(worklog["loggedMinutes"]), Convert.ToBoolean(worklog["confirmed"]));
                                worklogs.Add(workLogLocal);
                            }
                        }
                    }

                    return worklogs.OrderByDescending(w => w.Day).ToList();
                }
                else
                    return new List<WorklogResponse>();
            }
            catch (ApiException ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<WorklogResponse>();
            }
            catch (Exception ex)
            {
                Popup.ShowPopupError(ex.Message);
                return new List<WorklogResponse>();
            }
        }
    }
}
