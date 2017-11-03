using System;
using System.Net;
using Messages.Payload;

namespace Node
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            var payloadMessage = new PayloadMessage();

            var nodeConfig = args.Length > 0 ? GetNodeConfigFromArgs(args) : new NodeConfig();
            var nodeService = new NodeService(nodeConfig);
            nodeService.StartAsync();
            Console.WriteLine(nodeConfig);
            Console.ReadKey();
            nodeService.Stop();
            Console.WriteLine("\nServer stoped.\nPress any key to exit.");
            Console.ReadKey(); 
        }

        public static NodeConfig GetNodeConfigFromArgs(string[] args)
        {
            var nodeConfig = new NodeConfig
            {
                Name = args[0],
                MulticastIpEndPoint = GetIpEndPointFromString(args[1]),
                UdpIpEndPoint = GetIpEndPointFromString(args[2]),
                TcpIpEndPoint = GetIpEndPointFromString(args[3]),
                DataObjectsCount = Convert.ToInt32(args[4])
            };
            for (var i = 5; i < args.Length; i++)
            {
                nodeConfig.KnownEndPoints.Add(GetIpEndPointFromString(args[i]));
            }
            return nodeConfig;
        }

        public static IPEndPoint GetIpEndPointFromString(string address)
        {
            var addressParts = address.Split(':');
            return new IPEndPoint(IPAddress.Parse(addressParts[0]), Convert.ToInt32(addressParts[1]));
        }
    }
}