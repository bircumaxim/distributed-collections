using System;
using System.Net;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Node.Messages;
using Serialization.WireProtocol;
using Transport;
using Transport.Events;

namespace Node
{
    public class Node
    {
        private const int DefaultMessageLength = 52428800;
        private Buss _buss;
        private readonly string _nodeName;
        private readonly IPEndPoint _tcpIpeEndPoint;
        private readonly TcpConnectionManager _tcpConnectionManager;

        public Node(string nodeName, IPEndPoint multicastIpEndPoint, IPEndPoint tcpIpeEndPoint,
            IWireProtocol wireProtocol)
        {
            _tcpIpeEndPoint = tcpIpeEndPoint;
            _nodeName = nodeName;
            _buss = BussFactory.Instance.GetBussFor(nodeName + "multicast", multicastIpEndPoint, "UdpMulticast");
            _buss.MessageReceived += OnMessageReceived;
            _tcpConnectionManager = new TcpConnectionManager(tcpIpeEndPoint.Port, wireProtocol, DefaultMessageLength);
            _tcpConnectionManager.StartAsync();
            _tcpConnectionManager.ConnectorConnected += OnConnectorConnected;
        }
        
        #region Listeners

        private void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryRequest).Name)
            {
                SendDiscoveryResponse(args);
            } 
        }

        #endregion
        
        private void OnConnectorConnected(object sender, ConnectorConnectedEventArgs args)
        {
            Console.WriteLine("Connector connected");
        }
        
        private void SendDiscoveryResponse(MessegeReceviedEventArgs args)
        {
            var discoveryRequest = (DiscoveryRequest) args.Payload;
            _buss.Dispose();
            _buss = BussFactory.Instance.GetBussFor(_nodeName + "udp", discoveryRequest.BrockerIpEndPoint);
            _buss.Publish(
                discoveryRequest.ExchangeName,
                discoveryRequest.QueueName,
                new DiscoveryResponse {NodIpEndPoint = _tcpIpeEndPoint}
            );

            Console.WriteLine(discoveryRequest.BrockerIpEndPoint);
        }
    }
}