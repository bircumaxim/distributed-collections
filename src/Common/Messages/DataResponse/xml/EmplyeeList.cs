using System.Collections.Generic;
using System.Xml.Serialization;

namespace Common.Messages.DataResponse.xml
{
    public class EmplyeeList
    {
        [XmlArray("EmployeeList"), XmlArrayItem(typeof(EmployeeMessage), ElementName = "Employee")]
        public List<EmployeeMessage> Employees { get; set; }
    }
}