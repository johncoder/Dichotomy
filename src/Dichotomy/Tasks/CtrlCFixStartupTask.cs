using System;
using System.Runtime.InteropServices;

namespace Dichotomy.Tasks
{
    internal class CtrlCFixStartupTask : IStartupTask
    {
        private static Action _onCheck;

        [DllImport("Kernel32")]
        internal static extern bool SetConsoleCtrlHandler(HandlerRoutine handler, bool add);
        public delegate bool HandlerRoutine(CtrlTypes ctrlType);
        public enum CtrlTypes
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT,
            CTRL_CLOSE_EVENT,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT
        }

        private static bool ConsoleCtrlCheck(CtrlTypes ctrlType)
        {
            if (_onCheck != null)
            {
                _onCheck();
            }

            return false;
        }

        public static void SetOnCheck(Action onCheck)
        {
            _onCheck = onCheck;
        }

        public void Start()
        {
            if (!Environment.UserInteractive)
                return;

            var hr = new HandlerRoutine(ConsoleCtrlCheck);
            // keep the handler alive because the garbage collector will destroy it after any CTRL event
            GC.KeepAlive(hr);
            SetConsoleCtrlHandler(hr, true);
        }
    }
}