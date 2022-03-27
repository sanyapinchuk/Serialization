using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class Worker:Employee
    {
        public Worker():base()
        {
            this.FactoryAdress = "none";
            this.ClassWork = "none";
        }
        public Worker(String FactoryAdress, String ClassWork, String Experience, String Salary, String Function, int Age, string Name, string Sex):base(Experience, Salary, Function, Age, Name, Sex)
        {
            this.FactoryAdress = FactoryAdress;
            this.ClassWork = ClassWork;
            this.Experience = Experience;
            this.Salary = Salary;
            this.Function = Function;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
        }
        public String FactoryAdress { get; set; }
        public String ClassWork { get; set; }
    }
}
