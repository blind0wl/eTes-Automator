using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Firefox;


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

        public static void Close()
        {
            if (browser != null)
            {
                browser.Quit();
                browser = null;
            }
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
        
    }
}
