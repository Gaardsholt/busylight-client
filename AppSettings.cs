using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Resources;
using System.Text;

namespace busylight_client
{
    public class Settings
    {
        public ResourceSet resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
        private IConfigurationRoot Configuration { get; set; }

        public string Location => Configuration["AppSettings:Location"];
        public string SignalR_Uri => Configuration["AppSettings:SignalR_Uri"];
        public int Ring_Time
        {
            get
            {
                int.TryParse(Configuration["AppSettings:Ring_Time"], out int result);
                return result == 0 ? 5000 : result;
            }
        }


        public Settings()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            Configuration = builder.Build();
        }
    }
}
