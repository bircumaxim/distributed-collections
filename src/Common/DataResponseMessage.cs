using Common.Models;
using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common
{
    public class DataResponseMessage : Message
    {
        public bool IsLastKnownServerNode { get; set; }
        public Employee[] Employees { get; set; }
        
        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsLastKnownServerNode);
            serializer.WriteObjectArray(Employees);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsLastKnownServerNode = deserializer.ReadBoolean();
            Employees = deserializer.ReadObjectArray(() => new Employee());
        }
    }
}