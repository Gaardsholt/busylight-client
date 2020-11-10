using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace busylight_client
{
    class MenuGenerator
    {
        public ContextMenuStrip Menu;
        private static Settings _settings;

        public MenuGenerator(Settings settings)
        {
            _settings = settings;
            Menu = GenerateMenu();
        }

        private static ContextMenuStrip GenerateMenu()
        {
            var menu = new ContextMenuStrip();
            menu.SuspendLayout();

            menu.Items.AddRange(new ToolStripItem[]
            {
                new ToolStripMenuItem{ Text = _settings.Location, Enabled = false },
                new ToolStripSeparator()
            });
            menu.Items.AddRange(GetColorMenu());
            menu.Items.Add(new ToolStripSeparator());

            menu.Items.Add("Exit", _settings.resourceSet.GetObject("Exit") as Image, (a, b) => Application.Exit());

            menu.ResumeLayout(false);

            return menu;
        }

        private static ToolStripMenuItem[] GetColorMenu()
        {
            var colors = new List<ToolStripMenuItem>();
            foreach (DictionaryEntry entry in _settings.resourceSet)
            {
                var name = entry.Key.ToString();
                if (name == "Exit")
                    continue;

                if (entry.Value is Bitmap)
                {
                    var someMenuItem = new ToolStripMenuItem
                    {
                        Name = name,
                        Text = name,
                        Image = (Bitmap)entry.Value,
                        Enabled = false,
                        Tag = "Color"
                    };
                    someMenuItem.Click += (s, e) =>
                    {
                        //connection.InvokeAsync("SendToSelf", name);
                    };
                    colors.Add(someMenuItem);
                }
            }
            return colors.OrderBy(a => a.Text).ToArray();
        }
    }
}
