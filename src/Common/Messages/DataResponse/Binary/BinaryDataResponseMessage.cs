using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataResponse.Binary
{
    public class BinaryDataResponseMessage : DataResponseMessage
    {
        public EmployeeMessage[] EmployeesMessage { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteObjectArray(EmployeesMessage);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            EmployeesMessage = deserializer.ReadObjectArray(() => new EmployeeMessage());
        }
    }
}