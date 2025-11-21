using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC_DI
{
    internal class Car
    {
        // Through Property ...

        //public IEngine engine {  get; set; }


        // Through Constructor ...
        //public Car(IEngine engine)
        //{
        //    this.engine = engine;
        //}


        // Through setter method ...
        private IEngine engine;

        public void setEngine(IEngine engine)
        {
            this.engine = engine;
        }
        public void start()
        {
            engine.Ignite();
        }
    }
}
