namespace PIA_qBittorrent_Port_Changer
{
    using H.NotifyIcon;
    using Microsoft.Win32;
    using System.Timers;
    using System.Windows;

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
            notifyIcon = (TaskbarIcon)FindResource("NotifyIcon");
            notifyIcon.DataContext = trayIconViewModel;
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
      
        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose(); //the icon would clean up automatically, but this is cleaner
            base.OnExit(e);
        }
    }
}
