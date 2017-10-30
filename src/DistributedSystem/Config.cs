using System.Collections.Generic;
using System.Xml;

namespace DistributedSystem
{
    public class Config
    {
        public List<NodeConfig> Nodes { get; set; }
        
        public Config(string configFilePath)
        {
            var configsXmlDocument = new XmlDocument();
            configsXmlDocument.Load(configFilePath);
            Nodes = new List<NodeConfig>();

            LoadNodesFormConfigFile(configsXmlDocument.SelectSingleNode("DistributedSystem"));
        }

        private void LoadNodesFormConfigFile(XmlNode nodesXmlNode)
        {
            foreach (XmlElement nodeXmlElement in nodesXmlNode)
            {
                Nodes.Add(GetNode(nodeXmlElement));
            }
        }

        private NodeConfig GetNode(XmlNode nodesXmlNode)
        {
            if (nodesXmlNode.Attributes != null)
            {
                var nodeConfig = new NodeConfig
                {
                    NodeName = nodesXmlNode.Attributes.GetNamedItem("Name").Value,
                    MulticastIp = nodesXmlNode.Attributes.GetNamedItem("MulticastIp").Value,
                    MulticastPort = nodesXmlNode.Attributes.GetNamedItem("MulticastPort").Value,
                    TcpIp = nodesXmlNode.Attributes.GetNamedItem("TcpIp").Value,
                    TcpPort = nodesXmlNode.Attributes.GetNamedItem("TcpPort").Value,
                    ConectsTo = GetNodesToConnectTo(nodesXmlNode)
                };
            
                return nodeConfig;
            }

            return null;
        }
        
        private List<string> GetNodesToConnectTo(XmlNode nodesXmlNode)
        {
            var nodesToConnectTo = new List<string>();
            foreach (XmlElement nodeXmlElement in nodesXmlNode)
            {
                nodesToConnectTo.Add(nodeXmlElement.Attributes.GetNamedItem("Name").Value);
            }
            return nodesToConnectTo;
        }
    }
}