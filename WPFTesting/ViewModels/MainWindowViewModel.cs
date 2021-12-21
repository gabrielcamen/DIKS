using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Ninject;
using WPFTesting.Annotations;
using WPFTesting.Commands;
using WPFTesting.Models;
using WPFTesting.Models.Interfaces;
using WPFTesting.Services;
using WPFTesting.Views;

namespace WPFTesting.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IWindowService _windowService;
        private readonly IUserNameChecker _userNameChecker;
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
        
        public MainWindowViewModel(IUserNameChecker userNameChecker, IWindowService windowService, IMessageSenderViewModelFactory messageSenderViewModelFactory)
        {
            _userNameChecker = userNameChecker;
            _windowService = windowService;
            _messageSenderViewModelFactory = messageSenderViewModelFactory;
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

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
