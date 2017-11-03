using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;
using Common;
using Common.Models;
using Messages.Payload;
using Node.Data;

namespace Node
{
    internal class Program
    {
        public static void Main(string[] args)
        {
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
                DataType = (DataType) Enum.Parse(typeof(DataType), args[4]),
                DataObjectsCount = Convert.ToInt32(args[5])
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