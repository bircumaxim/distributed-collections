using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Node
{
    public class BytesHelper
    { 
        public static long GetObjectSizeInBytes(object obj)
        {
            if(obj == null)
                return 0;
            
            var binaryFormatter = new BinaryFormatter();
            var memoryStream = new MemoryStream();
            binaryFormatter.Serialize(memoryStream, obj);
            return memoryStream.Length;
        }

    }
}