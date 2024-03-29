using System.Diagnostics;
using System.Text;
using werkbank.services;

namespace werkbank
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            using (Mutex mutex = new(false, "Global\\d04fff17-6692-41b7-a11e-23cb9f1c340b"))
            {
                if (!mutex.WaitOne(0, false))
                {
                    // if another process of werkbank is already running, find its window,
                    // restore it if it was minimized and bring it to the front
                    foreach (WinApiService.Window window in WinApiService.GetRootWindows().ToList())
                    {
                        if (window.Class != null && window.Class.StartsWith("WindowsForms") && window.Title == FormWerkbank.Title)
                        {
                            WinApiService.ShowWindow(window.hWnd, WinApiService.ShowWindowCommands.Restore);
                            WinApiService.SetForegroundWindow(window.hWnd);
                        }
                    }
                }
                else
                {
                    // To customize application configuration such as set high DPI settings or default font,
                    // see https://aka.ms/applicationconfiguration.
                    ApplicationConfiguration.Initialize();
                    Application.Run(new FormWerkbank());
                }
            }
        }
    }
}