using System;
using System.Drawing;
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

            Application.Run(new MyApplicationContext());
        }
    }
}
