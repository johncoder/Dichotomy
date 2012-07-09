using Dichotomy.Helpers;

namespace Dichotomy.Flags
{
    public class InstallFlag : Flag
    {
        public InstallFlag()
        {
            Name = "install";
            Description = "Installs as a Windows Service and starts immediately.";
            Action = () => Elevation.AdminRequired(ServiceManager.InstallAndStart, string.Concat(Convention, Name));
        }
    }

    public class UninstallFlag : Flag
    {
        public UninstallFlag()
        {
            Name = "uninstall";
            Description = "Stops the Windows Service, and uninstalls it.";
            Action = () => Elevation.AdminRequired(ServiceManager.EnsureStoppedAndUninstall, string.Concat(Convention, Name));
        }
    }

    public class StartFlag : Flag
    {
        public StartFlag()
        {
            Name = "start";
            Description = "Starts the Windows Service.";
            Action = () => Elevation.AdminRequired(ServiceManager.StartService, string.Concat(Convention, Name));
        }
    }

    public class StopFlag : Flag
    {
        public StopFlag()
        {
            Name = "stop";
            Description = "Stops the Windows Service.";
            Action = () => Elevation.AdminRequired(ServiceManager.StopService, string.Concat(Convention, Name));
        }
    }

    public class RestartFlag : Flag
    {
        public RestartFlag()
        {
            Name = "restart";
            Description = "Restarts the Windows Service.";
            Action = () => Elevation.AdminRequired(ServiceManager.RestartService, string.Concat(Convention, Name));
        }
    }
}