#pragma warning disable 1998
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IME_Firmware_Tool
{
    public sealed class Debugging
    {
        public static bool IsDebugging
        {
            get
            {
                bool VSProcessFound = false;
                foreach (Process exe in Process.GetProcesses())
                {
                    if (exe.ProcessName == Process.GetCurrentProcess().ProcessName && Process.GetCurrentProcess().ProcessName.Contains(".vshost"))
                    {
                        VSProcessFound = true;
                        break;
                    }
                }
                if (!VSProcessFound)
                    return false;
                else
                    return true;
            }
        }
    }
}