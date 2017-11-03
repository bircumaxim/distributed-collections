using System.Collections.Generic;

namespace DistributedSystem
{
    public class NodeConfig
    {
        public string Name { get; set; }
        public string MulticastIpEndPoint { get; set; }
        public string UdpIpEndPoint { get; set; }
        public string TcpIpEndPoint { get; set; }
        public List<string> KnownEndPoints { get; set; }
        public string DataObjectsCount { get; set; }
        public string DataType { get; set; }

        public NodeConfig()
        {
            KnownEndPoints = new List<string>();
        }
    }
}