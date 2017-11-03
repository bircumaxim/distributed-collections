using System.Net;

namespace Mediator.Events
{
    public delegate void ServerDiscoveredEventHandler(object sender, ServerDiscoveredEventArgs args);
    
    public class ServerDiscoveredEventArgs
    {
        public IPEndPoint ServerIp { get; set; }
        public long DataQuantity { get; set; }

        public ServerDiscoveredEventArgs(IPEndPoint serverIp, long dataQuantity)
        {
            ServerIp = serverIp;
            DataQuantity = dataQuantity;
        }
    }
}