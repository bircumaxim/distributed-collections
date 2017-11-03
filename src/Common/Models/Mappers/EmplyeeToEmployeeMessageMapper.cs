using Common.Messages.DataResponse.Binary;

namespace Common.Models.Mappers
{
    public static class EmplyeeToEmployeeMessageMapper
    {
        public static EmployeeMessage Map(Employee employee)
        {
            return employee == null
                ? null
                : new EmployeeMessage
                {
                    FirstName = employee.FirstName,
                    LastName = employee.LastName,
                    Age = employee.Age,
                    InstantiationTimestamp = employee.InstantiationTimestamp
                };
        }
    }
}