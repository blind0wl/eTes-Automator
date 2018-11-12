﻿//Version 0.1 - Dave Payne 2018
//eTes Automator for Timesheet automated completion

using System;
using System.ComponentModel;
using System.Windows;
using AesEncDec;
using System.IO;
using System.Windows.Forms;
using System.Threading;

namespace eTes_Automator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //readonly NotifyIcon nIcon = new NotifyIcon();

        public string decusr = string.Empty;
        public string decpass = string.Empty;
        public string[] settingday = new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };
        public static MainWindow AppWindow;
        public string browserchoice = string.Empty;
        public string fridaycheck = string.Empty;


        public MainWindow()
        {

            InitializeComponent();
            AppWindow = this;
            Notification.CreateNotify();
            Scheduler sc = new Scheduler();
            sc.Start();

            ////Hide main window when the program begins
            //ShowInTaskbar = true;
            ////Visibility = Visibility.Hidden;
            //WindowState = System.Windows.WindowState.Normal;
            //nIcon.Icon = new Icon(@"../../images/clock.ico");
            ////nIcon.Icon = new Icon(@"images/clock.ico");
            //nIcon.Visible = true;
            //nIcon.Text = "eTes Automator";
            //nIcon.DoubleClick +=
            //    delegate (object sender, EventArgs args)
            //    {
            //        this.Show();
            //        this.WindowState = WindowState.Normal;
            //    };
            //System.Windows.Forms.ContextMenu notifyIconContextMenu = new System.Windows.Forms.ContextMenu();
            //notifyIconContextMenu.MenuItems.Add("Quit", new EventHandler(Quit));
            //nIcon.ContextMenu = notifyIconContextMenu;

            if (File.Exists("data\\data.ls"))
            {
                var sr = new StreamReader("data\\data.ls");

                string encusr = sr.ReadLine();
                string encpass = sr.ReadLine();
                sr.Close();

                decusr = AesCrypt.Decrypt(encusr);
                decpass = AesCrypt.Decrypt(encpass);

                textUsername.Text = decusr;
                textPassword.Text = decpass;
                passwordBox.Password = decpass;

            }
            else
            {
                System.Windows.MessageBox.Show("Ensure you fill out the login and password details and Apply.");
            }

            if (!File.Exists("Settings.ini"))
            {
                //Create new Settings.ini file
                var MyIni = new IniFile("Settings.ini");

                //Set default values for days of the week
                MyIni.Write("Saturday", "", "Week");
                MyIni.Write("Sunday", "", "Week");
                MyIni.Write("Monday", "7.50", "Week");
                MyIni.Write("Tuesday", "7.50", "Week");
                MyIni.Write("Wednesday", "7.50", "Week");
                MyIni.Write("Thursday", "7.50", "Week");
                MyIni.Write("Friday", "7.50", "Week");
                MyIni.Write("WorkOrder", "1010 REGULAR HOURS", "WorkOrder");
                MyIni.Write("Wait Time", "15", "Wait Time");
                MyIni.Write("Browser", "System.Windows.Controls.ComboBoxItem: Chrome", "Browser Choice");
                MyIni.Write("ManualSubmit", "False", "Submit");
                MyIni.Write("Reminded", "False", "Reminders"); //sets notifyicon to ignore or send the info message about closing from the notification area.


                //Set variables with Settings.ini values
                //string[] settingday = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };             
                //TextBox[] textboxes = { textMon, textTue, textWed, textThur, textFri, textSat, textSun };
                System.Windows.Controls.TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                for (int i = 0; i < settingday.Length; i++)
                {
                    textboxes[i].Text = MyIni.Read(settingday[i], "Week");
                }
                textWaittime.Text = (MyIni.Read("Wait Time", "Wait Time"));
                var browserchoice = MyIni.Read("Browser", "Browser Choice");
                if (browserchoice == "System.Windows.Controls.ComboBoxItem: Chrome")
                {
                    comboBrowser.SelectedIndex = 0;
                }
                else
                {
                    comboBrowser.SelectedIndex = 1;
                }

                fridaycheck = MyIni.Read("ManualSubmit", "Submit");
                if (fridaycheck == "True")
                {
                    fridayCheckBox.IsChecked = true;
                }
                else
                {
                    fridayCheckBox.IsChecked = false;
                }
                //Check if we need a wait time box, Firefox needs it, Chrome doesnt
                if (comboBrowser.SelectedIndex == 1)
                {
                    textBWait.Visibility = Visibility.Visible;
                    textWaittime.Visibility = Visibility.Visible;
                    textBlock8.Visibility = Visibility.Visible;
                    textWaittime.Text = (MyIni.Read("Wait Time", "Wait Time"));

                }
                else
                {
                    textBWait.Visibility = Visibility.Hidden;
                    textWaittime.Visibility = Visibility.Hidden;
                    textBlock8.Visibility = Visibility.Hidden;
                }
            }
            else
            {
                //Read Settings.ini
                var MyIni = new IniFile("Settings.ini");

                //string[] settingday = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                //TextBox[] textboxes = { textMon, textTue, textWed, textThur, textFri, textSat, textSun };
                System.Windows.Controls.TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                for (int i = 0; i < settingday.Length; i++)
                {
                    textboxes[i].Text = MyIni.Read(settingday[i], "Week");
                }

                browserchoice = MyIni.Read("Browser", "Browser Choice");
                if (browserchoice == "System.Windows.Controls.ComboBoxItem: Chrome")
                {
                    comboBrowser.SelectedIndex = 0;
                }
                else
                {
                    comboBrowser.SelectedIndex = 1;
                }

                var fridaycheck = MyIni.Read("ManualSubmit", "Submit");
                if (fridaycheck == "True")
                {
                    fridayCheckBox.IsChecked = true;
                }
                else
                {
                    fridayCheckBox.IsChecked = false;
                }
                //Check if we need a wait time box, Firefox needs it, Chrome doesnt
                if (comboBrowser.SelectedIndex == 1)
                {
                    textBWait.Visibility = Visibility.Visible;
                    textWaittime.Visibility = Visibility.Visible;
                    textBlock8.Visibility = Visibility.Visible;
                    textWaittime.Text = (MyIni.Read("Wait Time", "Wait Time"));

                }
                else
                {
                    textBWait.Visibility = Visibility.Hidden;
                    textWaittime.Visibility = Visibility.Hidden;
                    textBlock8.Visibility = Visibility.Hidden;
                }
            }
            
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == System.Windows.WindowState.Minimized)
                this.Hide();

            base.OnStateChanged(e);
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
            var MyIni = new IniFile("Settings.ini");
            if (MyIni.Read("Reminded", "Reminders") == "False")
            {
                Notification.Bubble("Remember you can quit the application with the right click context menu.");
                //Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "Remember you can quit the application with the right click context menu.", ToolTipIcon.Info);
                MyIni.Write("Reminded", "True", "Reminders");
            }
            base.OnClosing(e);
        }

        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (textUsername.Text.Length < 3 || passwordBox.Password.Length < 5)
            {
                System.Windows.MessageBox.Show("Username or Password is invalid");
            }
            else
            {
                Directory.CreateDirectory("data\\");

                var sw = new StreamWriter("data\\data.ls");

                var encusr = AesCrypt.Encrypt(textUsername.Text);
                var encpass = AesCrypt.Encrypt(passwordBox.Password);

                sw.WriteLine(encusr);
                sw.WriteLine(encpass);
                sw.Close();

                System.Windows.MessageBox.Show(String.Format("User {0} configuration was successfully saved.", textUsername.Text));

                var sr = new StreamReader("data\\data.ls");

                var r_encusr = sr.ReadLine();
                var r_encpass = sr.ReadLine();
                sr.Close();

                decusr = AesCrypt.Decrypt(r_encusr);
                decpass = AesCrypt.Decrypt(r_encpass);

                textUsername.Text = decusr;
                textPassword.Text = decpass;
                passwordBox.Password = decpass;
            }

            var MyIni = new IniFile("Settings.ini");
            System.Windows.Controls.TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
            for (int i = 0; i < settingday.Length; i++)
            {
                MyIni.Write(settingday[i], textboxes[i].Text, "Week");
            }
            if (comboBrowser.SelectedIndex == 1)
            {
                MyIni.Write("Wait Time", textWaittime.Text, "Wait Time");
            }

            MyIni.Write("Browser", comboBrowser.SelectedItem.ToString(), "Browser Choice");
            MyIni.Write("ManualSubmit", fridayCheckBox.IsChecked.ToString(), "Submit");
        }

        private void btnPassVis_Click(object sender, RoutedEventArgs e)
        {
            if (passwordBox.Visibility == Visibility.Visible)
            {
                passwordBox.Visibility = Visibility.Hidden;
                textPassword.Visibility = Visibility.Visible;
            }
            else
            {
                passwordBox.Visibility = Visibility.Visible;
                textPassword.Visibility = Visibility.Hidden;
            }
        }

        private async void btn_start_Click(object sender, RoutedEventArgs e)
        {
            if (MainWindow.AppWindow.btn_start.Content.ToString() == ("Start"))
            {

                Notification.Bubble("Filling out your timesheet");
                MainWindow.AppWindow.btn_start.Content = "Stop";                
                await TimeSheet.StartTimesheet();
            }
            else
            {
                Browser.Close();
                MainWindow.AppWindow.btn_start.Content = "Start";
            }

            
            

            //if (btn_start.Content.ToString() == ("Start"))
            //{

            //    Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "Filling out your timesheet", ToolTipIcon.Info);
            //}

            //DateTime currentDateTime = DateTime.Now;
            //int dayoftheweek = (int)currentDateTime.DayOfWeek;
            //if (btn_start.Content as string == "Start" && dayoftheweek >= 1 && dayoftheweek <= 5) //&& currentDateTime.Hour < 17)
            //{
            //    var MyIni = new IniFile("Settings.ini");
            //    btn_start.Content = "Stop";
            //    string etes = "https://etes.csc.com";
            //    if (comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Chrome")
            //    {
            //        Browser.StartBrowserChrome(etes);
            //        //Enter Username and Password fields of the VIP Access page - using existing saved username and pass
            //        Browser.WaitforBrowser("Sign In");
            //        //await Task.Delay(5000);                
            //        Browser.FindNameSendKeys("UserName", decusr);
            //        Browser.FindNameSendKeys("Password", decpass);
            //        Browser.IDClick("submitButton");
            //        Browser.WaitforBrowser("Internet Time Entry System");
            //    }
            //    else if (comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Firefox")
            //    {
            //        Browser.StartBrowserFirefox(etes);
            //        //Enter Username and Password fields of the VIP Access page - using existing saved username and pass                    
            //        await Task.Delay(5000); //Firefox can't deal with Seleniums explicit wait, so we need to async wait until the page loads.                
            //        Browser.FindNameSendKeys("UserName", decusr);
            //        Browser.FindNameSendKeys("Password", decpass);
            //        Browser.IDClick("submitButton");

            //        //Write the value to the Settings file in case they have changed it without hitting apply.
            //        //Read the waittime value in the Settings.ini so the function waits for the customised time.
            //        MyIni.Write("Wait Time", textWaittime.Text, "Wait Time");
            //        int waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
            //        //Wait the specified amount the user has entered
            //        await Task.Delay(waittime * 1000);
            //    }
            //    else
            //    {
            //        System.Windows.MessageBox.Show("Something went wrong with your choice, make sure you have the browser you've chosen installed.");
            //    }

            //    //Find the MenuBar frame and search and click the TimeSheet button
            //    Browser.SwitchFrame("/html/frameset/frame[1]");
            //    Browser.FindByXPathClick("/html/body/table/tbody/tr[4]/td/a[5]");
            //    Browser.DefaultFrame();
            //    //Switch Frame to the time entries
            //    Browser.SwitchFrame("/html/frameset/frame[2]");

            //    //Call WorkOrderCheck method - this will see if a new week is required and do some error checking on the timesheet before filling it out
            //    TimeSheet.WorkOrderCheck();

            //    List<string> dayArray = new List<string>();
            //    List<string> hoursArray = new List<string>();
            //    List<IWebElement> buttonArray = new List<IWebElement>();

            //    //Add the web form values into the dayArray list so we can compare what is in the form and what is in the settings.ini file.
            //    for (int i = 0; i < 7; i++)
            //    {
            //        buttonArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")));
            //        dayArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")).GetAttribute("value"));
            //    }
            //    //Read the settings.ini hours into the hoursArray list
            //    //TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
            //    for (int i = 0; i < settingday.Length; i++)
            //    {
            //        hoursArray.Add(MyIni.Read(settingday[i], "Week"));
            //    }

            //    bool hoursmatch = true;
            //    //Compare the hoursArray and dayArray Lists to see if they are the same...we might not need to do anything!
            //    if (hoursmatch == !dayArray.Except(hoursArray).Any())
            //    {
            //        Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "The Settings.ini file and whats on the etes page matches", ToolTipIcon.Info);
            //        //System.Windows.MessageBox.Show("The Settings.ini file and whats on the etes page matches");
            //    }
            //    else
            //    {
            //        Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "The Settings.ini file and the etes website hours do not match...going to try and populate now.", ToolTipIcon.Info);
            //        //System.Windows.MessageBox.Show("The Settings.ini file and the etes website hours do not match...going to try and populate now.");
            //        //Dayoftheweek is based on C# DayOfWeek, Sunday = 0, Monday = 1 etc, so to check Monday it must go through Sat, Sun and then Monday (or two extra days)
            //        for (int i = 0; i <= dayoftheweek + 1; i++)
            //        {
            //            //Clear the contents of the web field and then input the settings.ini value that was saved in the main form.
            //            buttonArray[i].Clear();
            //            buttonArray[i].SendKeys(hoursArray[i]);

            //        }
            //        if (dayoftheweek == 5 && fridayCheckBox.IsChecked == true)
            //        {
            //            Notification.Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", "Please review your timesheet.  If you need to add further details, now is the time to do so. Please submit manually if you need to edit any entries on the timesheet.", ToolTipIcon.Info);
            //            //System.Windows.MessageBox.Show("Please review your timesheet.  If you need to add further details, now is the time to do so. Please submit manually if you need to edit any entries on the timesheet.");
            //            return;
            //        }
            //        else
            //        {
            //            System.Windows.MessageBoxResult saveYesNo = System.Windows.MessageBox.Show("Would you like to Save the timesheet?", "Save Timesheet", MessageBoxButton.YesNo);
            //            if (saveYesNo == MessageBoxResult.Yes)
            //            {
            //                Browser.DefaultFrame();
            //                Browser.SwitchFrame("/html/frameset/frame[3]");
            //                Browser.FindByXPathClick("/html/body/table/tbody/tr/td/form/a[1]");     //Click Save
            //                Browser.browser.SwitchTo().Alert().Accept();
            //            }
            //            else
            //            {
            //                //System.Windows.MessageBox.Show("You have said NO...noone says no to me!!!!");
            //                return;
            //            }
            //        }
            //    }
            //}
            //else if (btn_start.Content as string == "Stop")
            //{
            //    Browser.Close();
            //    btn_start.Content = "Start";
            //}
            //else
            //{
            //    System.Windows.MessageBox.Show("It's not during the work week, sorry, we can't edit unless its Mon-Fri (before 5pm).", "Error", System.Windows.MessageBoxButton.OK, System.Windows.MessageBoxImage.Error);
            //}
        }

        private void comboBrowser_DropDownClosed(object sender, EventArgs e)
        {
            if (comboBrowser.SelectedIndex == 1)
            {
                var MyIni = new IniFile("Settings.ini");
                textBWait.Visibility = Visibility.Visible;
                textWaittime.Visibility = Visibility.Visible;
                textWaittime.Text = (MyIni.Read("Wait Time", "Wait Time"));
                textBlock8.Visibility = Visibility.Visible;
            }
            else
            {
                textBWait.Visibility = Visibility.Hidden;
                textWaittime.Visibility = Visibility.Hidden;
                textBlock8.Visibility = Visibility.Hidden;
            }

        }

    }
}
