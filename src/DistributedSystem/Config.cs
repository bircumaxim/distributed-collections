using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;

namespace DistributedSystem
{
    public class Config
    {
        
        private const string DefaultObjectsCount = "10";
        private const string DefaultMulticastIpEndPoint = "224.5.6.7:7000";
        
        public List<NodeConfig> ServerNodes { get; set; }

        public Config(string configFilePath)
        {
            var configsXmlDocument = new XmlDocument();
            configsXmlDocument.Load(configFilePath);
            ServerNodes = new List<NodeConfig>();
            LoadNodesFormConfigFile(configsXmlDocument.SelectSingleNode("System"));
        }

        private void LoadNodesFormConfigFile(XmlNode nodesXmlNode)
        {
            foreach (XmlElement nodeXmlElement in nodesXmlNode)
            {
                ServerNodes.Add(GetNode(nodeXmlElement));
            }
        }

        private NodeConfig GetNode(XmlNode nodesXmlNode)
        {
            if (nodesXmlNode.Attributes != null)
            {
                var nodeConfig = new NodeConfig
                {
                    Name = nodesXmlNode.Attributes.GetNamedItem("Name").Value ?? Guid.NewGuid().ToString(),
                    MulticastIpEndPoint = nodesXmlNode.Attributes.GetNamedItem("MulticastIpEndPoint").Value ?? DefaultMulticastIpEndPoint,
                    UdpIpEndPoint = nodesXmlNode.Attributes.GetNamedItem("UdpIpEndPoint").Value,
                    TcpIpEndPoint = nodesXmlNode.Attributes.GetNamedItem("TcpIpEndPoint").Value,
                    DataObjectsCount = nodesXmlNode.Attributes.GetNamedItem("DataObjects").Value ?? DefaultObjectsCount,
                    KnownEndPoints = GetKnownEndPoints(nodesXmlNode)
                };

                return nodeConfig;
            }
            return null;
        }

        private List<string> GetKnownEndPoints(XmlNode nodesXmlNode)
        {
            return (from XmlElement nodeXmlElement in nodesXmlNode
                select nodeXmlElement.Attributes.GetNamedItem("Name").Value).ToList();
        }
    }
}