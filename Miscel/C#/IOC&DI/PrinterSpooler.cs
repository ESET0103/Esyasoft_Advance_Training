using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IOC_DI
{
    internal sealed class PrinterSpooler
    {
        private static PrinterSpooler _instance = null;
        private static readonly object _lock = new object();
        private readonly Queue<string> _printQueue = new Queue<string>();

        // constructor should be private so that new object could not be created;

        private PrinterSpooler() { }


        // create one instance if it does not have any...
        public static PrinterSpooler Instance
        {
            get
            {
                lock (_lock)
                {
                    if (_instance == null)
                        _instance = new PrinterSpooler();
                    return _instance;
                }
            }
        }

        // add job to print
        public void AddPrintJob(string document)
        {
            _printQueue.Enqueue(document);
            Console.WriteLine($"Added '{document}' to print queue.");
        }

        public void ProcessQueue()
        {
            while (_printQueue.Count > 0)
            {
                var doc = _printQueue.Dequeue();
                Console.WriteLine($"Printing document: {doc}");
                Thread.Sleep(500); // Simulate printing delay
            }
        }
    }
}

