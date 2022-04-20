using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class ListPersons
    {
        public Individual Individuals { get; set; }
        public Entity Entitys { get; set; }
        public Manager Managers { get; set; }
        public Worker Workers { get; set; }
    }
}
