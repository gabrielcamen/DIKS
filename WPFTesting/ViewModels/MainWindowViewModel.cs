using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using WPFTesting.Annotations;
using WPFTesting.Commands;
using WPFTesting.Models;
using WPFTesting.Models.Interfaces;
using WPFTesting.Repository;
using WPFTesting.Services;
using WPFTesting.Views;

namespace WPFTesting.ViewModels
{
    public class MainWindowViewModel : INotifyPropertyChanged
    {
        private readonly IWindowService _windowService;
        private readonly IUserNameChecker _userNameChecker;
        private readonly IMessageSenderViewModelFactory _messageSenderViewModelFactory;
        private readonly IAccountsRepository _accountsRepository;
        private readonly IViewAccountsViewModelFactory _viewAccountModelFactory;
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
        public ICommand OnCheckUser { get; private set; }
        public ICommand OnSendEmail { get; private set; }
        public ICommand OnShowAllUsers{ get; private set; }

        public MainWindowViewModel(IUserNameChecker userNameChecker, IWindowService windowService, IMessageSenderViewModelFactory messageSenderViewModelFactory,
            IAccountsRepository accountsRepository, IViewAccountsViewModelFactory viewAccountModelFactory)
        {
            _userNameChecker = userNameChecker;
            _windowService = windowService;
            _messageSenderViewModelFactory = messageSenderViewModelFactory;
            _accountsRepository = accountsRepository;
            _viewAccountModelFactory = viewAccountModelFactory;
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
            OnShowAllUsers = new AwaitableRelayCommand(ShowAllUsers, () => true);
        }

        private async Task ShowAllUsers()
        {
          var accounts = await _accountsRepository.GetAllUsers();
          var viewModel = _viewAccountModelFactory.Create(accounts);
          _windowService.ShowWindow<ViewAccountsWindow>(viewModel);
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
