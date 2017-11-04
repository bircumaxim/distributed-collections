using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataResponse.Binary
{
    public class BinaryDataResponseMessage : DataResponseMessage
    {
        public BinaryEmployeeMessage[] EmployeeMessages { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteObjectArray(EmployeeMessages);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            EmployeeMessages = deserializer.ReadObjectArray(() => new BinaryEmployeeMessage());
        }
    }
}