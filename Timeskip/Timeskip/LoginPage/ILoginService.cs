using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeskip.LoginPage
{
    interface ILoginService
    {
        bool Login(string username, string password);
    }
}
