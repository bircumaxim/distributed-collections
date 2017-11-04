using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common;
using Common.Messages.DataQuantity;
using Common.Messages.DataRequest;
using Common.Messages.DataResponse;
using Common.Messages.DataResponse.Binary;
using Common.Models;
using Node.Data;
using Node.Events;
using Serialization.WireProtocol;
using Transport;
using Transport.Connectors;
using Transport.Connectors.Tcp;
using Transport.Events;

namespace Node
{
    public class ServerNode : IRun
    {
        private readonly TcpConnectionManager _tcpConnectionManager;
        private readonly List<IConnector> _tcpConnectors;
        private readonly List<IConnector> _knowedServerNodesConnectors;
        private readonly DataManager _dataManager;
        private IConnector _mediatorConnector;
        private long _totalDataQunatity;
        private readonly List<IPEndPoint> _knownEndPoints;
        public event DataQuantityEventArgsEvemtHandler DataQunatityComputed;
        private readonly DataType _dataType;

        public ServerNode(int tcpPort, List<IPEndPoint> knownEndPoints,DataType dataType, DataManager dataManager)
        {
            _dataType = dataType;
            _knownEndPoints = knownEndPoints;
            _dataManager = dataManager;
            _tcpConnectionManager = new TcpConnectionManager(tcpPort, new DefaultWireProtocol());
            _tcpConnectionManager.ConnectorConnected += OnTcpClientConnected;
            _tcpConnectors = new List<IConnector>();
            _knowedServerNodesConnectors = new List<IConnector>();
            _totalDataQunatity = BytesHelper.GetDataSizeInBytes(_dataManager.GetEmployees().ToList());
        }

        #region IRun methods

        public Task StartAsync()
        {
            return _tcpConnectionManager.StartAsync();
        }

        public void Stop()
        {
            _tcpConnectionManager.Stop();
        }

        #endregion

        #region Listeners

        private void OnTcpClientConnected(object sender, ConnectorConnectedEventArgs args)
        {
            var connector = args.Connector;
            connector.MessageReceived += OnMessageReceivedFromTcpClient;
            lock (_tcpConnectors)
            {
                _tcpConnectors.Add(connector);
            }
            connector.StartAsync();
        }

        private void OnMessageReceivedFromTcpClient(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(DataRequestMessage).Name)
            {
                HandleDataRequest(args);
            }
            if (args.Message.MessageTypeName == typeof(BinaryDataResponseMessage).Name)
            {
                HandleDataResponse(args);
            }
            if (args.Message.MessageTypeName == typeof(DataQuantityRequestMessage).Name)
            {
                HandleDataQunatityRequest(args);
            }
            if (args.Message.MessageTypeName == typeof(DataQuantityResponseMessage).Name)
            {
                HandleDataQunatityResponse(args);
            }
        }

        #endregion

        private void AddKnownServerConnector(IPEndPoint ipEndPoint)
        {
            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(ipEndPoint);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            var tcpConnector = new TcpConnector(socket, new DefaultWireProtocol());
            tcpConnector.MessageReceived += OnMessageReceivedFromTcpClient;
            tcpConnector.StartAsync();
            _knowedServerNodesConnectors.Add(tcpConnector);
        }

        private void HandleDataRequest(MessageReceivedEventArgs args)
        {
            var message = (DataRequestMessage) args.Message;
            var dataResponseMessage = DataResponseMessageFactory.GetDataResponseMessage(_dataType, _dataManager.GetEmployees(message.Filters));
            if (message.IsFromAServerNode || _knowedServerNodesConnectors.Count == 0)
            {
                dataResponseMessage.IsLastKnownServerNode = message.IsLastKnownServerNode;
                args.Connector.SendMessage(dataResponseMessage);
            }
            else
            {
                _mediatorConnector = args.Connector;
                SendGetDataMessageToKnownServerNodes(message);
            }
        }

        private void HandleDataResponse(MessageReceivedEventArgs args)
        {
            _mediatorConnector.SendMessage(args.Message);
        }

        private void SendGetDataMessageToKnownServerNodes(DataRequestMessage requestMessage)
        {
            requestMessage.IsFromAServerNode = true;
            for (var i = 0; i < _knowedServerNodesConnectors.Count; i++)
            {
                requestMessage.IsLastKnownServerNode = i == _knowedServerNodesConnectors.Count - 1;
                _knowedServerNodesConnectors[i].SendMessage(requestMessage);
            }
        }

        private void HandleDataQunatityRequest(MessageReceivedEventArgs args)
        {
            var message = (DataQuantityRequestMessage) args.Message;
            var dataQuantityResponseMessage = new DataQuantityResponseMessage
            {
                Quantity = BytesHelper.GetDataSizeInBytes(_dataManager.GetEmployees().ToList()),
                IsLastServerNode = message.IsLastServerNode
            };
            args.Connector.SendMessage(dataQuantityResponseMessage);
        }

        private void HandleDataQunatityResponse(MessageReceivedEventArgs args)
        {
            var message = (DataQuantityResponseMessage) args.Message;
            _totalDataQunatity += message.Quantity;
            if (message.IsLastServerNode)
            {
                DataQunatityComputed?.Invoke(this, new DataQuantityEventArgs(_totalDataQunatity));
                _totalDataQunatity = 0;
            }
        }

        public void GetDataQuantity()
        {
            var dataQuantityRequestMessage = new DataQuantityRequestMessage();
            _totalDataQunatity = BytesHelper.GetDataSizeInBytes(_dataManager.GetEmployees().ToList());
            if (_knowedServerNodesConnectors.Count == 0)
            {
                DataQunatityComputed?.Invoke(this, new DataQuantityEventArgs(_totalDataQunatity));
            }
            for (int i = 0; i < _knowedServerNodesConnectors.Count; i++)
            {
                dataQuantityRequestMessage.IsLastServerNode = i == _knowedServerNodesConnectors.Count - 1;
                _knowedServerNodesConnectors[i].SendMessage(dataQuantityRequestMessage);
            }
        }

        public void ConnectoToKnowServers()
        {
            _knownEndPoints.ForEach(AddKnownServerConnector);
        }
    }
}