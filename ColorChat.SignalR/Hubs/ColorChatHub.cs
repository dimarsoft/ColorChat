using System;
using ColorChat.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using Serilog;

namespace ColorChat.SignalR.Hubs
{
    public class ColorChatHub : Hub
    {
        public override Task OnConnectedAsync()
        {
            Log.Logger.Debug("Call: {Name}", nameof(OnConnectedAsync));
            return base.OnConnectedAsync();
        }

        public override Task OnDisconnectedAsync(Exception exception)
        {
            Log.Logger.Debug(exception,"Call: {Name}", nameof(OnDisconnectedAsync));
            return base.OnDisconnectedAsync(exception);
        }

        public async Task SendColorMessage(ColorChatColor color)
        {
            Log.Logger.Debug("SendColorMessage: {Color}", color);
            
            await Clients.All.SendAsync("ReceiveColorMessage", color);
        }
    }
}
