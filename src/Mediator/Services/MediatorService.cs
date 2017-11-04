using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Common;
using Common.Messages.DataRequest;
using Common.Messages.DataResponse;
using Common.Messages.DataResponse.Binary;
using Common.Messages.DataResponse.Json;
using Common.Messages.DataResponse.xml;
using Common.Models;
using Common.Models.Mappers;
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
        private List<Employee> _dataToSendToClient;

        public MediatorService(Config config)
        {
            _dataToSendToClient = new List<Employee>();
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
                _dataToSendToClient.Clear();
                _dataRequestMessagge = (DataRequestMessage) args.Message;
                _discoveryService.DiscoverServerNodes();
                SelectBestServerToConectAfter(_dataRequestMessagge.RequestTimeout).Start();
            }

            if (args.Message.GetType().BaseType?.Name == typeof(DataResponseMessage).Name)
            {
                var message = (DataResponseMessage) args.Message;
                AcumulateData(message);
                if (message.IsLastKnownServerNode)
                {
                    SendDataToClient();
                }
            }
        }

        private void AcumulateData(DataResponseMessage dataResponseMessage)
        {
            List<Employee> data = new List<Employee>();
            if (dataResponseMessage.MessageTypeName == typeof(BinaryDataResponseMessage).Name)
            {
                var message = (BinaryDataResponseMessage) dataResponseMessage;
                data = message.EmployeeMessages.ToList().ConvertAll(BinaryEmployeeMessageMapper.InversMap);
            }
            if (dataResponseMessage.MessageTypeName == typeof(XmlDataResponseMessage).Name)
            {
                var message = (XmlDataResponseMessage) dataResponseMessage;
                data = XmlHelper.Deserealize(message.EmployeeMessages).Employees.ToList()
                    .ConvertAll(XmlEmployeeMessageMapper.InversMap);
            }
            if (dataResponseMessage.MessageTypeName == typeof(JsonDataResponseMessage).Name)
            {
                var message = (XmlDataResponseMessage) dataResponseMessage;
                data = JsonHelper.Deserealize(message.EmployeeMessages);
            }
            _dataToSendToClient = _dataToSendToClient.Concat(data).ToList();
        }

        private void SendDataToClient()
        {
            var dataType = (DataType) Enum.Parse(typeof(DataType), _dataRequestMessagge.DataType);
            var dataResponseMessage = DataResponseMessageFactory.GetDataResponseMessage(dataType, _dataToSendToClient);
            _clientConnecionService.SendDataToClient(dataResponseMessage);
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