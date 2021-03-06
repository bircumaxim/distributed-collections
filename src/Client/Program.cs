﻿using System;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using Common;
using Serialization.WireProtocol;
using Transport.Connectors.Tcp;
using Transport.Events;

namespace Client
{
    internal class Program
    {
        private static TcpConnector _tcpConnector;
        private const string MediatorIp = "127.0.0.1";
        private const int MediatorPort = 4000;

        public static void Main(string[] args)
        {
            _tcpConnector = new TcpConnector(GetSocket(), new DefaultWireProtocol());
            _tcpConnector.StateChanged += OnConnectorStateChanged;
            _tcpConnector.MessageReceived += OnMessageReceived;
            _tcpConnector.StartAsync();
            Console.ReadLine();
        }

        private static void OnConnectorStateChanged(object sender, ConnectorStateChangeEventArgs args)
        {
            if (args.NewState == ConnectionState.Connected)
            {
                var requestMessage = new DataRequestMessageBuilder()
                    .OrderBy(empl => empl.Age)
                    .WithTimeout(100)
                    .Build();
                _tcpConnector.SendMessage(requestMessage);
            }
        }
        
        private static void OnMessageReceived(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(DataResponseMessage).Name)
            {
                var message = (DataResponseMessage) args.Message;
                Console.WriteLine(message.Employees.Length);
                message.Employees.ToList().ForEach(Console.WriteLine);
            }
        }

        private static Socket GetSocket()
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse(MediatorIp), MediatorPort));
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