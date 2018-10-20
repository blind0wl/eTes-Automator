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
            if (btn_start.Content as string == "Start" && currentDateTime.Hour < 17 && (currentDateTime.DayOfWeek != DayOfWeek.Saturday) && (currentDateTime.DayOfWeek != DayOfWeek.Sunday))
            {
                string etes = "https://etes.csc.com";
                string test = comboBrowser.SelectedItem.ToString();
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
                    goto browserexit;
                }
                
                btn_start.Content = "Stop";
                //Enter Username and Password fields of the VIP Access page - using existing saved username and pass
                await Task.Delay(5000);
                Browser.FindNameSendKeys("UserName", decusr);
                Browser.FindNameSendKeys("Password", decpass);
                Browser.IDClick("submitButton");
                //Read the waittime value in the Settings.ini so the function waits for the customised time.
                var MyIni = new IniFile("Settings.ini");
                MyIni.Write("Wait Time", textWaittime.Text, "Wait Time");
                waittime = Convert.ToInt32(MyIni.Read("Wait Time", "Wait Time"));
                await Task.Delay(waittime * 1000);
                try
                {
                    //Find the MenuBar frame and search and click the TimeSheet button
                    Browser.SwitchFrame("/html/frameset/frame[1]");
                    Browser.FindByXPathClick("/html/body/table/tbody/tr[4]/td/a[5]");
                    Browser.DefaultFrame();
                    //Switch Frame to the time entries
                    Browser.SwitchFrame("/html/frameset/frame[2]");
                    /*
                    var button0 = Browser.browser.FindElement(By.Name("button0_0")).GetAttribute("value");
                    var button1 = Browser.browser.FindElement(By.Name("button1_0")).GetAttribute("value");
                    var button2 = Browser.browser.FindElement(By.Name("button2_0")).GetAttribute("value");
                    var button3 = Browser.browser.FindElement(By.Name("button3_0")).GetAttribute("value");
                    var button4 = Browser.browser.FindElement(By.Name("button4_0")).GetAttribute("value");
                    var button5 = Browser.browser.FindElement(By.Name("button5_0")).GetAttribute("value");
                    var button6 = Browser.browser.FindElement(By.Name("button6_0")).GetAttribute("value");

                    MessageBox.Show(button0);
                    MessageBox.Show(button1);
                    MessageBox.Show(button2);
                    MessageBox.Show(button3);
                    MessageBox.Show(button4);
                    MessageBox.Show(button5);
                    MessageBox.Show(button6);
                    */

                    //Check that the current day is within the time period that you can enter data into the fields...Monday to Friday.  Shouldnt matter much once it is scheduled.

                    int dayoftheweek = (int)currentDateTime.DayOfWeek;
                    if (dayoftheweek >= 1 && dayoftheweek <= 5)
                    {
                        List<string> dayArray = new List<string>();
                        List<string> hoursArray = new List<string>();

                        //Add the web form values into the dayArray list so we can compare what is in the form and what is in the settings.ini file.
                        for (int i = 0; i < 7; i++)
                        {
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
                            MessageBox.Show("The Settings.ini file and whats on the etes matches");
                        }
                        else
                        {
                            MessageBox.Show("The Settings.ini file and the etes website hours do not match...going to try and populate now.");
                            //Dayoftheweek is based on C# DayOfWeek, Sunday = 0, Monday = 1 etc, so to check Monday it must go through Sat, Sun and then Monday (or two extra days)
                            for (int i = 0; i <= dayoftheweek + 2; i++)
                            {
                                //Clear the contents of the web field and then input the settings.ini value that was saved in the main form.

                                //Browser.browser.FindElement(By.Name(dayArray[i])).Clear();
                                //Browser.browser.FindElement(By.Name(dayArray[i])).SendKeys(hoursArray[i]);
                                //Same thing, just using Method instead
                                Browser.FindNameClear(dayArray[i]);
                                Browser.FindNameSendKeys(dayArray[i], hoursArray[i]);
                            }

                        }
                    }
                    else
                    {
                        MessageBox.Show("Outside of acceptable day range.");
                        goto nothing;
                    }
                }
                catch (Exception)
                {
                    //Page didnt load quick enough in for the wait time value
                    Browser.Close();
                    btn_start.Content = "Start";
                    MessageBox.Show("Please change the wait time configuration to a higher value.  Unfortunately we are unable to control the time it takes to press the Approve button on your mobile app and the program needs time for the page to load.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

                }
            browserexit:
                MessageBox.Show("Ensure a Browser is chosen");
            }
            
                
            else
            {
                MessageBox.Show("It's not during the work week, sorry, we can't edit unless its Mon-Fri (before 5pm).", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        nothing:
            Browser.Close();
            btn_start.Content = "Start";
            MessageBox.Show("Going back to application");j
        }

        
    }
}
