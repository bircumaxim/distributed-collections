using Serialization.Deserializer;
using Serialization.Serializer;

namespace Common.Messages.DataResponse.xml
{
    public class XmlDataResponseMessage : DataResponseMessage
    {
        public string EmployeeMessages { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUtf8(EmployeeMessages);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            EmployeeMessages = deserializer.ReadStringUtf8();
        }
    }
}