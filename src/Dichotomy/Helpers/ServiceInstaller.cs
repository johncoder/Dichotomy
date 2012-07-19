using System;
using System.Configuration.Install;
using System.ServiceProcess;

namespace Dichotomy.Helpers
{
    public abstract class CustomInstaller : Installer
    {
        private readonly ServiceProcessInstaller _serviceProcessInstaller;
        private readonly ServiceInstaller _serviceInstaller;

        protected CustomInstaller()
        {
            _serviceProcessInstaller = new ServiceProcessInstaller();

            _serviceInstaller = new ServiceInstaller
                {
                    DisplayName = ServiceManager.DisplayName,
                    ServiceName = ServiceManager.Name,
                    Description = ServiceManager.Description
                };

            _serviceProcessInstaller.Password = null;
            _serviceProcessInstaller.Username = null;

            Installers.AddRange(new Installer[]
                {
                    _serviceProcessInstaller,
                    _serviceInstaller
                });

            _serviceInstaller.StartType = ServiceStartMode.Automatic;
            _serviceProcessInstaller.Account = ServiceAccount.LocalSystem;
        }
    }
}