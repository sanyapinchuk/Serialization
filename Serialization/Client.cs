using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public abstract class Client: Person
    {
        public Client():base()
        {
            IsNew = false;
        }
        public Client(bool IsNew, int Age, string Name, string Sex):base(Age, Name, Sex)
        {
            this.IsNew = IsNew;
            this.Name = Name;
            this.Sex = Sex;
            this.Age = Age;
        }
        public bool IsNew { get; set; }
    }
}
