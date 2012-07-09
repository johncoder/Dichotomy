using System;

namespace Dichotomy.Flags
{
    public class HelpFlag : Flag
    {
        public HelpFlag(string name, Action action)
        {
            Name = name;
            Action = action;
            Description = "Displays information about available command line flags.";
        }
    }
}