using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace InputRecorder
{
    partial class App
    {
        private static NotifyIcon notifyIcon;
        private ContextMenu contextMenu;
        private MenuItem menuItemExit;
        private static EventTimer timerReplay;

        private void InitializeComponents()
        {
            //////////////menuItemExit//////////////
            menuItemExit = new MenuItem();
            menuItemExit.Text = "Exit";
            menuItemExit.Click += new EventHandler(menuItemExit_Clicked);

            //////////////contextMenu//////////////
            contextMenu = new ContextMenu();
            contextMenu.MenuItems.Add(menuItemExit);

            //////////////notifyIcon//////////////
            notifyIcon = new NotifyIcon();
            notifyIcon.Icon = InputRecorder.Properties.Resources.AOEICON;
            notifyIcon.Text = "Not Recording";
            notifyIcon.ContextMenu = contextMenu;
            notifyIcon.Visible = true;

            //////////////timerReplay//////////////
            timerReplay = new EventTimer(1);
            timerReplay.Elapsed += new EventTimerElapsedHandler(timerReplay_Tick);
        }

        //Set Events
        private void InitializeEvents()
        {
            Application.ApplicationExit += new EventHandler(OnAppExit);
            InitializeSystemHook();
        }
    }
}
