namespace Node
{
    public class NodeService
    {
        private readonly ServerNode _serverNode;
        private readonly DiscoveryService _discoveryService;

        public NodeService(NodeConfig nodeConfig)
        {
            _serverNode = new ServerNode(nodeConfig.TcpIpEndPoint.Port, nodeConfig.KnownEndPoints);
            _discoveryService = new DiscoveryService(nodeConfig.MulticastIpEndPoint, nodeConfig.TcpIpEndPoint, nodeConfig.UdpIpEndPoint);
        }

        public void Stop()
        {
            _discoveryService.Stop();
            _serverNode.Stop();
        }

        public void StartAsync()
        {
            _discoveryService.StartAsync();
            _serverNode.StartAsync();
        }
    }
}