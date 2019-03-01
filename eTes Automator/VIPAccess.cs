using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTes_Automator
{
    class VIPAccess
    {
        public static void StartVIP()
        {
            ProcessStartInfo vip = new ProcessStartInfo();
            vip.FileName = @"C:\Program Files (x86)\Symantec\VIP Access Client\VIPUIManager.exe";
        }
    }
}
