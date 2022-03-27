using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public abstract class Employee:Person
    {
        public Employee():base()
        {
            this.Experience = "none";
            this.Salary = "none";
            this.Function = "none";
        }
        public Employee(String Experience, String Salary, String Function, int Age, string Name, string Sex):base(Age,Name,Sex)
        {
            this.Experience = Experience;
            this.Salary = Salary;
            this.Function = Function;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
        }
        public String Experience { get; set; }
        public String Salary { get; set; }
        public String Function { get; set; }
    }
}
