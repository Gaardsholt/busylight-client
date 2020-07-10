using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Globalization;
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
                .WithUrl(new Uri(_settings.SignalR_Uri), options =>
                {
                    options.Headers.Add(_settings.KeyName, _settings.ApiKey);
                })
                .Build();
        }

        public async Task Connect()
        {

            connection.Closed += async error =>
            {
                await Task.Delay(new Random().Next(0, 5) * 1000);
                await ConnectWithRetryAsync();
            };
            connection.Reconnected += async error =>
            {
                await JoinGroup(_settings.Location);
            };

            connection.On("Police", async () =>
            {
                for (int i = 0; i < 400; i++)
                {
                    busy.Light(i % 2 == 0 ? Busylight.BusylightColor.Red : Busylight.BusylightColor.Blue);
                    await Task.Delay(75);
                }
            });
            connection.On("SetColor", (string colorAsString) =>
            {
                colorAsString = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(colorAsString?.ToLower());

                var color = GetColorFromString(colorAsString);
                busy.Light(color);
            });
            connection.On("Ring", async () =>
            {
                var tune = GetTuneFromString(_settings.Ring_Tune);
                var color = GetColorFromString(_settings.Ring_Color);

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
            catch (Exception e)
            {

                MessageBox.Show(e.Message, "Connection error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                Application.Exit();
            }


            async Task<bool> ConnectWithRetryAsync()
            {
                while (true)
                {
                    try
                    {
                        await connection.StartAsync();
                        return true;
                    }
  
                    catch
                    {
                        await Task.Delay(5000);
                    }
                }
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
