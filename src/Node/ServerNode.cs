using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Common;
using Messages;
using Node.Data;
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
        private ServerNodeDataMessage _messageToBeSent;
        private IConnector _mediatorConnector;

        public ServerNode(int tcpPort, List<IPEndPoint> knownEndPoints, DataManager dataManager)
        {
            _dataManager = dataManager;
            _tcpConnectionManager = new TcpConnectionManager(tcpPort, new DefaultWireProtocol());
            _tcpConnectionManager.ConnectorConnected += OnTcpClientConnected;
            _tcpConnectors = new List<IConnector>();
            _knowedServerNodesConnectors = new List<IConnector>();
            knownEndPoints.ForEach(AddKnownServerConnector);
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
            if (args.Message.MessageTypeName == typeof(GetDataMessage).Name)
            {
                HandleGetDataMessage(args);
            }
            if (args.Message.MessageTypeName == typeof(ServerNodeDataMessage).Name)
            {
                HandleServerNodeDataMessage(args);
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

        private void HandleGetDataMessage(MessageReceivedEventArgs args)
        {
            var message = (GetDataMessage) args.Message;
            
            _messageToBeSent = new ServerNodeDataMessage
            {
                Employees = _dataManager.GetEmployees().ToArray()
            };
            
            if (message.IsFromAServerNode || _knowedServerNodesConnectors.Count == 0)
            {
                _messageToBeSent.IsLastKnownServerNode = message.IsLastKnownServerNode;
                args.Connector.SendMessage(_messageToBeSent);
            }
            else
            {
                _mediatorConnector = args.Connector;
                SendGetDataMessageToKnownServerNodes(message);
            }
        }

        private void HandleServerNodeDataMessage(MessageReceivedEventArgs args)
        {
            var message = (ServerNodeDataMessage) args.Message;
            _messageToBeSent.Employees = _messageToBeSent.Employees.Concat(message.Employees).ToArray();
            if (message.IsLastKnownServerNode)
            {
                _mediatorConnector.SendMessage(_messageToBeSent);
                _messageToBeSent = null;
            }
        }

        private void SendGetDataMessageToKnownServerNodes(GetDataMessage message)
        {
            message.IsFromAServerNode = true;
            for (var i = 0; i < _knowedServerNodesConnectors.Count; i++)
            {
                message.IsLastKnownServerNode = i == _knowedServerNodesConnectors.Count - 1;
                _knowedServerNodesConnectors[i].SendMessage(message);
            }
        }
    }
}