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
//            Buss buss = BussFactory.Instance.GetBussFor("Discovery");
//            var discoveryRequest = new DiscoveryRequest
//            {
//                ExchangeName = "Discovery",
//                QueueName =  "discovery-responses",
//                BrockerIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000)
//            };
//            buss.Direct(discoveryRequest, "discovery-requests");
//            
//            Buss discoveryBuss = BussFactory.Instance.GetBussFor("DiscoveryResponse");
//            discoveryBuss.MessageReceived += OnMessageReceived;
//            discoveryBuss.Subscribe("discovery-responses");
//            Console.ReadLine();
//            discoveryBuss.Unsubscribe();
//            discoveryBuss.Dispose();
//            buss.Dispose();
//            Console.ReadLine();
            var buss = BussFactory.Instance.GetBussFor("udp", new IPEndPoint(IPAddress.Parse("127.0.0.1"),8000), new IPEndPoint(IPAddress.Parse("127.0.0.1"),3000));
            buss.Publish("Discovery", "discovery-responses", new DiscoveryResponse {NodIpEndPoint = new IPEndPoint(IPAddress.Any, 3000)});
            Console.ReadKey();
        }

        private static void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            if (args.Payload.MessageTypeName == typeof(DiscoveryResponse).Name)
            {
                var payload = (DiscoveryResponse) args.Payload;
                Console.WriteLine(payload.NodIpEndPoint.ToString());
            }   
        }
    }
}