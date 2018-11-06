using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;
using System.Windows.Forms;
using System.Drawing;
using System.Windows;

namespace eTes_Automator
{
    class Browser
    {
        public static IWebDriver browser;

        public static void StartBrowserChrome(string Url)
        {
            browser = new ChromeDriver();
            browser.Navigate().GoToUrl(Url);
        }

        public static void StartBrowserFirefox(string Url)
        {
            browser = new FirefoxDriver();
            browser.Navigate().GoToUrl(Url);
        }

        public static void WaitforBrowser(string title)
        {
            WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromSeconds(60));
            wait.Until<IWebElement>(d => d.FindElement(By.XPath("/html/frameset/frame[1]")));
            wait.Until((d) => { return d.Title == title; });
        }

        public static void Close()
        {
            if (browser != null)
            {
                browser.Quit();
                browser = null;
            }
        }

        public static string FindByXpathTitle(string path, string title)
        {
            return browser.FindElement(By.XPath(path)).GetAttribute(title);
        }

        public static void FindByName(string Name)
        {
            browser.FindElements(By.Name(Name));
        }

        public static void FindNameSendKeys(string Name, string Keys)
        {
            browser.FindElement(By.Name(Name)).SendKeys(Keys);

        }

        public static void FindNameClear(string Name)
        {
            browser.FindElement(By.Name(Name)).Clear();
        }

        public static void IDClick(string ID)
        {
            browser.FindElement(By.Id(ID)).Click();
        }

        public static void FindByXPathClick(string Xpath)
        {
            browser.FindElement(By.XPath(Xpath)).Click();
        }

        public static void SwitchFrame(string framename)
        {
            browser.SwitchTo().Frame(browser.FindElement(By.XPath(framename)));
        }

        public static void DefaultFrame()
        {
            browser.SwitchTo().DefaultContent();
        }

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
        
        //public static void CreateNotifyIcon()
        //{   
        //    NotifyIcon nIcon = new NotifyIcon();
        //    MainWindow.AppWindow.ShowInTaskbar = true;
        //    //Visibility = Visibility.Hidden;
        //    MainWindow.AppWindow.WindowState = System.Windows.WindowState.Normal;
        //    nIcon.Icon = new Icon(@"../../images/clock.ico");
        //    //nIcon.Icon = new Icon(@"images/clock.ico");
        //    nIcon.Visible = true;
        //    nIcon.Text = "eTes Automator";
        //    nIcon.DoubleClick +=
        //        delegate (object sender, EventArgs args)
        //        {
        //            MainWindow.AppWindow.Show();
        //            MainWindow.AppWindow.WindowState = WindowState.Normal;
        //        };
        //    System.Windows.Forms.ContextMenu notifyIconContextMenu = new System.Windows.Forms.ContextMenu();
        //    notifyIconContextMenu.MenuItems.Add("Quit", new EventHandler(MainWindow.AppWindow.Quit));
        //    nIcon.ContextMenu = notifyIconContextMenu;
        //}

    }
}
