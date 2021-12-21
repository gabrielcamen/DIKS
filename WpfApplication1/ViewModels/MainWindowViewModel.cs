using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using WpfApplication1.Commands;
using WpfApplication1.Models;
using WpfApplication1.Services;
using WPFTesting.Views;

namespace WpfApplication1.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly WindowService _windowService = new WindowService();
        private readonly UserNameChecker _userNameChecker = new UserNameChecker();
        
        private readonly IMessageSenderViewModelFactory _messageSenderViewModelFactory;

        private string _checkUserResultMessage;

        public string CheckUserResultMessage
        {
            get => _checkUserResultMessage;
            set
            {
                _checkUserResultMessage = value;
                OnPropertyChanged(nameof(CheckUserResultMessage));
            }
        }

        public User User { get; set; } = new User();
        public ICommand OnCheckUser { get; set; }
        public ICommand OnSendEmail { get; set; }
        
        public MainWindowViewModel()
        {
            LoadCommands();
        }

        private void CheckUser()
        {
            if (_userNameChecker.IsUserNameSecure(User.Name))
            {
                CheckUserResultMessage = "Good user!";
            }
            else
                CheckUserResultMessage = "Bad user!";
        }

        private void LoadCommands()
        {
            OnCheckUser = new RelayCommand(CheckUser, () => true);
            OnSendEmail = new RelayCommand(SendEmail, () => true);
        }

        private void SendEmail()
        {
            var viewModel = _messageSenderViewModelFactory.Create();
            _windowService.ShowWindow<EmailChecker>(viewModel);
            viewModel.SendEmail();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
