using System;
using System.Collections.Generic;
using Common;
using Mediator.Events;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Messages.Subscribe;

namespace Mediator.Services
{
    public class DiscoveryService
    {
        private readonly Queue<DiscoveryRequestMessage> _discoveryMessagesQueue;
        private readonly Buss _discoveryBuss;
        private bool _isDiscoveryResponseBussSubsciribed;
        private readonly Config _config;
        public event ServerDiscoveredEventHandler ServerDiscovered;

        public DiscoveryService(Config config)
        {
            _config = config;
            _discoveryMessagesQueue = new Queue<DiscoveryRequestMessage>();
            _discoveryBuss = BussFactory.Instance.GetBussFor(_config.DiscoveryExchangeName);
            var discoveryResponseBuss = BussFactory.Instance.GetBussFor(_config.DiscoveryExchangeName);
            discoveryResponseBuss.MessageReceived += OnMessageReceived;
            discoveryResponseBuss.Subscribe(_config.DiscoveryResponsesQueueName);
        }

        public void DiscoverServerNodes()
        {
            var discoveryRequestMessage = GetDiscoveryRequestMessage();
            if (_isDiscoveryResponseBussSubsciribed)
            {
                SendDicoveryRequest(discoveryRequestMessage);
                return;
            }
            _discoveryMessagesQueue.Enqueue(discoveryRequestMessage);
        }

        private void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(SubscribeSuccessMessage).Name)
            {
                _isDiscoveryResponseBussSubsciribed = true;
                while (_discoveryMessagesQueue.Count > 0)
                {
                    var message = _discoveryMessagesQueue.Dequeue();
                    SendDicoveryRequest(message);
                }
            }

            if (args.Payload.MessageTypeName == typeof(DiscoveryResponseMessage).Name)
            {
                var payload = (DiscoveryResponseMessage) args.Payload;
                Console.WriteLine($"{payload.NodIpEndPoint} : {payload.DataQuantity}");
                ServerDiscovered?.Invoke(this,
                    new ServerDiscoveredEventArgs(payload.NodIpEndPoint, payload.DataQuantity));
            }
        }

        private DiscoveryRequestMessage GetDiscoveryRequestMessage()
        {
            return new DiscoveryRequestMessage
            {
                ExchangeName = _config.DiscoveryExchangeName,
                QueueName = _config.DiscoveryResponsesQueueName,
                BrockerIpEndPoint = _config.BrokerIpEndPoint
            };
        }

        private void SendDicoveryRequest(DiscoveryRequestMessage discoveryRequestMessage)
        {
            _discoveryBuss.Direct(discoveryRequestMessage, _config.DiscoveryRequestsQueueName);
        }
    }
}