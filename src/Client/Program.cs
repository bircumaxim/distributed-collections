using System;
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

        public static void Main(string[] args)
        {
            Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000));
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            _tcpConnector = new TcpConnector(socket, new DefaultWireProtocol());
            _tcpConnector.StateChanged += OnConnectorStateChanged;
            _tcpConnector.MessageReceived += OnMessageReceived;
            _tcpConnector.StartAsync();
            Console.ReadLine();
        }

        private static void OnMessageReceived(object sender, MessageReceivedEventArgs e)
        {
            Console.WriteLine(e.Message.MessageTypeName);
        }

        private static void OnConnectorStateChanged(object sender, ConnectorStateChangeEventArgs args)
        {
            if (args.NewState == ConnectionState.Connected)
            {
                var requestMessage = new DataRequestMessageBuilder()
                    .Where(empl => empl.Age > 10)
                    .WithTimeout(1000)
                    .Build();
                _tcpConnector.SendMessage(requestMessage);
            }
        }
    }
}