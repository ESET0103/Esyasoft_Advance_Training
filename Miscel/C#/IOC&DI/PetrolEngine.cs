using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC_DI
{
    internal class PetrolEngine : IEngine
    {
        public void Ignite()
        {
            Console.WriteLine("PetrolEngine started");
        }
    }
}
