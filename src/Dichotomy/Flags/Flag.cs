using System;

namespace Dichotomy.Flags
{
    public class Flag
    {
        public string Name { get; set; }
        public Action Action { get; set; }
        public string Description { get; set; }
        public static string Convention { get; set; }
    }
}