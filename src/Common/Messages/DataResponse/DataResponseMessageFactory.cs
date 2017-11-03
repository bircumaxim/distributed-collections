using System.Collections.Generic;
using Common.Messages.DataResponse.Binary;
using Common.Models;
using Common.Models.Mappers;

namespace Common.Messages.DataResponse
{
    public class DataResponseMessageFactory
    {
        public static DataResponseMessage GetDataResponseMessage(DataType dataType, List<Employee> employees)
        {
            switch (dataType)
            {
                default:
                    return new BinaryDataResponseMessage
                    {
                        EmployeesMessage = employees.ConvertAll(EmplyeeToEmployeeMessageMapper.Map).ToArray()
                    };
            }
        }
    }
}