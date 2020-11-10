using System;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busylight_client
{
    class MyApplicationContext : ApplicationContext
    {
        private NotifyIcon TrayIcon;
        public Settings Settings;
        public ContextMenuStrip Menu;


        public MyApplicationContext()
        {
            Settings = new Settings();
            Menu = new MenuGenerator(Settings).Menu;

            Application.ApplicationExit += (object sender, EventArgs e) => { TrayIcon.Visible = false; };
            InitializeComponent();
            TrayIcon.Visible = true;
        }

        private void InitializeComponent()
        {

            TrayIcon = new NotifyIcon
            {
                Icon = Settings.resourceSet.GetObject("icon") as Icon,
                ContextMenuStrip = Menu,
                Text = "Busylight client",
                Visible = true
            };


            var serverConnect = new ServerConnect(Settings, Menu);
            Task.Run(() => serverConnect.Connect());

        }

    }
}
