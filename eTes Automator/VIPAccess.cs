using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using FlaUI.Core.Input;
using FlaUI.Core.Tools;
using FlaUI.UIA3;



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

        public static void GetVIPProcessID()
        {
            //Check to see if VIP is already running.  Looks for VIPUIManager as a running process and either copies the process ID into a variable and attaches it to FlaUI so the button can
            //be found.  Otherwise it uses the FlaUI library to start the application.
            int VIPID = 0;
            Process[] pname = Process.GetProcessesByName("VIPUIManager");
            if (pname.Length != 0)
            {
                VIPID = pname[0].Id;
                var app = FlaUI.Core.Application.Attach(VIPID);
                //System.Windows.MessageBox.Show("Found already running VIP");
                Retry.WhileException(() =>
                {
                    using (var automation = new UIA3Automation())
                    {
                        var window = app.GetMainWindow(automation);
                        window.Focus();
                        //System.Windows.MessageBox.Show(window.Title);

                        var copybtn = window.FindFirstDescendant(cf => cf.ByName("Copy Security code"));
                        copybtn.AsButton().Click();
                    }
                    app.Close();
                }, TimeSpan.FromSeconds(30), retryInterval: null);
            }
            else
            {
                var app = FlaUI.Core.Application.Launch(@"C:\Program Files (x86)\Symantec\VIP Access Client\VIPUIManager.exe");
                Retry.WhileException(() =>
                {

                    using (var automation = new UIA3Automation())
                    {
                        var window = app.GetMainWindow(automation);
                        //System.Windows.MessageBox.Show(window.Title);
                        window.SetForeground();
                        var copybtn = window.FindFirstDescendant(cf => cf.ByName("Copy Security code"));
                        copybtn.AsButton().Click();
                    }
                    app.Close();
                }, TimeSpan.FromSeconds(30), retryInterval: null);
            }

            //int VIPID = 0;
            //Process[] pname = Process.GetProcessesByName("VIPUIManager");
            //if (pname.Length != 0)
            //{
            //    VIPID = pname[0].Id;
            //    System.Windows.MessageBox.Show("Found already running VIP");
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("Did not find VIP Access...trying to open it now");
            //    VIPAccess myProcess = new VIPAccess();
            //    myProcess.OpenVIP();
            //    Process[] pname2 = Process.GetProcessesByName("VIPUIManager");
            //    if (pname2.Length != 0)
            //    {
            //        VIPID = pname2[0].Id;
            //        System.Windows.MessageBox.Show("VIP Should be open now.");
            //    }
            //    else
            //    {
            //        System.Windows.MessageBox.Show("VIP Access won't open.");
                    
            //    }
            //}
            //return VIPID;
        }

        //public static string GetProcessName(int processID)
        //{
        //    var process = Process.GetProcessById(processID);
        //    return process.ProcessName;
        //}

        //void OpenVIP()
        //{
        //    Process.Start(@"C:\Program Files (x86)\Symantec\VIP Access Client\VIPUIManager.exe");
        //}
    }
}
