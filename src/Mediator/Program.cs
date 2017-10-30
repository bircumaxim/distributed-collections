using System;
using System.Net;
using System.Net.Sockets;
using Mediator.Messages;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Transport.Connectors.Tcp;

namespace Mediator
{
    internal class Program
    {
        
        public static void Main(string[] args)
        {
            Buss buss = BussFactory.Instance.GetBussFor("Discovery");
            buss.MessageReceived += OnMessageReceived;
            var discoveryRequest = new DiscoveryRequest
            {
                ExchangeName = "Discovery",
                QueueName =  "discovery-responses",
                BrockerIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000)
            };
            buss.Direct(discoveryRequest, "discovery-requests");
            buss.Subscribe("discovery-responses");
            Console.ReadLine();
        }

        private static void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryResponse).Name)
            {
                var payload = (DiscoveryResponse) args.Payload;
                Console.WriteLine(payload.NodIpEndPoint.ToString());
                
                Socket socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.Connect(IPAddress.Parse("127.0.0.1"), 5000);
            }   
        }
    }
}