using System.Collections.Generic;
using Common.Models;

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

        public List<Employee> GetEmployees()
        {
            return _employees;
        }
    }
}