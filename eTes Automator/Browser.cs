﻿using System;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;
using OpenQA.Selenium.Support.UI;

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

        public static void StartBrowserChromeHeadless(string Url) //This is broken atm, gpu issue, no fix in sight atm
        {
            var browserOptions = new ChromeOptions();
            browserOptions.AddArgument("headless");
            browserOptions.AddArgument("disable-gpu");
            using (browser = new ChromeDriver(browserOptions))
            {
                browser.Navigate().GoToUrl(Url);
            }
        }

        public static void StartBrowserFirefox(string Url)
        {
            browser = new FirefoxDriver();
            browser.Navigate().GoToUrl(Url);
        }

        public static void WaitforBrowser(string title)
        {
            try
            {
                WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromSeconds(60));
                wait.Until((d) => { return d.Title == title; });
            }
            catch (Exception)
            {
                throw;
            }
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
            WebDriverWait wait = new WebDriverWait(browser, TimeSpan.FromSeconds(60));
            wait.Until((d) => { return d.FindElement(By.Name(Name)); });
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

        public static void FindByTimeSheetClick()
        {
            browser.FindElement(By.XPath("//img[@alt='Time Sheet']")).Click();
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
