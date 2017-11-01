using System;
using System.Collections.Generic;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Runtime.Remoting.Lifetime;
using System.Threading.Tasks;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Messages.Connection;
using Node.Data;
using Node.Messages;
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
        private IConnector _mediatorConnector;

        public ServerNode(int tcpPort, List<IPEndPoint> knownEndPoints)
        {
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
            lock (_tcpConnectors)
            {
                var connector = args.Connector;
                connector.MessageReceived += OnMessageReceivedFromTcpClient;
                connector.StartAsync();
                _tcpConnectors.Add(connector);
            }
        }

        private void OnMessageReceivedFromTcpClient(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(GetDataMessage).Name)
            {
                HandleGetDataMessage(args);
            }
            if (args.Message.MessageTypeName == typeof(ServerNodeData).Name)
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
            if (!message.IsFromAServerNode)
            {
                _mediatorConnector = args.Connector;
                SendGetDataMessageToKnownServerNodes(message);
            }
            else
            {
                var serverNodeData = new ServerNodeData
                {
                    IsLastKnownServerNode = message.IsLastKnownServerNode
                };
                args.Connector.SendMessage(serverNodeData);
            }
        }

        private void HandleServerNodeDataMessage(MessageReceivedEventArgs args)
        {
            var message = (ServerNodeData) args.Message;
            //TODO acumulate data to send 
            if (message.IsLastKnownServerNode)
            {
                //TODO send data to mediator
                //_mediatorConnector.SendMessage(data);
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