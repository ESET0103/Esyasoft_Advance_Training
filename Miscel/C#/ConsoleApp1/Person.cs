using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1
{
    internal class Person : Human
    {
        protected string pname { get; set; }
        public string gender { get; set; }

        public Person(string pname):base(pname)
        {
            this.pname = pname;
            Console.WriteLine($"I am a Person {pname}");
        }
        
        public void work()
        {
            Console.WriteLine("I work as ...");
        }

        public virtual void eat() {
            Console.WriteLine("I eat only veggies");
        }


    }
}
