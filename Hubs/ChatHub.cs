using Microsoft.AspNetCore.Authentication.OAuth.Claims;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace busylight_server.Hubs
{
    public class DynamicChat : Hub
    {
        private static ConcurrentDictionary<string, string> _connections = new ConcurrentDictionary<string, string>();

        public override async Task OnConnectedAsync()
        {
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            //if(_connections.TryGetValue(Context.ConnectionId, out string groupName))
            await LeaveGroup();

            await base.OnDisconnectedAsync(exception);
        }


        public async Task JoinGroup(string groupName)
        {
            await LeaveGroup();

            _connections[Context.ConnectionId] = groupName;

            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
        }

        public async Task LeaveGroup()
        {
            if(_connections.TryRemove(Context.ConnectionId, out string oldGroupName))
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, oldGroupName);

        }

        public async Task SendToSelf(string color)
        {
            await Clients.Caller.SendAsync("SetColor", color);
        }

    }

}
