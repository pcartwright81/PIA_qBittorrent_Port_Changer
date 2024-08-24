namespace PIA_qBittorrent_Port_Changer
{
    using IniParser;
    using System.Diagnostics;
    using System.IO;

    public class PortChecker
    {
        public TrayIconViewModel ViewModel { get; }

        public PortChecker(TrayIconViewModel viewModel)
        {
            ViewModel = viewModel;
            viewModel.checkNowEvent += (sender, e) => CheckPorts();
        }


        public void CheckPorts()
        {
            var piaPort = GetForwardedPort();
            var settingsFilePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), @"qBittorrent\qBittorrent.ini");
            var parser = new FileIniDataParser();
            var data = parser.ReadFile(settingsFilePath);
            var qBitPort = data["BitTorrent"]["Session\\Port"];
            ViewModel.PiaPort = piaPort;
            ViewModel.QbitPort = qBitPort;
            if (!qBitPort.Equals(piaPort))
            {
                KillTorrentProcess();

                data["BitTorrent"]["Session\\Port"] = piaPort;
                parser.WriteFile(settingsFilePath, data);

                Process.Start(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"qBittorrent\qbittorrent.exe"));
            }
        }
        
        private static string GetForwardedPort()
        {
            var process = new Process();
            process.StartInfo.FileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles), @"Private Internet Access\piactl.exe");
            process.StartInfo.Arguments = "get portforward";
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.WaitForExit();
            var output = process.StandardOutput.ReadToEnd();
            return output.Trim();
        }

        private static void KillTorrentProcess()
        {
            var qBitTorrentProcess = Process.GetProcesses().Where(pr => pr.ProcessName == "qbittorrent");

            foreach (var process in qBitTorrentProcess)
            {
                process.Kill();
            }
        }
    }
}
