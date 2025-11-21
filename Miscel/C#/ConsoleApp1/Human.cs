using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Human
    {
        string hname { get; set; }
        public Human(string hname)
        {
            this.hname = hname; 
            Console.WriteLine($"I am a Human {hname}");
        }

        public virtual void eat()
        {
            Console.WriteLine("Human can eat anything");
        }
    }
}
