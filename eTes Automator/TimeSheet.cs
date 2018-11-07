using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace eTes_Automator
{
    class TimeSheet
    {
        public async static void WorkOrderCheck()
        {
            //If they are using Firefox, it needs more time to find the element.  So wait 5 secs and then verify that the element list is populated.If it is, check that
            //REGULAR HOURS is in the field, if its not, then its a new work week and new order needs creating.
            if (MainWindow.AppWindow.comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Firefox")
            {
                await Task.Delay(5000);
                IList<IWebElement> ffelement = Browser.browser.FindElements(By.XPath("html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a")).ToList();
                if (ffelement.Count > 0 && Browser.FindByXpathTitle("/html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a", "title") != "REGULAR HOURS")
                {
                    System.Windows.MessageBox.Show("New Week has started, populating Work Order for you now");
                    string workorder = Browser.browser.FindElement(By.Id("QE")).GetAttribute("value");
                    if (workorder.Contains("REGULAR HOURS"))
                    {
                        Browser.IDClick("addfromqe");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Can't create the workorder as it does not follow the standard 1010 REGULAR HOURS expected. Going to close the browser, you will need to create a WORKORDER in the timesheet manually.  Or make sure that the REGULAR HOURS option is the default in the drop-down at page start.");
                        return;
                    }
                }
                else if (ffelement.Count == 0)
                {
                    System.Windows.MessageBox.Show("There is a problem with the browser finding the element, please try another browser such as Chrome.");
                    Browser.Close();
                    MainWindow.AppWindow.btn_start.Content = "Start";
                    return;
                }
            }
            else
            {
                if (Browser.FindByXpathTitle("/html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a", "title") != "REGULAR HOURS")
                {
                    System.Windows.MessageBox.Show("New Week has started, populating Work Order for you now");
                    string workorder = Browser.browser.FindElement(By.Id("QE")).GetAttribute("value");
                    if (workorder.Contains("REGULAR HOURS"))
                    {
                        Browser.IDClick("addfromqe");
                    }
                    else
                    {
                        System.Windows.MessageBox.Show("Can't create the workorder as it does not follow the standard 1010 REGULAR HOURS expected. Going to close the browser, you will need to create a WORKORDER in the timesheet manually.  Or make sure that the REGULAR HOURS option is the default in the drop-down at page start.");
                        return;
                    }
                }

            }
        }

    }
}
