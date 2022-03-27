using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class Individual:Client
    {
        public Individual():base()
        {
            this.IsPassport = false;
            this.IsAbonement = false;
        }
        public Individual(bool IsPassport, bool IsAbonement, bool IsNew, int Age, string Name, string Sex):base(IsNew, Age, Name, Sex)
        {
            this.IsPassport = IsPassport;
            this.IsAbonement = IsAbonement;
            this.IsNew = IsNew;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;

        }
        public bool IsPassport { get; set; }
        public bool IsAbonement { get; set; }
    }
}
