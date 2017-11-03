using System.Collections.Generic;
using System.IO;
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
           var defaultWireProtocol = new DefaultWireProtocol();
            long totayBytes = 0;
           data.ForEach(item =>
           {
               var stream = new MemoryStream();
               defaultWireProtocol.WriteMessage(new DefaultSerializer(stream), item);
               totayBytes += stream.Length;
           });
            return totayBytes;
        }

    }
}