using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IO.Swagger.Model;
using IO.Swagger.Client;
using Xamarin.Forms;
using Timeskip.API;

namespace Timeskip.Services.Timesheet
{
    public class UserService : IUserService
    {
        public UserResponse CurrentUserInfo()
        {
            try
            {
                var response = UserApi.GetUserApi().GetCurrentUserWithHttpInfo();
                if (response.StatusCode == 200)
                    return response.Data;
                else
                    throw new ApiException();
            }
            catch (ApiException ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
            catch (Exception ex)
            {
                Application.Current.MainPage.DisplayAlert("Error", ex.Message, "OK");
                return null;
            }
        }
    }
}
