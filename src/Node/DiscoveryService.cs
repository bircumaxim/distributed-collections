using System;
using System.Net;
using Common;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Node.Events;

namespace Node
{
    public class DiscoveryService
    {
        private Buss _udpBuss;
        private Buss _multicastBuss;
        private readonly IPEndPoint _udpIpEndPoint;
        private readonly IPEndPoint _tcIpEndPoint;
        private readonly IPEndPoint _multicastIpEndPoint;
        private readonly ServerNode _serverNode;
        private DiscoveryRequestMessage _discoveryRequestMessage;

        public DiscoveryService(IPEndPoint multicastIpEndPoint, IPEndPoint tcIpEndPoint, IPEndPoint udpIpEndPoint, ServerNode serverNode)
        {
            _serverNode = serverNode;
            _udpIpEndPoint = udpIpEndPoint;
            _tcIpEndPoint = tcIpEndPoint;
            _multicastIpEndPoint = multicastIpEndPoint;
            _serverNode.DataQunatityComputed += OnDataQuantityComputed;
        }

        #region IRun methods

        public void StartAsync()
        {
            _multicastBuss = BussFactory.Instance.GetBussFor("multicast", _multicastIpEndPoint,
                _multicastIpEndPoint, BrokerProtocolType.UdpMulticast);
            _multicastBuss.MessageReceived += OnMessageReceivedFromMulticastGroup;
        }

        public void Stop()
        {
            _serverNode.DataQunatityComputed -= OnDataQuantityComputed;
            _multicastBuss.MessageReceived -= OnMessageReceivedFromMulticastGroup;
            _multicastBuss.Dispose();
            _udpBuss?.Dispose();
        }

        #endregion

        private void OnMessageReceivedFromMulticastGroup(object sender, MessegeReceviedEventArgs args)
        {   
            if (args.Payload.MessageTypeName == typeof(DiscoveryRequestMessage).Name)
            {
                _discoveryRequestMessage = (DiscoveryRequestMessage) args.Payload;
                _serverNode.GetDataQuantity();
            }
            if (args.Payload.MessageTypeName == typeof(ConnectTheGraphMessage).Name)
            {
                _serverNode.ConnectoToKnowServers();
                Console.WriteLine("Server graph connected !");
            }
        }

        
        private void OnDataQuantityComputed(object sender, DataQuantityEventArgs args)
        {
            SendDiscoveryResponse(args.Qunatity);
        }
        
        private void SendDiscoveryResponse(long dataQuantity)
        {
            _udpBuss = BussFactory.Instance.GetBussFor("udp", _discoveryRequestMessage.BrockerIpEndPoint,
                _udpIpEndPoint);
            _udpBuss.Publish(
                _discoveryRequestMessage.ExchangeName,
                _discoveryRequestMessage.QueueName,
                new DiscoveryResponseMessage
                {
                    NodIpEndPoint = _tcIpEndPoint,
                    DataQuantity = dataQuantity
                }
            );
        }
    }
}