using System.Collections.Generic;
using System.Linq;
using Common.Models;
using Common.Models.Filters;

namespace Node.Data
{
    public class DataManager
    {
        private readonly List<Employee> _employees;

        public DataManager(int numberOfEmployeesToGenerate)
        {
            _employees = new List<Employee>();
            GenerateRandomEmplyees(numberOfEmployeesToGenerate);
        }

        private void GenerateRandomEmplyees(int numberOfEmplyeesToGenerate)
        {
            for (var i = 0; i < numberOfEmplyeesToGenerate; i++)
            {
                _employees.Add(RandomDataGenerator.GetNewRandomEmployee());
            }
        }

        public Employee[] GetEmployees(List<Filter<Employee>> filters = null)
        {
            var employees = _employees.Select(empl => empl).ToArray();
            if (filters != null)
                foreach (var filter in filters)
                {
                    employees = filter.Execute(employees.ToArray());
                }
            return employees;
        }
    }
}