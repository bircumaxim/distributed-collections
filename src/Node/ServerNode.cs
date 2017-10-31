using System.Net;
using System.Threading.Tasks;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Node.Messages;
using Serialization.WireProtocol;
using Transport;

namespace Node
{
    public class ServerNode : IRun
    {
        private readonly Buss _multicastBuss;
        private Buss _udpBuss;
        private readonly string _nodeName;
        private readonly IPEndPoint _tcpIpEndPoint;
        private readonly IPEndPoint _udpIpEndPoint;
        private readonly TcpConnectionManager _tcpConnectionManager;

        public ServerNode(NodeConfig nodeConfig)
        {
            _nodeName = nodeConfig.Name;
            _udpIpEndPoint = nodeConfig.UdpIpEndPoint;
            _tcpIpEndPoint = nodeConfig.TcpIpEndPoint;
            _multicastBuss = BussFactory.Instance.GetBussFor(_nodeName + "multicast", nodeConfig.MulticastIpEndPoint,
                nodeConfig.MulticastIpEndPoint, "UdpMulticast");
            _multicastBuss.MessageReceived += OnMessageReceived;
            _tcpConnectionManager = new TcpConnectionManager(_tcpIpEndPoint.Port, new DefaultWireProtocol());
        }

        #region IRun methods

        public Task StartAsync()
        {
            return _tcpConnectionManager.StartAsync();
        }

        public void Stop()
        {
            _tcpConnectionManager.Stop();
            _multicastBuss.Dispose();
        }

        #endregion

        #region Listeners

        private void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryRequest).Name)
            {
                SendDiscoveryResponse(args);
            }
        }

        #endregion

        private void SendDiscoveryResponse(MessegeReceviedEventArgs args)
        {
            var discoveryRequest = (DiscoveryRequest) args.Payload;
            _udpBuss?.Dispose();
            _udpBuss = BussFactory.Instance.GetBussFor(_nodeName + "udp", discoveryRequest.BrockerIpEndPoint, _udpIpEndPoint);
            _udpBuss.Publish(
                discoveryRequest.ExchangeName,
                discoveryRequest.QueueName,
                new DiscoveryResponse {NodIpEndPoint = _tcpIpEndPoint}
            );
        }
    }
}