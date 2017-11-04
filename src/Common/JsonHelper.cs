using System;
using System.Collections.Generic;
using System.IO;
using Common.Models;
using System.Web.Script.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Schema;
using static Newtonsoft.Json.Schema.JsonSchema;


namespace Common
{
    public class JsonHelper
    {
        public static string Serealize(List<Employee> employeeList)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Serialize(employeeList);
        }

        public static List<Employee> Deserealize(string employeeList)
        {
            var serializer = new JavaScriptSerializer();
            return serializer.Deserialize<List<Employee>>(employeeList);
        }

        public static void ValidateJson(string jsonToValidate, string schemaFilePath, ValidationEventHandler validationEventHandler)
        {
            if (!File.Exists(schemaFilePath))
            {
                throw new Exception($"{schemaFilePath} no such file !");
            }
            using (StreamReader streamReader = new StreamReader(schemaFilePath))
            {
                var jsonSchema = Parse(streamReader.ReadToEnd());
                var test = JArray.Parse(jsonToValidate);
                test.Validate(jsonSchema, validationEventHandler);
            }
        }
    }
}