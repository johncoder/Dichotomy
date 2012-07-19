using Dichotomy.Configuration;
using Dichotomy.Helpers;

namespace Dichotomy.ConsoleTest
{
    class Program
    {
        static void Main()
        {
            ServiceManager.Initialize("MyService", "My Service", "This is my special service that doesn't do much of anything!");

            var config = new ConfigurationOptions("--");
            var runner = new Runner(new Service(), config);
            runner.Run();
        }
    }
}
