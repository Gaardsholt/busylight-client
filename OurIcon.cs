using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busylight_client
{
    class OurIcon : IDisposable
    {
        NotifyIcon ni;

        public OurIcon()
        {
            ni = new NotifyIcon();
        }

        public async Task DisplayAsync()
        {
            ni.Icon = Properties.Resources.icon;
            ni.Text = "Best.gaardsholt.com";
            ni.Visible = true;

            ni.Click += (s, e) =>
            {
                if (((MouseEventArgs)e).Button == MouseButtons.Left)
                    typeof(NotifyIcon).GetMethod("ShowContextMenu", System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.NonPublic).Invoke(ni, null);
            };



            var myContext = new ContextMenus();
            ni.ContextMenuStrip = await myContext.CreateAsync();
        }

        public void Dispose()
        {
            ni.Dispose();

        }
    }

}
