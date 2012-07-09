using System;
using System.Collections.Generic;
using Dichotomy.Flags;
using Dichotomy.Helpers;
using Dichotomy.Tasks;

namespace Dichotomy.Configuration
{
    public class ConfigurationOptions
    {
        internal bool CaptureCtrlC { get; set; }
        public string CommandlineConvention { get; protected set; }
        public IDictionary<string, Flag> CommandlineFlags { get; set; }
        internal ICollection<IStartupTask> StartupTasks { get; set; }

        public ConfigurationOptions() : this("-")
        {
        }

        public ConfigurationOptions(string commandlineConvention)
        {
            CaptureCtrlC = true;

            CommandlineFlags = new Dictionary<string, Flag>(StringComparer.InvariantCultureIgnoreCase);

            CommandlineConvention = commandlineConvention;
            Flag.Convention = CommandlineConvention;

            AddCommandlineFlag(new Flag[]
            {
                new InstallFlag(),
                new UninstallFlag(),
                new StartFlag(),
                new StopFlag(),
                new RestartFlag(),
                new HelpFlag("help", () => this.WriteAllCommands()),
                new HelpFlag("?", () => this.WriteAllCommands())
            });

            StartupTasks = new List<IStartupTask>
            {
                new CtrlCFixStartupTask()
            };
        }

        public void AddCommandlineFlag(IEnumerable<Flag> flags)
        {
            foreach(var flag in flags)
            {
                AddCommandlineFlag(flag);
            }
        }

        public void AddCommandlineFlag(Flag flag)
        {
            CommandlineFlags.Add(CommandlineConvention + flag.Name, flag);
        }
    }
}