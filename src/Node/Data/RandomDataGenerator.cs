using System;
using Common.Models;

namespace Node.Data
{
    public static class RandomDataGenerator
    {
        private static readonly Random Rand = new Random(DateTime.Now.Second);
        private static readonly string[] Names = { "aaron", "abdul", "abe", "abel", "abraham", "adam", "adan", "adolfo", "adolph", "adrian", "abby", "abigail", "adele", "adrian"};
        
        public static Employee GetNewRandomEmployee()
        {
            return new Employee
            {
                FirstName = GetRandName(),
                LastName = GetRandName(),
                Age = Rand.Next(1, 99),
                InstantiationTimestamp = DateTime.Now
            };
        }
        
        
        public static string GetRandName()
        {
            return Names[Rand.Next(0, Names.Length - 1)];
        }
    }
}