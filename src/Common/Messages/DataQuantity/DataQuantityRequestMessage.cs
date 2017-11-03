using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataQuantity
{
    public class DataQuantityRequestMessage : Message
    {
        public bool IsLastServerNode { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsLastServerNode);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsLastServerNode = deserializer.ReadBoolean();
        }
    }
}