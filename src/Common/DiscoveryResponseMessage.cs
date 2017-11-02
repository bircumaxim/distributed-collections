using System.Net;
using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common
{
    public class DiscoveryResponseMessage : Message
    {
        public IPEndPoint NodIpEndPoint { get; set; }
        
        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUtf8(NodIpEndPoint.Address.ToString());
            serializer.WriteInt32(NodIpEndPoint.Port);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            var ip = deserializer.ReadStringUtf8();
            var port = deserializer.ReadInt32();
            NodIpEndPoint = new IPEndPoint(IPAddress.Parse(ip), port);
        }
    }
}