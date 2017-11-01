using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

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
        }

        public static void StartProcesForNode(NodeConfig nodeConfig)
        {
            var serverInfoArgs = new List<string>
            {
                nodeConfig.Name,
                nodeConfig.MulticastIpEndPoint,
                nodeConfig.UdpIpEndPoint,
                nodeConfig.TcpIpEndPoint,
                nodeConfig.DataObjectsCount
            };
            Process.Start(NodeExePath, BuildArgs(serverInfoArgs.Concat(GetKnownIpsForNode(nodeConfig)).ToList()));
        }

        public static List<string> GetKnownIpsForNode(NodeConfig nodeConfig)
        {
            return nodeConfig.KnownEndPoints
                .Select(knowedNode => _config.ServerNodes.FirstOrDefault(node => node.Name == knowedNode)?.TcpIpEndPoint)
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