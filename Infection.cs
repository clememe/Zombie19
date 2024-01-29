using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombie19
{
    public class Infection
    {
        public int ID { get; set; }

        public Character Character { get; set; }

        public string variation { get; set; }

        private static int nextId = 0;

        public Infection(Character character)
        {
            ID = nextId++;

            this.Character = character;

            this.variation = character.Variation;
        }
    }
}
