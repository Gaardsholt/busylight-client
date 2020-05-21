using System;
using System.Windows.Forms;

namespace busylight_client
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            using (OurIcon pi = new OurIcon())
            {
                pi.DisplayAsync();

                Application.Run();
            }
        }
    }
}
