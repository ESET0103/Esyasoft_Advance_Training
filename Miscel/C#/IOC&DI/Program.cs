namespace IOC_DI
{
    internal class Program
    {
        static void Main(string[] args)
        {
            //Console.WriteLine("Hello, World!");
            //GasEngine engine = new GasEngine();


            //Car Swift = new Car();
            //Swift.engine = new PetrolEngine();
            //Swift.setEngine(new GasEngine());
            //Swift.start();





            //  Implementing Singleton ...

            var spooler1 = PrinterSpooler.Instance;
            var spooler2 = PrinterSpooler.Instance;

            spooler1.AddPrintJob("abc");
            spooler2.AddPrintJob("dce");

            //if (ReferenceEquals(spooler1,spooler2)) {
            //    Console.WriteLine("true");  // true only when only one instace is created...
            //}
            //else
            //{
            //    Console.WriteLine("false");
            //}

            spooler1.ProcessQueue();

        }
    }
}

