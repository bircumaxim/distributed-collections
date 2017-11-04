using System;
using System.Collections.Generic;
using System.Linq;
using Common.Messages.DataResponse.Binary;
using Common.Messages.DataResponse.Json;
using Common.Messages.DataResponse.xml;
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
                case DataType.Binary:
                    return new BinaryDataResponseMessage
                    {
                        EmployeeMessages = employees.ConvertAll(BinaryEmployeeMessageMapper.Map).ToArray()
                    };
                case DataType.Json:
                    return new JsonDataResponseMessage
                    {
                        EmployeeMessages = JsonHelper.Serealize(employees)
                    };
                case DataType.Xml:
                    return new XmlDataResponseMessage
                    {
                        EmployeeMessages = XmlHelper.Serealize(new EmployeeList
                        {
                            Employees = employees.ConvertAll(XmlEmployeeMessageMapper.Map).ToList()
                        })
                    };
                default:
                    throw new Exception($"{dataType} is not supported");
            }
        }
    }
}