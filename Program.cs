using Microsoft.AspNetCore.Connections.Features;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busylight_client
{
    static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.SetHighDpiMode(HighDpiMode.SystemAware);
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);




            var settings = new Settings();
            var menuGen = new MenuGenerator(settings);

            var notifyIcon1 = new NotifyIcon()
            {
                Icon = settings.resourceSet.GetObject("icon") as Icon,
                ContextMenuStrip = menuGen.Menu,
                Text = "Busylight client",
                Visible = true
            };



            var serverConnect = new ServerConnect(settings, menuGen.Menu.Items);
            Task.Run(() => serverConnect.Connect());

            

            Application.Run();
        }
    }
}
