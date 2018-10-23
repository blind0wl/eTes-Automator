//Version 0.1 - Dave Payne 2018
//eTes Automator for Timesheet automated completion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using AesEncDec;
using System.IO;
using Path = System.IO.Path;
using OpenQA.Selenium;


namespace eTes_Automator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string decusr = string.Empty;
        private string decpass = string.Empty;
        private string[] settingday = new string[] { "Saturday", "Sunday", "Monday", "Tuesday", "Wednesday", "Thursday", "Friday" };        
        private int waittime = 15000;

        public MainWindow()
        {
            InitializeComponent();            

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
                MessageBox.Show("Ensure you fill out the login and password details and Apply.");
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
                MyIni.Write("Browser", "Chrome", "Browser Choice");
                

                //Set variables with Settings.ini values
                //string[] settingday = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };             
                //TextBox[] textboxes = { textMon, textTue, textWed, textThur, textFri, textSat, textSun };
                TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                for (int i = 0; i < settingday.Length; i++)
                {
                    textboxes[i].Text = MyIni.Read(settingday[i], "Week");
                }
                waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
                textWaittime.Text = Convert.ToString(waittime);

                //Read the values in Settings.ini for each day of the week, then populate the textbox with that value.
                /*
                var monday = MyIni.Read("Monday", "Week");
                var tuesday = MyIni.Read("Tuesday", "Week");
                var wednesday = MyIni.Read("Wednesday", "Week");
                var thursday = MyIni.Read("Thursday", "Week");
                var friday = MyIni.Read("Friday", "Week");
                var saturday = MyIni.Read("Saturday", "Week");
                var sunday = MyIni.Read("Sunday", "Week");

                

                //Populate the textboxes with the values saved in the ini file
                textSat.Text = saturday;
                textSun.Text = sunday;
                textMon.Text = monday;
                textTue.Text = tuesday;
                textWed.Text = wednesday;
                textThur.Text = thursday;
                textFri.Text = friday;

                textWaittime.Text = Convert.ToString(waittime);
                */
            }
            else
            {
                //Read Settings.ini
                var MyIni = new IniFile("Settings.ini");

                //string[] settingday = new string[] { "Monday", "Tuesday", "Wednesday", "Thursday", "Friday", "Saturday", "Sunday" };
                //TextBox[] textboxes = { textMon, textTue, textWed, textThur, textFri, textSat, textSun };
                TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                for (int i = 0; i < settingday.Length; i++)
                {
                    textboxes[i].Text = MyIni.Read(settingday[i], "Week");
                }

                waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
                textWaittime.Text = Convert.ToString(waittime);
                var browserchoice = MyIni.Read("Browser", "Browser Choice");
                if (browserchoice == "System.Windows.Controls.ComboBoxItem: Chrome")
                {
                    comboBrowser.SelectedIndex = 0;
                }
                else
                {
                    comboBrowser.SelectedIndex = 1;
                }

                
                /*
                //Set variables with Settings.ini values
                var monday = MyIni.Read("Monday", "Week");
                var tuesday = MyIni.Read("Tuesday", "Week");
                var wednesday = MyIni.Read("Wednesday", "Week");
                var thursday = MyIni.Read("Thursday", "Week");
                var friday = MyIni.Read("Friday", "Week");
                var saturday = MyIni.Read("Saturday", "Week");
                var sunday = MyIni.Read("Sunday", "Week");
                //Read the Wait Time value from Settings.ini.  This will control the time between authenticating and finding the frame.
                waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));

                //Populate the textboxes with the values saved in the ini file
                textSat.Text = saturday;
                textSun.Text = sunday;
                textMon.Text = monday;
                textTue.Text = tuesday;
                textWed.Text = wednesday;
                textThur.Text = thursday;
                textFri.Text = friday;
                //Convert Wait time back to a string to populate the text box
                textWaittime.Text = Convert.ToString(waittime);
                */
            }

            
    }
        
        private void btn_Apply_Click(object sender, RoutedEventArgs e)
        {
            if (textUsername.Text.Length < 3 || passwordBox.Password.Length < 5)
            {
                MessageBox.Show("Username or Password is invalid");
            }
            else
            {
                //string dir = textUsername.Text;
                //string dir = Path.GetRandomFileName();   //Obfuscation of folder name...not sure how it will react when calling it though as its random.
                Directory.CreateDirectory("data\\");

                var sw = new StreamWriter("data\\data.ls");

                string encusr = AesCrypt.Encrypt(textUsername.Text);
                string encpass = AesCrypt.Encrypt(passwordBox.Password);
                
                sw.WriteLine(encusr);
                sw.WriteLine(encpass);
                sw.Close();

                MessageBox.Show(String.Format("User {0} configuration was successfully saved.", textUsername.Text));

                var sr = new StreamReader("data\\data.ls");

                string r_encusr = sr.ReadLine();
                string r_encpass = sr.ReadLine();
                sr.Close();

                decusr = AesCrypt.Decrypt(r_encusr);
                decpass = AesCrypt.Decrypt(r_encpass);

                textUsername.Text = decusr;
                textPassword.Text = decpass;
                passwordBox.Password = decpass;


            }
            var MyIni = new IniFile("Settings.ini");
            TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
            for (int i = 0; i < settingday.Length; i++)
            {
                MyIni.Write(settingday[i], textboxes[i].Text, "Week");
            }
            MyIni.Write("Wait Time", textWaittime.Text, "Wait Time");
            
            MyIni.Write("Browser", comboBrowser.SelectedItem.ToString(), "Browser Choice");
            /*
            //Set default values for days of the week
            MyIni.Write("Saturday", textSat.Text, "Week");
            MyIni.Write("Sunday", textSun.Text, "Week");
            MyIni.Write("Monday", textMon.Text, "Week");
            MyIni.Write("Tuesday", textTue.Text, "Week");
            MyIni.Write("Wednesday", textWed.Text, "Week");
            MyIni.Write("Thursday", textThur.Text, "Week");
            MyIni.Write("Friday", textFri.Text, "Week");
            
            */
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
            DateTime currentDateTime = DateTime.Now;
            if (btn_start.Content as string == "Start") //&& currentDateTime.Hour < 17 && (currentDateTime.DayOfWeek != DayOfWeek.Saturday) && (currentDateTime.DayOfWeek != DayOfWeek.Sunday))
            {
                string etes = "https://etes.csc.com";
                if (comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Chrome")
                {
                    Browser.StartBrowserChrome(etes);
                }
                else if (comboBrowser.SelectedItem.ToString() == "System.Windows.Controls.ComboBoxItem: Firefox")
                {
                    Browser.StartBrowserFirefox(etes);
                }
                else
                {
                    MessageBox.Show("Something went wrong with your choice, make sure you have the browser you've chosen installed.");
                }
                
                btn_start.Content = "Stop";
                //Enter Username and Password fields of the VIP Access page - using existing saved username and pass
                Browser.WaitforBrowser("Sign In");
                //await Task.Delay(5000);
                Browser.FindNameSendKeys("UserName", decusr);
                Browser.FindNameSendKeys("Password", decpass);
                Browser.IDClick("submitButton");
                Browser.WaitforBrowser("Internet Time Entry System");
                
                
                //Read the waittime value in the Settings.ini so the function waits for the customised time.
                var MyIni = new IniFile("Settings.ini");
                /*
                MyIni.Write("Wait Time", textWaittime.Text, "Wait Time");
                waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
                await Task.Delay(waittime * 1000);
                */
                
                //Find the MenuBar frame and search and click the TimeSheet button
                Browser.SwitchFrame("/html/frameset/frame[1]");
                Browser.FindByXPathClick("/html/body/table/tbody/tr[4]/td/a[5]");
                Browser.DefaultFrame();
                //Switch Frame to the time entries
                Browser.SwitchFrame("/html/frameset/frame[2]");

                string title2check = Browser.FindByXpathTitle("/html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a", "title");
                //string titlecheck = Browser.browser.FindElement(By.XPath("/html/body/form/table[3]/tbody/tr/td/table[1]/tbody/tr[2]/td[1]/left/a")).GetAttribute("title");
                if (title2check != "REGULAR HOURS")
                {
                    MessageBox.Show("New Week has started, populating Work Order for you now");
                    string workorder = Browser.browser.FindElement(By.Id("QE")).GetAttribute("value");
                    if (workorder == "REGULAR HOURS")
                    {
                        Browser.IDClick("addfromqw");
                    }
                    else
                    {
                        MessageBox.Show("Can't create the workorder as it does not follow the standard 1010 REGULAR HOURS expected. Going to close the browser, you will need to create a WORKORDER in the timesheet manually.");
                        goto Alldone;
                    }
                }
                //Check that the current day is within the time period that you can enter data into the fields...Monday to Friday.  Shouldnt matter much once it is scheduled.
                int dayoftheweek = (int)currentDateTime.DayOfWeek;
                if (dayoftheweek >= 1 && dayoftheweek <= 5)
                {
                    List<string> dayArray = new List<string>();
                    List<string> hoursArray = new List<string>();
                    List<IWebElement> buttonArray = new List<IWebElement>();

                    //Add the web form values into the dayArray list so we can compare what is in the form and what is in the settings.ini file.
                    for (int i = 0; i < 7; i++)
                    {
                        buttonArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")));
                        dayArray.Add(Browser.browser.FindElement(By.Name("button" + i + "_0")).GetAttribute("value"));
                    }
                    //Read the settings.ini hours into the hoursArray list
                    //TextBox[] textboxes = { textSat, textSun, textMon, textTue, textWed, textThur, textFri };
                    for (int i = 0; i < settingday.Length; i++)
                    {
                        hoursArray.Add(MyIni.Read(settingday[i], "Week"));
                    }
                        
                    bool hoursmatch = true;
                    //Compare the hoursArray and dayArray Lists to see if they are the same...we might not need to do anything!
                    if (hoursmatch = !dayArray.Except(hoursArray).Any())
                    {
                        MessageBox.Show("The Settings.ini file and whats on the etes page matches");
                    }
                    else
                    {
                        MessageBox.Show("The Settings.ini file and the etes website hours do not match...going to try and populate now.");
                        //Dayoftheweek is based on C# DayOfWeek, Sunday = 0, Monday = 1 etc, so to check Monday it must go through Sat, Sun and then Monday (or two extra days)
                        for (int i = 0; i <= dayoftheweek + 1; i++)
                        {
                            //Clear the contents of the web field and then input the settings.ini value that was saved in the main form.
                            buttonArray[i].Clear();
                            buttonArray[i].SendKeys(hoursArray[i]);
                            
                        }
                        MessageBoxResult saveYesNo = MessageBox.Show("Would you like to Save the timesheet?", "Save Timesheet", MessageBoxButton.YesNo);
                        if (saveYesNo == MessageBoxResult.Yes)
                        {
                            Browser.DefaultFrame();
                            Browser.SwitchFrame("/html/frameset/frame[3]");
                            Browser.FindByXPathClick("/html/body/table/tbody/tr/td/form/a[1]");     //Click Save
                            Browser.browser.SwitchTo().Alert().Accept();
                            //goto Alldone;
                        }
                        else
                        {
                            MessageBox.Show("You have said NO...noone says no to me!!!!");
                            goto Alldone;
                        }

                    }
                }
                else
                {
                    MessageBox.Show("Outside of acceptable day range.");
                }
            }   
            else
            {
                MessageBox.Show("It's not during the work week, sorry, we can't edit unless its Mon-Fri (before 5pm).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            Alldone:
                //Browser.Close();
                btn_start.Content = "Start";
                MessageBox.Show("Going back to application");
        }
    }
}
