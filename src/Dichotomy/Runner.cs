using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using Dichotomy.Configuration;
using Dichotomy.Flags;
using Dichotomy.Helpers;

namespace Dichotomy
{
    public class Runner
    {
        private readonly IDichotomyService _serviceBase;
        private readonly ConfigurationOptions _config;

        public Runner(IDichotomyService service) : this(service, new ConfigurationOptions()) { }

        public Runner(IDichotomyService service, ConfigurationOptions config)
        {
            _serviceBase = service;
            _config = config;
        }

        public void Setup(Action<IDictionary<string, Flag>> addCommandLineSupport)
        {
            addCommandLineSupport(_config.CommandlineFlags);
        }

        public void Run()
        {
            Run(Environment.GetCommandLineArgs().Skip(1).ToArray());
        }

        public void Run(string[] args)
        {
            if (Environment.UserInteractive)
            {
                try
                {
                    InteractiveRun(args);
                }
                catch (ReflectionTypeLoadException e)
                {
                    Console.WriteLine("A critical error occurred while starting the server. Please see the exception details bellow for more details:");

                    Console.WriteLine(e);
                    foreach (var loaderException in e.LoaderExceptions)
                    {
                        Console.WriteLine("- - - -");
                        Console.WriteLine(loaderException);
                    }

                    Elevation.WaitForUserInputAndExitWithError();
                }
                catch (Exception e)
                {
                    Console.WriteLine("A critical error occurred while starting the server. Please see the exception details bellow for more details:");

                    Console.WriteLine(e);

                    Elevation.WaitForUserInputAndExitWithError();
                }
            }
            else
            {
                try
                {
                    var serviceBase = _serviceBase as ServiceBase ?? new GenericService(_serviceBase);

                    ServiceBase.Run(serviceBase);
                }
                // no catch here, we want the exception to be logged by Windows
                finally
                {
                    var serviceBase = _serviceBase as IDisposable;

                    if (serviceBase != null)
                    {
                        serviceBase.Dispose();
                    }
                }
            }
        }

        private void ExecuteStartupTasks()
        {
            foreach(var task in _config.StartupTasks)
            {
                task.Start();
            }
        }

        private void InteractiveRun(string[] args)
        {
            if (args.Length > 0 && _config.CommandlineFlags.ContainsKey(args[0]))
            {
                _config.CommandlineFlags[args[0]].Action();
                return;
            }

            ExecuteStartupTasks();

            using (_serviceBase)
            {
                _serviceBase.Start();

                Console.WriteLine("Press any key to stop...");
                Console.ReadKey(true);

                _serviceBase.Stop();
            }
        }
    }
}
