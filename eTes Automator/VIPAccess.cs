using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace eTes_Automator
{
    class VIPAccess
    {
        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindow(string lpClassName, string lpWindowName);
        [DllImport("user32.dll", EntryPoint = "FindWindow", SetLastError = true)]
        static extern IntPtr FindWindowByCaption(IntPtr ZeroOnly, string lpWindowName);

        [DllImport("user32.dll", SetLastError = true)]
        static extern IntPtr FindWindowEx(IntPtr hwndParent, IntPtr hwndChildAfter, string lpszClass, string lpszWindow);

        [DllImport("User32.dll")]
        public static extern int SendMessage(IntPtr hWnd, int uMsg, int wParam, string lParam);

        const int WM_SETTEXT = 0x000c;

        public int VIPID = 0;

        public static void GetVIPAccess()
        {
            var VIPProcess = GetVIPProcessID();
            if (VIPProcess != 0)
            {
                System.Windows.MessageBox.Show(String.Format("Found VIP Process ID running on {0} and process name {1}", VIPProcess.ToString(), GetProcessName(VIPProcess)));

            }
            else
            {
                System.Windows.MessageBox.Show("Did not find VIP Access....wtf.");
            }
        }

        public static int GetVIPProcessID()
        {
            //int VIPID = 0;
            Process[] pname = Process.GetProcessesByName("VIPUIManager");
            if (pname.Length != 0)
            {
                int VIPID = pname[0].Id;
            }
            else
            {
                VIPAccess myProcess = new VIPAccess();
                myProcess.OpenVIP();
                Process[] pname2 = Process.GetProcessesByName("VIPUIManager");
                if (pname2.Length != 0)
                {
                    VIPID = pname2[0].Id;
                }
                else
                {
                    System.Windows.MessageBox.Show("VIP Access won't open.");
                    
                }
            }
            return VIPID;
        }

        public static string GetProcessName(int processID)
        {
            var process = Process.GetProcessById(processID);
            return process.ProcessName;
        }

        void OpenVIP()
        {
            Process.Start(@"C:\Program Files (x86)\Symantec\VIP Access Client\VIPUIManager.exe");
        }

        void CopyCredID()
        {
            
            IntPtr hwndChild = IntPtr.Zero;

            hwndChild = FindWindowEx((IntPtr)VIPID, IntPtr.Zero, "Button", "Copy");
            SendMessage((int)hwndChild, BN_CLICKED, 0, IntPtr.Zero);

        }

        
    }
}
