using System.Collections.Generic;
using System.Linq;
using Common;
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

        public List<Employee> GetEmployees(List<Filter<Employee>> filters = null)
        {
            var employees = _employees.Select(empl => empl);
            if (filters != null)
                employees = filters.Aggregate(employees, (current, filter) => filter.Execute(current.ToArray()));
            return employees.ToList();
        }
    }
}