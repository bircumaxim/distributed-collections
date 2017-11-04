using System.IO;
using System.Text;
using System.Xml.Linq;
using System.Xml.Schema;
using System.Xml.Serialization;
using Common.Messages.DataResponse.xml;

namespace Common
{
    public class XmlHelper
    {
        public static string Serealize(EmployeeList employeeList)
        {
            var serializer = new XmlSerializer(typeof(EmployeeList));
            var stream = new MemoryStream();
            serializer.Serialize(stream, employeeList);
            return Encoding.ASCII.GetString(stream.ToArray());
        }

        public static EmployeeList Deserealize(string employeeList)
        {
            var serializer = new XmlSerializer(typeof(EmployeeList));
            return (EmployeeList) serializer.Deserialize(new MemoryStream(Encoding.ASCII.GetBytes(employeeList)));
        }

        public static void ValidateXml(string xml, string schemaFilePath, ValidationEventHandler handler)
        {
            var xmlSchema = new XmlSchemaSet();
            xmlSchema.Add("", schemaFilePath);
            var document = XDocument.Load(new MemoryStream(Encoding.ASCII.GetBytes(xml)));
            document.Validate(xmlSchema, handler);
        }
    }
}