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
            SomeList.Add(3);
            SomeList.Add(5);

            Arr[0] = new int[] { 1, 2, 3 };
            Arr[1] = new int[] { 4, 5, 9 };
        }
        public Individual(bool IsPassport, bool IsAbonement, bool IsNew, int Age, string Name, string Sex):base(IsNew, Age, Name, Sex)
        {
            this.IsPassport = IsPassport;
            this.IsAbonement = IsAbonement;
            this.IsNew = IsNew;
            this.Age = Age;
            this.Name = Name;
            this.Sex = Sex;
            SomeList.Add(3);
            SomeList.Add(5);    

            Arr[0] = new int[] { 1, 2, 3 };
            Arr[1] = new int[] { 4, 5, 6 };
        }
        public bool IsPassport { get; set; }
        public bool IsAbonement { get; set; }

        public List<int> SomeList { get; set; } = new List<int>();

        public int[][] Arr { get; set; } = new int[2][];
    }
}
