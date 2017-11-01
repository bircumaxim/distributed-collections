using System;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using Mediator.Messages;
using MessageBuss.Buss;
using MessageBuss.Buss.Events;
using Messages.Subscribe;
using Transport.Connectors.Tcp;

namespace Mediator
{
    internal class Program
    {
        private static Buss _buss;

        public static void Main(string[] args)
        {
            _buss = BussFactory.Instance.GetBussFor("Discovery");
            
            
            Buss discoveryBuss = BussFactory.Instance.GetBussFor("DiscoveryResponse");
            discoveryBuss.MessageReceived += OnMessageReceived;
            discoveryBuss.Subscribe("discovery-responses");
            
            Console.ReadLine();
            discoveryBuss.Unsubscribe();
            discoveryBuss.Dispose();
            _buss.Dispose();
            Console.ReadLine();
        }

        private static void OnMessageReceived(object sender, MessegeReceviedEventArgs args)
        {
            Console.WriteLine(args.Payload.MessageTypeName);
            if (args.Payload.MessageTypeName == typeof(DiscoveryResponse).Name)
            {
                var payload = (DiscoveryResponse) args.Payload;
                Console.WriteLine(payload.NodIpEndPoint.ToString());
            }
            if (args.Payload.MessageTypeName == typeof(SubscribeSuccessMessage).Name)
            {
                var discoveryRequest = new DiscoveryRequest
                {
                    ExchangeName = "Discovery",
                    QueueName =  "discovery-responses",
                    BrockerIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8000)
                };
                _buss.Direct(discoveryRequest, "discovery-requests");
                Thread.Sleep(1000);
            }
        }
    }
}