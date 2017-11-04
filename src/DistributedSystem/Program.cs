using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using Common;
using Common.Messages;
using Serialization.WireProtocol;
using Transport.Connectors.UdpMulticast;

namespace DistributedSystem
{
    internal static class Program
    {
        public static string ConfigPath = Path.Combine(Directory.GetCurrentDirectory(), "DistributedSystem/config.xml");
        public static string NodeExePath = Path.Combine(Directory.GetCurrentDirectory(), "Node/bin/Debug/Node.exe");
        private static Config _config;

        public static void Main(string[] args)
        {
            _config = new Config(ConfigPath);
            _config.ServerNodes.ForEach(StartProcesForNode);
            SendConnectGraphMessage();
        }

        private static void SendConnectGraphMessage()
        {
            Thread.Sleep(2000);
            var list = _config.ServerNodes.Select(node => node.MulticastIpEndPoint).Distinct().ToList();
            list.ForEach(ip =>
            {
                var ipParts = ip.Split(':');
                var ipEndPoint = new IPEndPoint(IPAddress.Parse(ipParts[0]), Convert.ToInt32(ipParts[1]));
                new UdpMulticastSender(ipEndPoint, new DefaultWireProtocol(), 4500).Send(new ConnectTheGraphMessage());
            });   
        }

        public static void StartProcesForNode(NodeConfig nodeConfig)
        {
            var serverInfoArgs = new List<string>
            {
                nodeConfig.Name,
                nodeConfig.MulticastIpEndPoint,
                nodeConfig.UdpIpEndPoint,
                nodeConfig.TcpIpEndPoint,
                nodeConfig.DataType,
                nodeConfig.DataObjectsCount
            };
            var knownIpsForNode = GetKnownIpsForNode(nodeConfig);
            Process.Start(NodeExePath, BuildArgs(serverInfoArgs.Concat(knownIpsForNode).ToList()));
        }

        public static List<string> GetKnownIpsForNode(NodeConfig nodeConfig)
        {
            return nodeConfig.KnownEndPoints
                .Select(knowedNode =>
                    _config.ServerNodes.FirstOrDefault(node => node.Name == knowedNode)?.TcpIpEndPoint)
                .ToList();
        }

        public static string BuildArgs(List<string> argsList)
        {
            var args = "";
            argsList.ForEach(argItem => args = args + $" {argItem}");
            return args;
        }
    }
}