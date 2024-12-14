using Microsoft.AspNetCore.SignalR;
using SignalR.Web.Models;

namespace SignalR.Web.Hubs
{
    public class ExampleTypeSafeHub : Hub<IExampleTypeSafeHub>
    {
        private static int ConnectedClientCount = 0;

        public async Task BroadcastMessageToAllClient(string message)
        {
            await Clients.All.ReceiveMessageForAllClient(message);
            //await Clients.All.SendAsync("ReceiveMessageForAllClient", message);
        }

        public async Task BroadcastMessageToCallerClient(string message)
        {
            await Clients.Caller.ReceiveMessageForCallerClient(message);
        }

        public async Task BroadcastMessageToOtherClient(string message)
        {
            await Clients.Others.ReceiveMessageForOtherClient(message);
        }

        public async Task BroadcastMessageToIndividualClient(string connectionId, string message)
        {
            await Clients.Client(connectionId).ReceiveMessageForIndividualClient(message);
        }

        public async Task BroadcastMessageToGroupClient(string groupName, string message)
        {
            await Clients.Group(groupName).ReceiveMessageForGroupClient(message);
        }

        public async Task AddGroup(string groupName)
        {
            var connectionId = Context.ConnectionId;
            await Groups.AddToGroupAsync(connectionId, groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} Grubuna Dahil oldun");
            await Clients.Group(groupName).ReceiveMessageForGroupClient($"{connectionId} kişisi, {groupName} grubuna dahil oldu.");
        }

        public async Task RemoveGroup(string groupName)
        {
            var connectionId = Context.ConnectionId;
            await Groups.RemoveFromGroupAsync(connectionId, groupName);
            await Clients.Caller.ReceiveMessageForCallerClient($"{groupName} Grubundan ayrıldın");
            await Clients.Group(groupName).ReceiveMessageForGroupClient($"{connectionId} kişisi, {groupName} grubundan ayrıldı.");

        }

        public async Task BroadcastTypedMessageToAllClient(Product product)
        {
            await Clients.All.ReceiveTypedMessageForAllClient(product);
        }

        public async Task BroadcastStreamDataToAllClient(IAsyncEnumerable<string> nameAsChunks)
        {
            await foreach (var name in nameAsChunks) await Clients.All.ReceiveMessageAsStreamForAllClient(name);
        }

        public async Task BroadcastStreamProductToAllClient(IAsyncEnumerable<Product> productAsChunks)
        {
            await foreach (var product in productAsChunks) await Clients.All.ReceiveProductAsStreamForAllClient(product);
        }

        public async IAsyncEnumerable<string> BroadcastFromHubToClient(int count)
        {
            foreach (var item in Enumerable.Range(1, count).ToList())
            {
                await Task.Delay(1000);
                yield return $"{item}. data";
            }
        }

        public override async Task OnConnectedAsync()
        {
            ConnectedClientCount++;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            ConnectedClientCount--;
            await Clients.All.ReceiveConnectedClientCountAllClient(ConnectedClientCount);
            await base.OnDisconnectedAsync(exception);
        }

    }
}
