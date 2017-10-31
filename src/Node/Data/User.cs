using System;
using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;

namespace Node.Data
{
    public class User : Message
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InstantiationTimestamp { get; set; }

        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteStringUtf8(FirstName);
            serializer.WriteStringUtf8(LastName);
            serializer.WriteInt32(Age);
            serializer.WriteDateTime(InstantiationTimestamp);
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            FirstName = deserializer.ReadStringUtf8();
            LastName = deserializer.ReadStringUtf8();
            Age = deserializer.ReadInt32();
            InstantiationTimestamp = deserializer.ReadDateTime();
        }

        public override string ToString()
        {
            return $"FirstName: {FirstName}\nLastName: {LastName}\nAge: {Age}\nCreatedOn: {InstantiationTimestamp}\n";
        }
    }
}