using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common
{
    public class XmlDataResponseMessage : Message
    {
        public bool IsLastKnownServerNode { get; set; }
        public string Employees { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsLastKnownServerNode);
            serializer.WriteStringUtf8(Employees);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsLastKnownServerNode = deserializer.ReadBoolean();
            Employees = deserializer.ReadStringUtf8();
        }
    }
}