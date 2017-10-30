using System.Net;
using Messages.Payload;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Mediator.Messages
{
    public class DiscoveryRequest : PayloadMessage
    {
        public IPEndPoint BrockerIpEndPoint { get; set; }
        public string ExchangeName { get; set; }
        public string QueueName { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUtf8(BrockerIpEndPoint.Address.ToString());
            serializer.WriteInt32(BrockerIpEndPoint.Port);
            serializer.WriteStringUtf8(ExchangeName);
            serializer.WriteStringUtf8(QueueName);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            var ip = deserializer.ReadStringUtf8();
            var port = deserializer.ReadInt32();
            BrockerIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
            ExchangeName = deserializer.ReadStringUtf8();
            QueueName = deserializer.ReadStringUtf8();
        }
    }
}