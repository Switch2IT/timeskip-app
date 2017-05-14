using IO.Swagger.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Timeskip.API
{
    public class ApiConfig
    {
        private static Configuration config;

        private ApiConfig()
        {
        }

        public static Configuration GetConfig()
        {
            if (config == null)
            {
                config = new Configuration(SwaggerClient.GetClient());
            }

            return config;
        }
    }
}
