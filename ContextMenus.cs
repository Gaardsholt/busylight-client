using busylight_client.Properties;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busylight_client
{
    class ContextMenus
    {
        private static Busylight.SDK busy = new Busylight.SDK();


        public static Busylight.BusylightColor GetColorFromString(string colorAsString)
        {
            var propInfo = typeof(Busylight.BusylightColor).GetProperty(colorAsString);
            if (propInfo != null)
            {
                var color = (Busylight.BusylightColor)propInfo.GetValue(propInfo, null);
                return color;
            }
            return Busylight.BusylightColor.Red;
        }
        public static Busylight.BusylightSoundClip GetTuneFromString(string tuneAsString)
        {
            try
            {
                return (Busylight.BusylightSoundClip)Enum.Parse(typeof(Busylight.BusylightSoundClip), tuneAsString, true);
            }
            catch (Exception)
            {
                return Busylight.BusylightSoundClip.KuandoTrain;
            }

        }

        //Microsoft.AspNetCore.SignalR.Client.HubConnection connection;
        HubConnection connection;

        private async Task ConnectToServerAsync()
        {
            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(Settings.Default.SignalR_Uri))
                .WithAutomaticReconnect()
                .Build();


            connection.Closed += async (error) =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await connection.StartAsync();
            };
            connection.Reconnecting += async (error) =>
            {
                await Task.Delay(1);
            };
            connection.Reconnected += async (error) =>
            {
                await Task.Delay(1);
            };

            connection.On("SetColor", (string colorAsString) =>
            {
                colorAsString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(colorAsString?.ToLower());

                var color = GetColorFromString(colorAsString);
                if (color != null)
                {
                    busy.Light(color);
                }
            });

            connection.On("Ring", async (string colorAsString, string tuneAsString) =>
            {
                var tune = GetTuneFromString(tuneAsString);
                var color = GetColorFromString(colorAsString);

                busy.Alert(color, tune, Busylight.BusylightVolume.High);
                await Task.Delay(Settings.Default.Ring_Time);
                busy.Light(Busylight.BusylightColor.Off);
            });


            try
            {
                await connection.StartAsync();
                await JoinGroup(Settings.Default.Location);
            }
            catch (Exception)
            {
                //Just let it fail?
            }
        }

        private async Task JoinGroup(string groupName)
        {
            await connection.InvokeAsync("JoinGroup", groupName);
        }


        public async Task<ContextMenuStrip> CreateAsync()
        {
            await ConnectToServerAsync();

            var locations = new string[] { "Stilling", "Brande", "Aarhus" }.OrderBy(a => a).ToArray();


            var menu = new ContextMenuStrip();
            var menuLocations = new ToolStripMenuItem("Location");


            menuLocations.DropDownItems.AddRange(locations.Select(a => new ToolStripMenuItem(a, null, async (obj, e) =>
            {
                var thisObj = (ToolStripMenuItem)obj;
                await JoinGroup(thisObj.Text);
                Settings.Default.Location = thisObj.Text;
                Settings.Default.Save();

                UpdateLocationMenu(menuLocations);
            })
            {
                Checked = a == Settings.Default.Location
            }).ToArray());

            menu.Items.Add(menuLocations);


            var colors = new List<ToolStripMenuItem>();
            var resourceSet = Resources.ResourceManager.GetResourceSet(CultureInfo.CurrentUICulture, true, true);
            foreach (DictionaryEntry entry in resourceSet)
            {
                var name = entry.Key.ToString();
                if (name == "Exit")
                    continue;

                if (entry.Value is Bitmap)
                {
                    var someMenuItem = new ToolStripMenuItem
                    {
                        Text = name,
                        Image = (Bitmap)entry.Value
                    };
                    someMenuItem.Click += (s, e) =>
                    {
                        connection.InvokeAsync("SendToSelf", name);
                    };
                    colors.Add(someMenuItem);
                }
            }

            menu.Items.AddRange(colors.OrderBy(a => a.Text).ToArray());
            menu.Items.Add(new ToolStripSeparator());

            var menuItemExit = new ToolStripMenuItem
            {
                Text = "E&xit",
                Image = Resources.Exit
            };
            menuItemExit.Click += (s, e) => { Application.Exit(); };
            menu.Items.Add(menuItemExit);

            return menu;
        }

        private void UpdateLocationMenu(ToolStripMenuItem menuLocations)
        {
            foreach (var item in menuLocations.DropDownItems)
            {
                if (item is ToolStripMenuItem)
                {
                    var menuItem = item as ToolStripMenuItem;
                    menuItem.Checked = (menuItem.Text == Settings.Default.Location);
                }
            }
        }



    }

}
