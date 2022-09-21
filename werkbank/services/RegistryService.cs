using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Win32;

namespace werkbank.services
{
    public static class RegistryService
    {
        /// <summary>
        /// Update the applications registry value for autostart.
        /// </summary>
        /// <param name="AutoStart"></param>
        /// <exception cref="Exception"></exception>
        public static void UpdateApplicationAutoStart(bool AutoStart)
        {
            RegistryKey? key = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);

            if (key == null)
            {
                throw new Exception("Could not open startup registry key");
            }

            if (AutoStart && key.GetValue(Application.ProductName) == null)
            {
                key.SetValue(Application.ProductName, Application.ExecutablePath);
            }

            if (!AutoStart && key.GetValue(Application.ProductName) != null)
            {
                key.DeleteValue(Application.ProductName, false);
            }
        }
    }
}
