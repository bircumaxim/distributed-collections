using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common
{
    public class GetDataMessage : Message
    {
        public bool IsFromAServerNode { get; set; }
        public bool IsLastKnownServerNode { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsFromAServerNode);
            serializer.WriteBoolean(IsLastKnownServerNode);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsFromAServerNode = deserializer.ReadBoolean();
            IsLastKnownServerNode = deserializer.ReadBoolean();
        }
    }
}