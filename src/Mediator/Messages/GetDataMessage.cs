using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Mediator.Messages
{
    public class GetDataMessage : Message
    {
        public bool IsFromAServerNode { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsFromAServerNode);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsFromAServerNode = deserializer.ReadBoolean();
        }
    }
}