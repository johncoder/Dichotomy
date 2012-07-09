using System;

namespace Dichotomy
{
    public interface IDichotomyService : IDisposable
    {
        void Start();
        void Stop();
    }
}