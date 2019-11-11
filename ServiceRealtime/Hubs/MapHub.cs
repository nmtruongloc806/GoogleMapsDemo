using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SignalRService.Hubs
{
    public class MapHub : Hub
    {
        public async Task SendLocation(string iconColor, float lat, float lng)
        {
            await Clients.All.SendAsync("ReceiveLocation", iconColor, lat, lng);
        }
    }
}
