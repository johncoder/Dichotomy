using Dichotomy.Configuration;

namespace Dichotomy.ConsoleTest
{
    class Program
    {
        static void Main()
        {
            var config = new ConfigurationOptions("--");
            var runner = new Runner(new Service(), config);
            runner.Run();
        }
    }
}
