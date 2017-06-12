using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace prng_of_fortune
{
    public class Player
    {
        public string name { get; set; }
        public int cash { get; set; }

        public Player()
        {
            this.name = Console.ReadLine();
            this.cash = 0;
        }

    }

}
