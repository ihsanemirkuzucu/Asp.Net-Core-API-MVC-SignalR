using SignalR.Web.Models;

namespace SignalR.Web.Hubs
{
    public interface IExampleTypeSafeHub
    {
        Task ReceiveMessageForAllClient(string message);
        Task ReceiveMessageForCallerClient(string message);
        Task ReceiveMessageForOtherClient(string message);
        Task ReceiveMessageForIndividualClient(string message);
        Task ReceiveMessageForGroupClient(string message);
        Task ReceiveConnectedClientCountAllClient(int clientCount);
        Task ReceiveTypedMessageForAllClient(Product product);
        Task ReceiveMessageAsStreamForAllClient(string name);
        Task ReceiveProductAsStreamForAllClient(Product product);
    }
}
