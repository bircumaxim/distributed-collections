using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataResponse
{
    public class DataResponseMessage : Message
    {
        public bool IsLastKnownServerNode { get; set; }
       
        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsLastKnownServerNode);
           
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsLastKnownServerNode = deserializer.ReadBoolean();
        }
    }
}