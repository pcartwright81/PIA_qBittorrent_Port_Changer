namespace PIA_qBittorrent_Port_Changer
{
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using Prism.Commands;
    using System.Text;
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
                OnPropertyChanged(nameof(ToolTipText));
            }
        }
        public string? QbitPort
        {
            get => qbitPort; 
            set
            {
                qbitPort = value;
                OnPropertyChanged(nameof(QbitPortDisplay));
                OnPropertyChanged(nameof(ToolTipText));
            }
        }

        public bool IsPaused
        {
            get => isPaused;
            set
            {
                isPaused = value;
                OnPropertyChanged(nameof(PauseText));
                OnPropertyChanged(nameof(ToolTipText));
            }
        }

        public string QbitPortDisplay => $"Qbit: {QbitPort}";

        public string PiaPortDisplay => $"Pia: {PiaPort}";

        public string PauseText => IsPaused ?  "Resume" : "Pause";

        public string ToolTipText
        { 
            get
            {
                    var sbTemp = new StringBuilder();
                    sbTemp.AppendLine("PIA Qbit Port Changer");
                    sbTemp.AppendLine($"State: {(IsPaused ? "Paused" : "Running")}");
                    sbTemp.AppendLine(QbitPortDisplay);
                    sbTemp.Append(PiaPortDisplay);
                    return sbTemp.ToString();
            } 
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        protected bool SetProperty<T>(ref T field, T newValue, [CallerMemberName] string? propertyName = null)
        {
            if (!Equals(field, newValue))
            {
                field = newValue;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
                return true;
            }

            return false;
        }

        
    }
}
