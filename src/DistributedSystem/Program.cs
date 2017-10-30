using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Xml.Serialization;

namespace DistributedSystem
{
    internal static class Program
    {
        public static string ConfigFilePath =
            Path.Combine(Directory.GetCurrentDirectory(), "src\\DistributedSystem\\config.xml");

        public static void Main(string[] args)
        {
            var config = new Config(ConfigFilePath);
            var fileName = Path.Combine(Directory.GetCurrentDirectory(), "src\\Node\\bin\\Debug\\Node.exe");

            config.Nodes.ForEach(node =>
            {
                string argsToSned = $"{node.NodeName} {node.MulticastIp}:{node.MulticastPort} {node.TcpIp}:{node.TcpPort}"; 
                node.ConectsTo.ForEach(nodeNmae =>
                {
                    var nodeToConnect = config.Nodes.FirstOrDefault(n => n.NodeName == nodeNmae);
                    if (nodeToConnect != null)
                    {
                        argsToSned = argsToSned + $" {nodeToConnect.TcpIp}:{nodeToConnect.TcpPort}";
                    }
                });
                Process.Start(fileName, argsToSned);
            });
            Console.ReadLine();
        }
    }
}