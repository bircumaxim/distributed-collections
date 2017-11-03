using System.Collections.Generic;
using System.IO;
using Common.Models;
using Common.Models.Filters;
using Serialization;
using Serialization.Deserializer;
using Serialization.Serializer;
using Serialization.WireProtocol;

namespace Common.Messages.DataRequest
{
    public class DataRequestMessage : Message
    {
        public bool IsFromAServerNode { get; set; }
        public bool IsLastKnownServerNode { get; set; }
        public int RequestTimeout { get; set; }
        public List<Filter<Employee>> Filters { get; set; }

        public DataRequestMessage()
        {
            Filters = new List<Filter<Employee>>();
        }
        
        public override void Serialize(ISerializer serializer)
        {
            base.Serialize(serializer);
            serializer.WriteBoolean(IsFromAServerNode);
            serializer.WriteBoolean(IsLastKnownServerNode);
            serializer.WriteInt32(RequestTimeout);
            serializer.WriteInt32(Filters.Count);
            Filters.ForEach(fitler => serializer.WriteByteArray(GetBytesFormFilter(fitler)));
        }

        public override void Deserialize(IDeserializer deserializer)
        {
            base.Deserialize(deserializer);
            IsFromAServerNode = deserializer.ReadBoolean();
            IsLastKnownServerNode = deserializer.ReadBoolean();
            RequestTimeout = deserializer.ReadInt32();
            var filtersCount = deserializer.ReadInt32();
            for (var i = 0; i < filtersCount; i++)
            {
                Filters.Add(GetFilterFormBytes(deserializer.ReadByteArray()));
            }
        }

        private Filter<Employee> GetFilterFormBytes(byte[] filterBytes)
        {
            var wireProtocol = new DefaultWireProtocol();
            return (Filter<Employee>) wireProtocol.ReadMessage(new DefaultDeserializer(new MemoryStream(filterBytes)));
        }

        private byte[] GetBytesFormFilter(Filter<Employee> filter)
        {
            var wireProtocol = new DefaultWireProtocol();
            var stream = new MemoryStream();
            wireProtocol.WriteMessage(new DefaultSerializer(stream), filter);
            return stream.ToArray();
        }   
    }
}