using IO.Swagger.Client;
using IO.Swagger.Model;
using System;
using System.Collections.Generic;
using Timeskip.API;
using Xamarin.Forms;

namespace Timeskip.TSEntryPage
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<ActivityResponse>();
            }
        }


        //public List<ProjectResponse> AllProjects() => App.OrganisationsApi.ListProjects("canguru");
        //todo: dummy
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<ProjectResponse>();
            }
        }
        //end dummy
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
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return new List<OrganizationResponse>();
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "Cancel");
                return new List<OrganizationResponse>();
            }
        }
    }
}
