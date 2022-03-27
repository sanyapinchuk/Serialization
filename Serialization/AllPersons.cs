﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serialization
{
    public class AllPersons
    {
        public AllPersons()
        {
            //Managers.Add(new Manager());
            //Workers.Add(new Worker("Bobruisk","hard","many","2k","none",23,"Kirill","Man"));
            /*Individual.Add(new Individual());
            Individual.Add(new Individual(true,true,false,23,"Boris","Male"));*/
        }

        public List<Individual> Individuals = new List<Individual>();
        public List<Entity> Entitys  = new List<Entity>();
        public List<Manager> Managers  = new List<Manager>();
        public List<Worker> Workers = new List<Worker>();
    }
}
