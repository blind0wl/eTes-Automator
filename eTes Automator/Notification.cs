using System;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace eTes_Automator
{
    class Notification
    {
        public static class Globals
        {
            public static NotifyIcon nIcon = new NotifyIcon();
        }
        public static void CreateNotify()
        {
            
            //Hide main window when the program begins
            MainWindow.AppWindow.ShowInTaskbar = true;
            MainWindow.AppWindow.Visibility = Visibility.Hidden;
            //MainWindow.AppWindow.WindowState = System.Windows.WindowState.Normal;
            Globals.nIcon.Icon = new Icon(@"../../images/clock.ico");
            //Globals.nIcon.Icon = new Icon(@"images/clock.ico");
            Globals.nIcon.Visible = true;
            Globals.nIcon.Text = "eTes Automator";
            Globals.nIcon.DoubleClick +=
                delegate (object sender, EventArgs args)
                {
                    MainWindow.AppWindow.Show();
                    MainWindow.AppWindow.WindowState = WindowState.Normal;
                };
            System.Windows.Forms.ContextMenu notifyIconContextMenu = new System.Windows.Forms.ContextMenu();
            notifyIconContextMenu.MenuItems.Add("Quit", new EventHandler(Quit));
            Globals.nIcon.ContextMenu = notifyIconContextMenu;
        }

        public static void Bubble(String text)
        {
            Globals.nIcon.ShowBalloonTip(3000, "eTes Automator", text, ToolTipIcon.Info);
        }

        public static void Quit(object sender, EventArgs e)
        {
            Globals.nIcon.Dispose();
            Environment.Exit(0);
        }
    }
}
