using System;
using System.Net;
using System.Xml;

namespace Mediator
{
    public class Config
    {
        public IPEndPoint BrokerIpEndPoint { get; set; }
        public string DiscoveryExchangeName { get; set; }
        public string DiscoveryRequestsQueueName { get; set; }
        public string DiscoveryResponseExchcengeName { get; set; }
        public string DiscoveryResponsesQueueName { get; set; }

        public Config(string configFilePath)
        {
            var configsDocument = new XmlDocument();
            configsDocument.Load(configFilePath);
            LoadConfigFromFile(configsDocument.SelectSingleNode("Mediator"));
        }

        private void LoadConfigFromFile(XmlNode configDocument)
        {
            SetBroker(configDocument.SelectSingleNode("Broker"));
            DiscoveryExchangeName = configDocument.SelectSingleNode("DiscoveryExchange")?.Attributes
                ?.GetNamedItem("Name").Value;
            DiscoveryRequestsQueueName = configDocument.SelectSingleNode("DiscoveryRequestsQueue")?.Attributes
                ?.GetNamedItem("Name").Value;
            DiscoveryResponseExchcengeName = configDocument.SelectSingleNode("DiscoveryResponseExchcenge")?.Attributes
                ?.GetNamedItem("Name").Value;
            DiscoveryResponsesQueueName = configDocument.SelectSingleNode("DiscoveryResponsesQueue")?.Attributes
                ?.GetNamedItem("Name").Value;
        }

        private void SetBroker(XmlNode configNode)
        {
            var brokerIp = configNode?.Attributes?.GetNamedItem("Ip").Value ?? "127.0.0.1";
            var brokerPort = Convert.ToInt32(configNode?.Attributes?.GetNamedItem("Port").Value ?? "8000");
            BrokerIpEndPoint = new IPEndPoint(IPAddress.Parse(brokerIp), brokerPort);
        }
    }
}