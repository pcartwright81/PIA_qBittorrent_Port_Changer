namespace PIA_qBittorrent_Port_Changer
{
    using H.NotifyIcon;
    using Microsoft.Win32;
    using System.IO;
    using System.Timers;
    using System.Windows;
    using System.Windows.Controls;
    using System.Windows.Data;

    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private TaskbarIcon? notifyIcon;
        private static readonly TrayIconViewModel trayIconViewModel = new();

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var icon = PIA_qBittorrent_Port_Changer.Properties.Resources.Icon;
            ShutdownMode = ShutdownMode.OnExplicitShutdown;

            // Create tray.
            notifyIcon = new TaskbarIcon
            {
                Icon = new System.Drawing.Icon(new MemoryStream(icon)),
                DataContext = trayIconViewModel,
                ContextMenu = BuildMenu()
            };
            notifyIcon.ForceCreate();
            
            // Set up timer for check.
            PortChecker portChecker = new(trayIconViewModel);
            var aTimer = new Timer();
            aTimer.Elapsed += (sender, e) => portChecker.CheckPorts();
            aTimer.Interval = TimeSpan.FromHours(5).TotalMilliseconds;
            aTimer.Enabled = true;
            portChecker.CheckPorts();

            // Register from resume from sleep event.
            SystemEvents.PowerModeChanged += (sender, e) =>
            {
                switch (e.Mode)
                {
                    case PowerModes.Resume:
                        portChecker.CheckPorts();
                        break;
                };
            };
        }

        private static ContextMenu BuildMenu()
        {

            var menu = new ContextMenu();
            var menuItem = new MenuItem();
            Binding myBinding = new Binding("PiaPortDisplay")
            {
                Source = trayIconViewModel
            };
            menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, myBinding);   
            menu.Items.Add(menuItem);

            menuItem = new MenuItem();
            myBinding = new Binding("QbitPortDisplay")
            {
                Source = trayIconViewModel
            };
            menuItem.SetBinding(HeaderedItemsControl.HeaderProperty, myBinding);
            menu.Items.Add(menuItem);

            menu.Items.Add(new MenuItem
            {
                Header = "Check Now",
                Command = trayIconViewModel.CheckNowCommand
            });
            menu.Items.Add(new Separator());
            menu.Items.Add(new MenuItem
            {
                Header = "Exit",
                Command = trayIconViewModel.ExitCommand
            });
            return menu;
        }

        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
