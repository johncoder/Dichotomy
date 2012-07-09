using System.ServiceProcess;

namespace Dichotomy
{
    public class GenericService : ServiceBase, IDichotomyService
    {
        private readonly IDichotomyService _service;

        public GenericService(IDichotomyService service)
        {
            _service = service;
        }

        protected override void OnStart(string[] args)
        {
            Start();
        }

        public virtual void Start()
        {
            _service.Start();
        }

        protected override void OnStop()
        {
            _service.Stop();
        }

        protected override void Dispose(bool disposing)
        {
            if (_service != null)
            {
                _service.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}