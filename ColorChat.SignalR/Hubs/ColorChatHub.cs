using ColorChat.Domain.Models;
using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;

namespace ColorChat.SignalR.Hubs
{
    public class ColorChatHub : Hub
    {
        public async Task SendColorMessage(ColorChatColor color)
        {
            await Clients.All.SendAsync("ReceiveColorMessage", color);
        }
    }
}
