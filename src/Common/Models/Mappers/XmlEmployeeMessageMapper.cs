using Common.Messages.DataResponse.xml;

namespace Common.Models.Mappers
{
    public static class XmlEmployeeMessageMapper
    {
        public static XmlEmployeeMessage Map(Employee employee)
        {
            return employee == null
                ? null
                : new XmlEmployeeMessage
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Age = employee.Age,
                    InstantiationTimestamp = employee.InstantiationTimestamp
                };
        }

        public static Employee InversMap(XmlEmployeeMessage employee)
        {
            return employee == null
                ? null
                : new Employee
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Age = employee.Age,
                    InstantiationTimestamp = employee.InstantiationTimestamp
                };
        }
    }
}