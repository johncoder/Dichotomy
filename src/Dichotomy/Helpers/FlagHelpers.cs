using System;
using System.Linq;
using Dichotomy.Configuration;

namespace Dichotomy.Helpers
{
    internal static class FlagHelpers
    {
        public static void WriteAllCommands(this ConfigurationOptions config)
        {
            Console.WriteLine("Command Line Arguments:");

            var max = config.CommandlineFlags.Values.Max(f => f.Name.Length);

            foreach (var flag in config.CommandlineFlags.Values)
            {
                Console.WriteLine("    {0}\t{1}", (config.CommandlineConvention + flag.Name).PadRight(max + 2, ' '), flag.Description);
            }
        }
    }
}
