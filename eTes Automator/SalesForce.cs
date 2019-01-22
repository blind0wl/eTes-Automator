using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace eTes_Automator
{
    class SalesForce
    {
        public async static Task Automate()
        {
            try
            {
                if (MainWindow.AppWindow.CheckSFHeadless.IsChecked ?? false)
                {
                    string http = "https://gpl.amer.csc.com/siteminderagent/forms/login.dxc.h.fcc?TYPE=33554433&REALMOID=06-0b8d0989-bf34-120b-9c1f-85f4febe0000&GUID=1&SMAUTHREASON=0&METHOD=GET&SMAGENTNAME=amer_gp_cscgppndc001_apache_agent&TARGET=-SM-HTTPS%3a%2f%2fgpl%2eamer%2ecsc%2ecom%2fsiteminderagent%2fredirectjsp%2fredirectToHPES%2ejsp%3fSPID%3dhttps-%3A-%2F-%2Fenterpriseservices%2emy%2esalesforce%2ecom%26TargetResource%3d-%2Fhome-%2Fhome%2ejsp%26SMPORTALURL%3dhttps-%3A-%2F-%2Fgpl%2eamer%2ecsc%2ecom-%2Faffwebservices-%2Fpublic-%2Fsaml2sso%26SAMLTRANSACTIONID%3d2d3da0eb--7e981915--74039841--ef27253d--f7429581--b3";
                    Browser.StartBrowserChromeHeadless(http);
                    Browser.WaitforBrowser("DXC Global Pass - Login");
                    //Use Mailaddress to split the decusr into the user only
                    MailAddress addr = new MailAddress(MainWindow.AppWindow.decusr);
                    var username = addr.User;
                    Browser.FindNameSendKeys("USER", username);
                    Browser.FindNameSendKeys("PASSWORD", MainWindow.AppWindow.decpass);
                    Browser.IDClick("loginbtn");
                    await Task.Delay(5000);
                    Browser.Close();
                }
                else if (!MainWindow.AppWindow.CheckSFHeadless.IsChecked ?? false)
                {
                    string http = "https://gpl.amer.csc.com/siteminderagent/forms/login.dxc.h.fcc?TYPE=33554433&REALMOID=06-0b8d0989-bf34-120b-9c1f-85f4febe0000&GUID=1&SMAUTHREASON=0&METHOD=GET&SMAGENTNAME=amer_gp_cscgppndc001_apache_agent&TARGET=-SM-HTTPS%3a%2f%2fgpl%2eamer%2ecsc%2ecom%2fsiteminderagent%2fredirectjsp%2fredirectToHPES%2ejsp%3fSPID%3dhttps-%3A-%2F-%2Fenterpriseservices%2emy%2esalesforce%2ecom%26TargetResource%3d-%2Fhome-%2Fhome%2ejsp%26SMPORTALURL%3dhttps-%3A-%2F-%2Fgpl%2eamer%2ecsc%2ecom-%2Faffwebservices-%2Fpublic-%2Fsaml2sso%26SAMLTRANSACTIONID%3d2d3da0eb--7e981915--74039841--ef27253d--f7429581--b3";
                    Browser.StartBrowserChrome(http);
                    Browser.WaitforBrowser("DXC Global Pass - Login");
                    //Use Mailaddress to split the decusr into the user only
                    MailAddress addr = new MailAddress(MainWindow.AppWindow.decusr);
                    var username = addr.User;
                    Browser.FindNameSendKeys("USER", username);
                    Browser.FindNameSendKeys("PASSWORD", MainWindow.AppWindow.decpass);
                    Browser.IDClick("loginbtn");
                    await Task.Delay(5000);
                    Browser.Close();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
