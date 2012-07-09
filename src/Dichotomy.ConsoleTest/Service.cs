using System;

namespace Dichotomy.ConsoleTest
{
    public class Service : IDichotomyService
    {
        public void Start()
        {
            Console.WriteLine("Starting");
        }

        public void Stop()
        {
            Console.WriteLine("Stopping");
        }

        public void Dispose()
        {
            Console.WriteLine("Disposing");
        }
    }
}