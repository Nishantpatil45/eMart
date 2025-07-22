using Microsoft.Extensions.Configuration;

namespace eMart.Service.Core.Helper
{
    public class ConfigurationManager
    {
        public static IConfiguration AppSetting
        {
            get;
        }

        static ConfigurationManager()
        {
            AppSetting = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json").Build();
        }
    }
}
