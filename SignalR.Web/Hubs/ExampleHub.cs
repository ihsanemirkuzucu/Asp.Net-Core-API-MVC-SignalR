using Microsoft.AspNetCore.SignalR;

namespace SignalR.Web.Hubs
{
    public class ExampleHub:Hub
    {
        public async Task BroadcastMessageAllClient(string message)
        {
            await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        }
    }
}
