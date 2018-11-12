using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace eTes_Automator
{
    class TimeSheet
    {
        public async static void WorkOrderCheck()
        {
            //If they are using Firefox, it needs more time to find the element.  So wait 5 secs and then verify that the element list is populated.If it is, check that
            //REGULAR HOURS is in the field, if its not, then its a new work week and new order needs creating.
            if (MainWindow.AppWindow.browserchoice.ToString() == "System.Windows.Controls.ComboBoxItem: Firefox")
            //if (MainWindow.AppWindow.comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Firefox")
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
                IList<IWebElement> ffelement = Browser.browser.FindElements(By.XPath("html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a")).ToList();
                if (ffelement.Count == 0)
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
                else if (Browser.FindByXpathTitle("/html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a", "title") != "REGULAR HOURS")
                {
                    System.Windows.MessageBox.Show("Can't create the workorder as it does not follow the standard 1010 REGULAR HOURS expected. Going to close the browser, you will need to create a WORKORDER in the timesheet manually.  Or make sure that the REGULAR HOURS option is the default in the drop-down at page start.");
                }

            }
        }

        public async static Task StartTimesheet()
        {
            DateTime currentDateTime = DateTime.Now;
            int dayoftheweek = (int)currentDateTime.DayOfWeek;
            if (dayoftheweek >= 1 && dayoftheweek <= 5) //&& currentDateTime.Hour < 17)
            {
                var MyIni = new IniFile("Settings.ini");
               
                string etes = "https://etes.csc.com";
                //if (MainWindow.AppWindow.comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Chrome")
                if (MainWindow.AppWindow.browserchoice == "System.Windows.Controls.ComboBoxItem: Chrome")
                {
                    Browser.StartBrowserChrome(etes);
                    //Enter Username and Password fields of the VIP Access page - using existing saved username and pass
                    Browser.WaitforBrowser("Sign In");
                    //await Task.Delay(5000);                
                    Browser.FindNameSendKeys("UserName", MainWindow.AppWindow.decusr);
                    Browser.FindNameSendKeys("Password", MainWindow.AppWindow.decpass);
                    Browser.IDClick("submitButton");
                    Browser.WaitforBrowser("Internet Time Entry System");
                }
                else if (MainWindow.AppWindow.browserchoice == "System.Windows.Controls.ComboBoxItem: Firefox")
                {
                    Browser.StartBrowserFirefox(etes);
                    //Enter Username and Password fields of the VIP Access page - using existing saved username and pass                    
                    await Task.Delay(5000); //Firefox can't deal with Seleniums explicit wait, so we need to async wait until the page loads.                
                    Browser.FindNameSendKeys("UserName", MainWindow.AppWindow.decusr);
                    Browser.FindNameSendKeys("Password", MainWindow.AppWindow.decpass);
                    Browser.IDClick("submitButton");

                    //Write the value to the Settings file in case they have changed it without hitting apply.
                    //Read the waittime value in the Settings.ini so the function waits for the customised time.
                    MyIni.Write("Wait Time", MainWindow.AppWindow.textWaittime.Text, "Wait Time");
                    int waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
                    //Wait the specified amount the user has entered
                    await Task.Delay(waittime * 1000);
                }
                else
                {
                    System.Windows.MessageBox.Show("Something went wrong with your choice, make sure you have the browser you've chosen installed.");
                }

                //Find the MenuBar frame and search and click the TimeSheet button
                Browser.SwitchFrame("/html/frameset/frame[1]");
                Browser.FindByXPathClick("/html/body/table/tbody/tr[4]/td/a[5]");
                Browser.DefaultFrame();
                //Switch Frame to the time entries
                Browser.SwitchFrame("/html/frameset/frame[2]");

                //Call WorkOrderCheck method - this will see if a new week is required and do some error checking on the timesheet before filling it out
                WorkOrderCheck();

                List<string> dayArray = new List<string>();
                List<string> hoursArray = new List<string>();
                List<IWebElement> buttonArray = new List<IWebElement>();
                await Task.Delay(3000); //Firefox erroring because it cant find the buttons...sigh
                //Add the web form values into the dayArray list so we can compare what is in the form and what is in the settings.ini file.
                for (int i = 0; i < 7; i++)
                {
                    buttonArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")));
                    dayArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")).GetAttribute("value"));
                }
                //Read the settings.ini hours into the hoursArray list
                //TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                for (int i = 0; i < MainWindow.AppWindow.settingday.Length; i++)
                {
                    hoursArray.Add(MyIni.Read(MainWindow.AppWindow.settingday[i], "Week"));
                }

                bool hoursmatch = true;
                //Compare the hoursArray and dayArray Lists to see if they are the same...we might not need to do anything!
                if (hoursmatch == !dayArray.Except(hoursArray).Any())
                {
                    Notification.Bubble("The Settings.ini file and whats on the etes page matches");
                    //Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "The Settings.ini file and whats on the etes page matches", ToolTipIcon.Info);
                    //System.Windows.MessageBox.Show("The Settings.ini file and whats on the etes page matches");
                }
                else
                {
                    Notification.Bubble("The Settings.ini file and the etes website hours do not match...going to try and populate now.");
                    //Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "The Settings.ini file and the etes website hours do not match...going to try and populate now.", ToolTipIcon.Info);
                    //System.Windows.MessageBox.Show("The Settings.ini file and the etes website hours do not match...going to try and populate now.");
                    //Dayoftheweek is based on C# DayOfWeek, Sunday = 0, Monday = 1 etc, so to check Monday it must go through Sat, Sun and then Monday (or two extra days)
                    for (int i = 0; i <= dayoftheweek + 1; i++)
                    {
                        //Clear the contents of the web field and then input the settings.ini value that was saved in the main form.
                        buttonArray[i].Clear();
                        buttonArray[i].SendKeys(hoursArray[i]);

                    }
                    if (dayoftheweek == 5 && MainWindow.AppWindow.fridaycheck.ToString() == "True")
                    {
                        Notification.Bubble("Please review your timesheet.  If you need to add further details, now is the time to do so. Please submit manually if you need to edit any entries on the timesheet.");
                        //Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "Please review your timesheet.  If you need to add further details, now is the time to do so. Please submit manually if you need to edit any entries on the timesheet.", ToolTipIcon.Info);
                        //System.Windows.MessageBox.Show("Please review your timesheet.  If you need to add further details, now is the time to do so. Please submit manually if you need to edit any entries on the timesheet.");
                        return;
                    }
                    else
                    {
                        System.Windows.MessageBoxResult saveYesNo = System.Windows.MessageBox.Show("Would you like to Save the timesheet?", "Save Timesheet", System.Windows.MessageBoxButton.YesNo);
                        if (saveYesNo == System.Windows.MessageBoxResult.Yes)
                        {
                            Browser.DefaultFrame();
                            Browser.SwitchFrame("/html/frameset/frame[3]");
                            Browser.FindByXPathClick("/html/body/table/tbody/tr/td/form/a[1]");     //Click Save
                            Browser.browser.SwitchTo().Alert().Accept();
                        }
                        else
                        {
                            //System.Windows.MessageBox.Show("You have said NO...noone says no to me!!!!");
                            return;
                        }
                    }
                }
            }
            
            else
            {
                System.Windows.MessageBox.Show("It's not during the work week, sorry, we can't edit unless its Mon-Fri (before 5pm).", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            }
        }

    }
}
