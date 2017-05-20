using IO.Swagger.Client;

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
