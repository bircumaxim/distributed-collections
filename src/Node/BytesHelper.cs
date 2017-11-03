using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Common;
using Common.Models;
using Serialization;
using Serialization.Serializer;
using Serialization.WireProtocol;

namespace Node
{
    public class BytesHelper
    { 
        public static long GetDataSizeInBytes(List<Employee> data)
        {
            var stream = new MemoryStream();
            var binaryFormatter = new BinaryFormatter();
            binaryFormatter.Serialize(stream, data);
            return stream.Length;
        }

    }
}