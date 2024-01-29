using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zombie19
{
    internal class Group
    {
        public int Id {  get; set; }

        public List<Character> Characters { get; set; }

        private static int nextId = 0;

        public Group()
        {
            Characters = new List<Character>();
            Id = nextId++;
        }
    }
}
