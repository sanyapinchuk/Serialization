using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class Entity:Client
    {
        public Entity():base()
        {
            this.RegistrationNumber = "none";
            this.NComponyName = "none";
            this.DataOfGeristrarion = "none";
        }
        public Entity(String RegistrationNumber, String NComponyName, String DataOfGeristrarion, bool IsNew, int Age, string Name, string Sex) : base(IsNew, Age, Name, Sex)
        {
            this.RegistrationNumber = RegistrationNumber;
            this.NComponyName = NComponyName;
            this.DataOfGeristrarion = DataOfGeristrarion;
            this.IsNew = IsNew;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
        }
        public String RegistrationNumber { get; set; }
        public String NComponyName { get; set; }
        public String DataOfGeristrarion { get; set; }
    }
}
