using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using busylight_server.Hubs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace busylight_server.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class BusyController : ControllerBase
    {
        private IHubContext<DynamicChat> HubContext { get; set; }
        public BusyController(IHubContext<DynamicChat> hubcontext) => HubContext = hubcontext;


        [HttpPost("{color}")]
        public async Task Color(string color, string group)
        {
            await HubContext.Clients.Group(group).SendAsync("SetColor", color);
        }

        [HttpPost("{group}")]
        public async Task Ring(string group, string color, string tune)
        {
            await HubContext.Clients.Group(group).SendAsync("Ring", color, tune);
        }

    }
}