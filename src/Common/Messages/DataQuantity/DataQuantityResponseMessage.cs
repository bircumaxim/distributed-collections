using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataQuantity
{
    public class DataQuantityResponseMessage : Message
    {
        public long Quantity { get; set; }
        public bool IsLastServerNode { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteInt64(Quantity);
            serializer.WriteBoolean(IsLastServerNode);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            Quantity = deserializer.ReadInt64();
            IsLastServerNode = deserializer.ReadBoolean();
        }
    }
}