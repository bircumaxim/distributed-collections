using Common.Messages.DataResponse.Binary;

namespace Common.Models.Mappers
{
    public static class BinaryEmployeeMessageMapper
    {
        public static BinaryEmployeeMessage Map(Employee employee)
        {
            return employee == null
                ? null
                : new BinaryEmployeeMessage
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Age = employee.Age,
                    InstantiationTimestamp = employee.InstantiationTimestamp
                };
        }

        public static Employee InversMap(BinaryEmployeeMessage employee)
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