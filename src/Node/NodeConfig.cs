using System;
using System.Collections.Generic;
using System.Net;

namespace Node
{
    public class NodeConfig
    {
        public string Name { get; set; }
        public IPEndPoint MulticastIpEndPoint { get; set; }
        public IPEndPoint UdpIpEndPoint { get; set; }
        public IPEndPoint TcpIpEndPoint { get; set; }
        public List<IPEndPoint> KnownEndPoints { get; set; }
        public int DataObjectsCount { get; set; }

        public NodeConfig()
        {
            Name = Guid.NewGuid().ToString();
            MulticastIpEndPoint = new IPEndPoint(IPAddress.Parse("224.5.6.7"), 7000);
            UdpIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 3000);
            TcpIpEndPoint = new IPEndPoint(IPAddress.Parse("127.0.0.1"), 4000);
            DataObjectsCount = 10;
            KnownEndPoints = new List<IPEndPoint>();
        }

        public override string ToString()
        {
            var info = $"Name: {Name}\n" +
                       $"MulticastIp: {MulticastIpEndPoint}\n" +
                       $"UdpIpEndPoint: {UdpIpEndPoint}\n" +
                       $"TcpIpEndPoint: {TcpIpEndPoint}\n" +
                       $"DataObjects: {DataObjectsCount}\n";

            if (KnownEndPoints.Count > 0)
            {
                info = info + "Connected to: \n";
                KnownEndPoints.ForEach(ip => info = info + $"\t{ip}\n");
            }
            return info;
        }
    }
}