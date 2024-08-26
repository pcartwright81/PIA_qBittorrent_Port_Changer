namespace PIA_qBittorrent_Port_Changer
{
    using H.NotifyIcon;
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
            var portChecker = new PortChecker(trayIconViewModel);
            portChecker.CheckPorts();
        }
      
        protected override void OnExit(ExitEventArgs e)
        {
            notifyIcon?.Dispose();
            base.OnExit(e);
        }
    }
}
