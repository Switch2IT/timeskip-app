using Timeskip.ViewModel;

namespace Timeskip.Services.Login
{
    interface ILoginService
    {
        string Login(string username, string password);
    }
}
