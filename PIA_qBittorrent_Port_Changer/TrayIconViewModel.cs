namespace PIA_qBittorrent_Port_Changer
{
    using Prism.Commands;
    using System.ComponentModel;
    using System.Runtime.CompilerServices;
    using System.Windows;
    using System.Windows.Input;

    public class TrayIconViewModel : INotifyPropertyChanged
    {
        public EventHandler? checkNowEvent;
        private string? piaPort;
        private string? qbitPort;

        public TrayIconViewModel()
        {
            ExitCommand = new DelegateCommand(ExecuteExitCommand);
            CheckNowCommand = new DelegateCommand(() => checkNowEvent?.Invoke(this, EventArgs.Empty));
        }

        private void ExecuteExitCommand()
        {
            Application.Current.Shutdown();
        }

        public ICommand? ExitCommand { get; set; }

        public ICommand? CheckNowCommand { get; set; }

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

        public string QbitPortDisplay => $"Qbit:{QbitPort}";

        public string PiaPortDisplay => $"Pia:{PiaPort}";

      

        public event PropertyChangedEventHandler? PropertyChanged;

        // Create the OnPropertyChanged method to raise the event
        // The calling member's name will be used as the parameter.
        protected void OnPropertyChanged([CallerMemberName] string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
