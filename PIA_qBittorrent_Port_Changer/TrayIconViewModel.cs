namespace PIA_qBittorrent_Port_Changer
{
    using Prism.Commands;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    public class TrayIconViewModel : INotifyPropertyChanged
    {
        private string? piaPort;
        private string? qbitPort;
        private bool isPaused;

        public TrayIconViewModel()
        {
            ExitCommand = new DelegateCommand(Application.Current.Shutdown);
        }

        public ICommand? ExitCommand { get; set; }

        public ICommand? CheckNowCommand { get; set; }

        public ICommand? PauseCommand { get; set; }

        public string? PiaPort
        {
            get => piaPort;
            set
            {
                piaPort = value;
                OnPropertyChanged(nameof(PiaPortDisplay));
            }
        }
        public string? QbitPort
        {
            get => qbitPort; 
            set
            {
                qbitPort = value;
                OnPropertyChanged(nameof(QbitPortDisplay));
            }
        }

        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                OnPropertyChanged(nameof(PauseText));
            }
        }

        public string QbitPortDisplay => $"Qbit:{QbitPort}";

        public string PiaPortDisplay => $"Pia:{PiaPort}";

        public string PauseText => IsPaused ?  "Resume" : "Pause";

        public event PropertyChangedEventHandler? PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
