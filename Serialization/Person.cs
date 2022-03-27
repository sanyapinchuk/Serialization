using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using System.IO;
namespace Serialization
{
    public abstract class Person:object
    {
        public Person()
        {
            Age = 0;
            Name = "none";
            Sex = "none";
        } 

        public Person(int Age, string Name, string Sex)
        {
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
        }

        public int Age { get; set; }
        public string Name { get; set; }
        public string Sex { get; set; }
        

    }
}
