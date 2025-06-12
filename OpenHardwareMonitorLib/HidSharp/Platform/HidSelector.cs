using System.Threading;

namespace HidSharp.Platform
{
    sealed class HidSelector
    {
        public static readonly HidManager Instance;
        static readonly Thread ManagerThread;

        static HidSelector()
        {
            foreach (var hidManager in new HidManager[]
                {
                    new Windows.WinHidManager(),
                    new Linux.LinuxHidManager(),
                    new MacOS.MacHidManager(),
                    new Unsupported.UnsupportedHidManager()
                })
            {
                if (hidManager.IsSupported)
                {
                    var readyEvent = new ManualResetEvent(false);

                    Instance = hidManager;
                    Instance.InitializeEventManager();
                    ManagerThread = new Thread(Instance.RunImpl) { IsBackground = true, Name = "HID Manager" };
                    ManagerThread.Start(readyEvent);
                    readyEvent.WaitOne();
                    break;
                }
            }
        }
    }
}
