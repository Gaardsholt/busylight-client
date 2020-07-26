using Microsoft.Extensions.Configuration;
using System.Globalization;
using System.IO;
using System.Resources;

namespace busylight_client
{
    public class Settings
    {
        public ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        private static IConfigurationRoot Configuration { get; set; }

        public string Location => Configuration.GetValue("AppSettings:Location", "Brande");
        public string SignalR_Uri => Configuration.GetValue<string>("AppSettings:SignalR_Uri");
        public string ApiKey => Configuration.GetValue<string>("AppSettings:ApiKey");
        public string KeyName => Configuration.GetValue("AppSettings:KeyName", "ApiKey");
        public string Ring_Tune => Configuration.GetValue("AppSettings:Ring_Tune", "OpenOffice");
        public string Ring_Color => Configuration.GetValue("AppSettings:Ring_Color", "Red");
        public int Ring_Time => Configuration.GetValue("AppSettings:Ring_Time", 5000);
        public string Idle_Color => Configuration.GetValue("AppSettings:Idle_Color", "Off");

        public Settings()
        {
            Configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .Build();

        }

    }
}
