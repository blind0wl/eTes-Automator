//Version 0.1.5 - Dave Payne 2018
//eTes Automator for Timesheet automated completion

using System;
using System.ComponentModel;
using System.Windows;
using AesEncDec;
using System.IO;
using AutoUpdaterDotNET;

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
        public string securitychoice = string.Empty;
        public string fridaycheck = string.Empty;
        public string appliedcheck = string.Empty;
        public string browserclose = string.Empty;


        public MainWindow()
        {
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.Start("http://162.217.248.211/etes/Updater.xml");
            
            InitializeComponent();
            AppWindow = this;
            Notification.CreateNotify();
            Scheduler sc = new Scheduler();
            sc.Start();
            

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
                Visibility = Visibility.Visible;
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
                MyIni.Write("Security", "System.Windows.Controls.ComboBoxItem: VIP App (PC)", "Security Choice");
                MyIni.Write("ManualSubmit", "False", "Submit");
                MyIni.Write("Reminded", "False", "Reminders"); //sets notifyicon to ignore or send the info message about closing from the notification area.
                MyIni.Write("Applied", "False", "Check"); //checks that user has applied their data before starting browser
                MyIni.Write("SalesForce", "False", "Check"); //sets salesforce autologin option at setting.ini creation (off by default)
                MyIni.Write("CloseBrowser", "False", "Check"); //sets whether to close the browser after updating the timesheet or not.  (off by default)


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

                var securitychoice = MyIni.Read("Security", "Security Choice");
                if (securitychoice == "System.Windows.Controls.ComboBoxItem: VIP App (PC)")
                {
                    comboSecurity.SelectedIndex = 0;
                }
                else
                {
                    comboSecurity.SelectedIndex = 1;
                }

                //Check if the Submit on Friday checkbox is checked or not and apply the correct check against the check object
                fridaycheck = MyIni.Read("ManualSubmit", "Submit");
                if (fridaycheck == "True")
                {
                    fridayCheckBox.IsChecked = true;
                }
                else
                {
                    fridayCheckBox.IsChecked = false;
                }
                
                //Check if the Close Browser checkbox is checked or not and apply the correct check against the check object
                browserclose = MyIni.Read("CloseBrowser", "Check");
                if (browserclose == "True")
                {
                    ChkCloseAfterUpdate.IsChecked = true;
                }
                else
                {
                    ChkCloseAfterUpdate.IsChecked = false;
                }

                //Read the apply check into the variable so that the Start button can run.  This is required so the user cant start it without applying their username and pass.
                var appliedcheck = MyIni.Read("Applied", "Check");
               
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
                //Read False into appliedcheck to ensure user cant start the program without applying their config.
                appliedcheck = MyIni.Read("Applied", "Check");
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

                securitychoice = MyIni.Read("Security", "Security Choice");
                if (securitychoice == "System.Windows.Controls.ComboBoxItem: VIP App (PC)")
                {
                    comboSecurity.SelectedIndex = 0;
                }
                else
                {
                    comboSecurity.SelectedIndex = 1;
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

                //Check if the Close Browser checkbox is checked or not and apply the correct check against the check object
                var browserclose = MyIni.Read("CloseBrowser", "Check");
                if (browserclose == "True")
                {
                    ChkCloseAfterUpdate.IsChecked = true;
                }
                else
                {
                    ChkCloseAfterUpdate.IsChecked = false;
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
                //Read the apply check into the variable so that the Start button can run.  This is required so the user cant start it without applying their username and pass.
                var appliedcheck = MyIni.Read("Applied", "Check");
                //Read the Settings.ini file to see if we need to set the checkbox true or false.
                if (MyIni.Read("SalesForce", "Check") == "True")
                {
                    ChkSalesForce.IsChecked = true;
                    //If the checkbox for salesforce is checked, run the salesforce automation schedule.The ?? false is because IsChecked is an interdeminable bool, so it can be true, false, or null.The ?? tells it, if its null then just set it false.
                    //Scheduler sf = new Scheduler();
                    //sf.SFStart();
                }
                else
                {
                    ChkSalesForce.IsChecked = false;
                }
            }
        }

        protected override void OnStateChanged(EventArgs e)
        {
            if (WindowState == WindowState.Minimized)
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
                MessageBox.Show("Username or Password is invalid");
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

                MessageBox.Show(String.Format("User {0} configuration was successfully saved.", textUsername.Text));

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
            MyIni.Write("Security", comboSecurity.SelectedItem.ToString(), "Security Choice");
            MyIni.Write("ManualSubmit", fridayCheckBox.IsChecked.ToString(), "Submit");
            MyIni.Write("CloseBrowser", ChkCloseAfterUpdate.IsChecked.ToString(), "Check");
            securitychoice = MyIni.Read("Security", "Security Choice");
            browserchoice = MyIni.Read("Browser", "Browser Choice");
            browserclose = MyIni.Read("CloseBrowser", "Check");
            //Once apply is clicked write true to the settings.ini file and then read it into our public string of appliedcheck so that when start checks it will allow it through
            MyIni.Write("Applied", "True", "Check");
            appliedcheck = MyIni.Read("Applied", "Check");

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
            if (appliedcheck == "False")
            {
                MessageBox.Show("Please apply your changes before starting the automated timesheet.");
            }
            else
            {
                if (btn_start.Content.ToString() == ("Start"))
                {
                    var MyIni = new IniFile("Settings.ini");

                    //Should probably be writing the choices they have made prior to pressing Start instead of reading old values
                    //browserchoice = MyIni.Read("Browser", "Browser Choice");
                    //securitychoice = MyIni.Read("Security", "Security Choice");
                    //browserclose = MyIni.Read("CloseBrowser", "Check");
                    //appliedcheck = MyIni.Read("Applied", "Check");
                    //Causing issues with copy of VIP ID
                    //Notification.Bubble("Filling out your timesheet, be prepared to authenticate");

                    MyIni.Write("Browser", comboBrowser.SelectedItem.ToString(), "Browser Choice");
                    MyIni.Write("Security", comboSecurity.SelectedItem.ToString(), "Security Choice");
                    MyIni.Write("ManualSubmit", fridayCheckBox.IsChecked.ToString(), "Submit");
                    MyIni.Write("CloseBrowser", ChkCloseAfterUpdate.IsChecked.ToString(), "Check");
                    browserchoice = MyIni.Read("Browser", "Browser Choice");
                    securitychoice = MyIni.Read("Security", "Security Choice");
                    browserclose = MyIni.Read("CloseBrowser", "Check");
                    appliedcheck = MyIni.Read("Applied", "Check");

                    btn_start.Content = "Stop";
                    await TimeSheet.StartTimesheet();
                }
                else
                {
                    Browser.Close();
                    btn_start.Content = "Start";
                }
            }
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
                browserchoice = "System.Windows.Controls.ComboBoxItem: Firefox";
            }
            else
            {
                textBWait.Visibility = Visibility.Hidden;
                textWaittime.Visibility = Visibility.Hidden;
                textBlock8.Visibility = Visibility.Hidden;
                browserchoice = "System.Windows.Controls.ComboBoxItem: Chrome";
            }

        }

        private void BtnUpdate_Click(object sender, RoutedEventArgs e)
        {
            AutoUpdater.ReportErrors = true;
            AutoUpdater.Mandatory = true;
            AutoUpdater.RunUpdateAsAdmin = true;
            AutoUpdater.Start("http://162.217.248.211/etes/Updater.xml");
        }

        private async void ButtonSF_Click(object sender, RoutedEventArgs e)
        {

            await SalesForce.Automate();
        }

        private void ChkSalesForce_Checked(object sender, RoutedEventArgs e)
        {
            var MyIni = new IniFile("Settings.ini");
            MyIni.Write("SalesForce", "True", "Check");
            Scheduler sf = new Scheduler();
            sf.SFStart();
        }

        private void ChkSalesForce_Unchecked(object sender, RoutedEventArgs e)
        {
            var MyIni = new IniFile("Settings.ini");
            MyIni.Write("SalesForce", "False", "Check");
            Scheduler sf = new Scheduler();
            sf.InterruptSF();
        }
    }
}
