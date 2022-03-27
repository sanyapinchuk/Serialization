using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class Manager:Employee
    {
        public Manager() : base()
        {
            this.OfficeStage = "none";
            this.OfficeName = "none";
            this.WorkPhone = "none";
        }
        public Manager(String OfficeStage, String OfficeName, String WorkPhone, String Experience, String Salary, String Function, int Age, string Name, string Sex):base(Experience,Salary, Function, Age, Name, Sex)
        {
            this.OfficeStage = OfficeStage;
            this.OfficeName = OfficeName;
            this.WorkPhone = WorkPhone;
            this.Experience = Experience;
            this.Salary = Salary;
            this.Function = Function;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
        }
        public String OfficeStage { get; set; }
        public String OfficeName { get; set; }
        public String WorkPhone { get; set; }
    }
}
