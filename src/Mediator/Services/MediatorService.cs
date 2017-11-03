using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Common.Messages.DataRequest;
using Common.Messages.DataResponse;
using Common.Messages.DataResponse.Binary;
using Mediator.Events;
using Transport.Events;

namespace Mediator.Services
{
    public class MediatorService
    {
        private readonly DiscoveryService _discoveryService;
        private readonly ClientConnecionService _clientConnecionService;
        private readonly Dictionary<IPEndPoint, long> _discoveredServers = new Dictionary<IPEndPoint, long>();
        private DataRequestMessage _dataRequestMessagge;

        public MediatorService(Config config)
        {
            _discoveryService = new DiscoveryService(config);
            _discoveryService.ServerDiscovered += OnServerDiscoverd;
            _clientConnecionService = new ClientConnecionService(1000);
            _clientConnecionService.MessageReceived += OnMessageReceived;
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {   
            if (args.Message.MessageTypeName == typeof(DataRequestMessage).Name)
            {
                _discoveredServers.Clear();
                _dataRequestMessagge = (DataRequestMessage) args.Message;
                _discoveryService.DiscoverServerNodes();
                SelectBestServerToConectAfter(_dataRequestMessagge.RequestTimeout).Start();
            }

            if (args.Message.MessageTypeName == typeof(BinaryDataResponseMessage).Name)
            {
                _clientConnecionService.SendDataToClient(args.Message);
            }
        }

        async Task SelectBestServerToConectAfter(int timeToWait)
        {
            await Task.Delay(timeToWait);
            var maxQuantity = _discoveredServers.Max(entry => entry.Value);
            var ipEndPoint = _discoveredServers.FirstOrDefault(entry => entry.Value == maxQuantity).Key;
            new ServerNodeConnectionManager(ipEndPoint, _dataRequestMessagge).MessageReceived += OnMessageReceived;
        }

        private void OnServerDiscoverd(object sender, ServerDiscoveredEventArgs args)
        {
            _discoveredServers.Add(args.ServerIp, args.DataQuantity);
        }
    }
}