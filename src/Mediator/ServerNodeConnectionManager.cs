using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Net;
using System.Net.Sockets;
using Common;
using Common.Messages.DataRequest;
using Common.Messages.DataResponse;
using Common.Messages.DataResponse.Binary;
using Serialization.WireProtocol;
using Transport.Connectors.Tcp;
using Transport.Events;

namespace Mediator
{
    public class ServerNodeConnectionManager
    {
        private readonly DataRequestMessage _dataRequestMessage;
        public event MessageReceivedHandler MessageReceived;

        public ServerNodeConnectionManager(IPEndPoint ipEndPoint, DataRequestMessage dataRequestMessage)
        {
            _dataRequestMessage = dataRequestMessage;
            var tcpConnector = new TcpConnector(GetTcpSocket(ipEndPoint), new DefaultWireProtocol());
            tcpConnector.StateChanged += OnStateChanged;
            tcpConnector.MessageReceived += OnMessageReceived;
            tcpConnector.StartAsync();
        }

        private void OnStateChanged(object sender, ConnectorStateChangeEventArgs args)
        {
            if (args.NewState == ConnectionState.Connected)
            {
                args.Connector.SendMessage(_dataRequestMessage);
            }
        }

        private void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(BinaryDataResponseMessage).Name)
            {
                MessageReceived?.Invoke(this, args);
            }
        }

        private Socket GetTcpSocket(IPEndPoint ipEndPoint)
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
            return socket;
        }
    }
}