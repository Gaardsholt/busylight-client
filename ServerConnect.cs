using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace busylight_client
{
    class ServerConnect
    {
        private static Busylight.SDK busy = new Busylight.SDK();
        HubConnection connection;

        public ToolStripItemCollection MenuItems;
        private static Settings _settings;

        public ServerConnect(Settings settings, ToolStripItemCollection menuItems)
        {
            _settings = settings;
            MenuItems = menuItems;

            connection = new HubConnectionBuilder()
                .WithUrl(new Uri(_settings.SignalR_Uri))
                .WithAutomaticReconnect()
                .Build();
        }



        public async Task Connect()
        {

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
                busy.Light(color);
            });
            connection.On("Ring", async (string colorAsString, string tuneAsString) =>
            {
                var tune = GetTuneFromString(tuneAsString);
                var color = GetColorFromString(colorAsString);

                busy.Alert(color, tune, Busylight.BusylightVolume.High);
                await Task.Delay(_settings.Ring_Time);
                busy.Light(Busylight.BusylightColor.Off);
            });

            try
            {
                await connection.StartAsync();
                await JoinGroup(_settings.Location);
                AddClickEvent();
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
        private async Task SendToSelf(string color)
        {
            await connection.InvokeAsync("SendToSelf", color);
        }
        private static Busylight.BusylightColor GetColorFromString(string colorAsString)
        {
            var propInfo = typeof(Busylight.BusylightColor).GetProperty(colorAsString);
            if (propInfo != null)
            {
                var color = (Busylight.BusylightColor)propInfo.GetValue(propInfo, null);
                return color;
            }
            return Busylight.BusylightColor.Red;
        }
        private static Busylight.BusylightSoundClip GetTuneFromString(string tuneAsString)
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
        private void AddClickEvent()
        {
            foreach (var item in MenuItems)
            {
                if (item is ToolStripMenuItem)
                {
                    var aa = item as ToolStripMenuItem;
                    if ((string)aa.Tag == "Color")
                    {
                        aa.Enabled = true;
                        aa.Click += async (s, e) =>
                        {
                            await SendToSelf(aa.Name);
                        };
                    }
                }
            }
        }
    }
}
