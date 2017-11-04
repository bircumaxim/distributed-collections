using System;

namespace Common.Models
{
    [Serializable]
    public class Employee
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public DateTime InstantiationTimestamp { get; set; }

        public override string ToString()
        {
            return $"FirstName: {FirstName}\nLastName: {LastName}\nAge: {Age}\nCreatedOn: {InstantiationTimestamp}\n";
        }
    }
}