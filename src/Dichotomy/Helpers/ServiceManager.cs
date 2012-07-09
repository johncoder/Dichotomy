using System;
using System.Configuration.Install;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;

namespace Dichotomy.Helpers
{
    internal static class ServiceManager
    {
        private static bool _initialized;
        private static string _name;

        public static string Name
        {
            get
            {
                EnsureInitialized();
                return _name;
            }
        }

        public static void Initialize(string name)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Service name is required.", "name");
            }

            _name = name;
            _initialized = true;
        }

        private static void EnsureInitialized()
        {
            if (!_initialized)
            {
                _name = new FileInfo(Assembly.GetEntryAssembly().Location).Name.Replace(".exe", string.Empty);
                _initialized = true;
            }
        }

        public static void EnsureStoppedAndUninstall()
        {
            EnsureInitialized();

            if (ServiceIsInstalled() == false)
            {
                Console.WriteLine("Service is not installed");
            }
            else
            {
                var stopController = new ServiceController(_name);

                if (stopController.Status == ServiceControllerStatus.Running)
                    stopController.Stop();

                ManagedInstallerClass.InstallHelper(new[] { "/u", Assembly.GetEntryAssembly().Location });
            }
        }

        public static void StopService()
        {
            EnsureInitialized();

            var stopController = new ServiceController(_name);

            if (stopController.Status == ServiceControllerStatus.Running)
            {
                stopController.Stop();
                stopController.WaitForStatus(ServiceControllerStatus.Stopped);
            }
        }


        public static void StartService()
        {
            EnsureInitialized();

            var stopController = new ServiceController(_name);

            if (stopController.Status != ServiceControllerStatus.Running)
            {
                stopController.Start();
                stopController.WaitForStatus(ServiceControllerStatus.Running);
            }
        }

        public static void RestartService()
        {
            EnsureInitialized();

            var stopController = new ServiceController(_name);

            if (stopController.Status == ServiceControllerStatus.Running)
            {
                stopController.Stop();
                stopController.WaitForStatus(ServiceControllerStatus.Stopped);
            }
            if (stopController.Status != ServiceControllerStatus.Running)
            {
                stopController.Start();
                stopController.WaitForStatus(ServiceControllerStatus.Running);
            }

        }

        public static void InstallAndStart()
        {
            EnsureInitialized();

            if (ServiceIsInstalled())
            {
                Console.WriteLine("Service is already installed");
            }
            else
            {
                ManagedInstallerClass.InstallHelper(new[] { Assembly.GetEntryAssembly().Location });
                SetRecoveryOptions(_name);
                var startController = new ServiceController(_name);
                startController.Start();
            }
        }

        public static bool ServiceIsInstalled()
        {
            EnsureInitialized();

            return (ServiceController.GetServices().Count(s => s.ServiceName == _name) > 0);
        }

        public static void SetRecoveryOptions(string serviceName)
        {
            int exitCode;
            var arguments = string.Format("failure {0} reset= 500 actions= restart/60000", serviceName);
            using (var process = new Process())
            {
                var startInfo = process.StartInfo;
                startInfo.FileName = "sc";
                startInfo.WindowStyle = ProcessWindowStyle.Hidden;

                // tell Windows that the service should restart if it fails
                startInfo.Arguments = arguments;

                process.Start();
                process.WaitForExit();

                exitCode = process.ExitCode;

                process.Close();
            }

            if (exitCode != 0)
                throw new InvalidOperationException(
                    "Failed to set the service recovery policy. Command: " + Environment.NewLine + "sc " + arguments + Environment.NewLine + "Exit code: " + exitCode);
        }

    }
}