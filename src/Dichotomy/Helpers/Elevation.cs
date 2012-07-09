using System;
using System.Diagnostics;
using System.Reflection;
using System.Security.Principal;

namespace Dichotomy.Helpers
{
    public static class Elevation
    {
        internal static bool RunAgainAsAdmin(this Assembly assembly, string[] cmdLine)
        {
            return RunAgainAsAdmin(assembly, string.Join(" ", cmdLine));
        }

        internal static bool RunAgainAsAdmin(Assembly assembly, string cmdLine)
        {
            try
            {
                var process = Process.Start(new ProcessStartInfo
                {
                    Arguments = cmdLine,
                    FileName = assembly.Location,
                    Verb = "runas"
                });
                process.WaitForExit();
                Console.WriteLine(process.StandardOutput.ReadToEnd());
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static bool RunningAsAdmin
        {
            get
            {
                var p = new WindowsPrincipal(WindowsIdentity.GetCurrent());
                return p.IsInRole(WindowsBuiltInRole.Administrator);
            }
        }

        public static void AdminRequired(Action actionThatMayRequireAdminPrivileges, string cmdLine)
        {
            if (RunningAsAdmin)
            {
                actionThatMayRequireAdminPrivileges();
                return;
            }
            else
            {
                if (RunAgainAsAdmin(Assembly.GetEntryAssembly(), cmdLine))
                    return;
            }
        }

        internal static void WaitForUserInputAndExitWithError()
        {
            Console.WriteLine("Stopping program execution");
            Console.WriteLine("Press any key to continue...");
            Console.ReadKey(true);
            Environment.Exit(-1);
        }

    }
}