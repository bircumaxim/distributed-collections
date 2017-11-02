using System.Net;
using Common;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;

namespace Node
{
    public class DiscoveryService
    {
        private Buss _udpBuss;
        private Buss _multicastBuss;
        private readonly IPEndPoint _udpIpEndPoint;
        private readonly IPEndPoint _tcIpEndPoint;
        private readonly IPEndPoint _multicastIpEndPoint;

        public DiscoveryService(IPEndPoint multicastIpEndPoint, IPEndPoint tcIpEndPoint, IPEndPoint udpIpEndPoint)
        {
            _udpIpEndPoint = udpIpEndPoint;
            _tcIpEndPoint = tcIpEndPoint;
            _multicastIpEndPoint = multicastIpEndPoint;
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
            _multicastBuss.Dispose();
            _udpBuss?.Dispose();
        }

        #endregion

        private void OnMessageReceivedFromMulticastGroup(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryRequestMessage).Name)
            {
                SendDiscoveryResponse(args);
            }
        }

        private void SendDiscoveryResponse(MessegeReceviedEventArgs args)
        {
            var discoveryRequest = (DiscoveryRequestMessage) args.Payload;
            _udpBuss = BussFactory.Instance.GetBussFor("udp", discoveryRequest.BrockerIpEndPoint,
                _udpIpEndPoint);
            _udpBuss.Publish(
                discoveryRequest.ExchangeName,
                discoveryRequest.QueueName,
                new DiscoveryResponseMessage {NodIpEndPoint = _tcIpEndPoint}
            );
        }
    }
}