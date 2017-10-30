using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml.Serialization;
using Serialization.WireProtocol;

namespace Node
{
    internal class Program
    {
        public static void Main(string[] args)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(NodeConfig));
//            String config = "";
            foreach (var s in args)
            {
                Console.WriteLine(s);
            }
//            using (TextReader reader = new StringReader(Encoding.ASCII.GetString(args[0].ToCharArray().Select(c => (byte)c).ToArray())))
//            {
//                NodeConfig result = (NodeConfig) serializer.Deserialize(reader);
//                Console.WriteLine(result.NodeName);
//            }
//              
            
            
            Console.ReadLine();
        }

        private static void StartNode()
        {
            var node = new Node("test", new IPEndPoint(IPAddress.Parse("224.5.6.7"), 7000),
                new IPEndPoint(IPAddress.Parse("127.0.0.1"), 5000), new DefaultWireProtocol());
            Console.ReadKey();
        }
    }
}