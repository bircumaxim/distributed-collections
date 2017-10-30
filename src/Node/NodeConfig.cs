using System.Collections.Generic;

namespace Node
{
    public class NodeConfig
    {
        public string NodeName { get; set; }
        public string MulticastIp { get; set; }
        public string MulticastPort { get; set; }
        public string TcpIp { get; set; }
        public string TcpPort { get; set; }
        public List<string> ConectsTo { get; set; }

        public NodeConfig()
        {
            ConectsTo = new List<string>();
        }
    }
}