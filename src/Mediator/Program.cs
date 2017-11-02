using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using Common;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Messages;
using Messages.Subscribe;
using Serialization.WireProtocol;
using Transport.Connectors.Tcp;
using Transport.Events;

namespace Mediator
{
    internal class Program
    {
        private static Buss _buss;
        private static TcpConnector _tcpConnector;

        public static void Main(string[] args)
        {
//            _buss = BussFactory.Instance.GetBussFor("Discovery");
//            Buss discoveryBuss = BussFactory.Instance.GetBussFor("DiscoveryResponse");
//            discoveryBuss.MessageReceived += OnMessageReceived;
//            discoveryBuss.Subscribe("discovery-responses");
//            
//            Console.ReadLine();
//            discoveryBuss.Unsubscribe();
//            Console.ReadKey();
//            discoveryBuss.Dispose();
//            _buss.Dispose();
//            Console.ReadLine();


            var socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                socket.Connect(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000));
//                var defaultWireProtocol = new DefaultWireProtocol();
//                var stream = new MemoryStream();
//                defaultWireProtocol.WriteMessage(new DefaultSerializer(stream), new GetDataMessage());
//                var buffer = stream.ToArray();
//                socket.Send(buffer, buffer.Length, SocketFlags.None);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            _tcpConnector = new TcpConnector(socket, new DefaultWireProtocol());
            _tcpConnector.MessageReceived += OnMessageReceivedFromTcpConnector;
            _tcpConnector.StateChanged += OnTcpConnectorStateChange;
            _tcpConnector.StartAsync();
            Console.ReadLine();
        }

        private static void OnMessageReceivedFromTcpConnector(object sender, MessageReceivedEventArgs args)
        {
            if (args.Message.MessageTypeName == typeof(DataResponseMessage).Name)
            {
                var message = (DataResponseMessage) args.Message;
                Console.WriteLine(message.Employees.Length);
            }
        }

        private static void OnTcpConnectorStateChange(object sender, ConnectorStateChangeEventArgs args)
        {
            if (args.NewState == ConnectionState.Connected)
            {
                _tcpConnector.SendMessage(new DataRequestMessage());
            }
        }

        private static void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryResponseMessage).Name)
            {
                var payload = (DiscoveryResponseMessage) args.Payload;
                Console.WriteLine(payload.NodIpEndPoint.ToString());
            }
            if (args.Payload.MessageTypeName == typeof(SubscribeSuccessMessage).Name)
            {
                new Thread(() =>
                {
                    while (true)
                    {
                        var discoveryRequest = new DiscoveryRequestMessage
                        {
                            ExchangeName = "Discovery",
                            QueueName = "discovery-responses",
                            BrockerIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000)
                        };
                        _buss.Direct(discoveryRequest, "discovery-requests");
                        Thread.Sleep(1000);
                    }
                }).Start();
            }
        }
    }
}