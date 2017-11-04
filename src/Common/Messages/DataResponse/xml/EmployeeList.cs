using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Messages.DataResponse.xml
{
    public class EmployeeList
    {
        [XmlArray("EmployeeList"), XmlArrayItem(typeof(XmlEmployeeMessage), ElementName = "Employee")]
        public List<XmlEmployeeMessage> Employees { get; set; }
    }
}