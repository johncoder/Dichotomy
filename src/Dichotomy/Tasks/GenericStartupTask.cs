using System;

namespace Dichotomy.Tasks
{
    internal class GenericStartupTask : IStartupTask
    {
        private readonly Action _task;

        public GenericStartupTask(Action task)
        {
            _task = task;
        }

        public void Start()
        {
            _task();
        }
    }
}