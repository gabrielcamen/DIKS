using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using WPFTesting.Annotations;
using WPFTesting.Commands;
using WPFTesting.Models;
using WPFTesting.Repository;

namespace WPFTesting.ViewModels
{
    public interface IViewAccountsViewModelFactory
    {
        IViewAccountsViewModel Create(List<Account> accounts);
    }
    
    public interface IViewAccountsViewModel
    {
        public List<Account> Accounts { get; set; }
        
        ICommand AddUserCommand { get; }
        
        public Account NewAccount { get; set; }
    }
    
    public sealed class ViewAccountsViewModel : IViewAccountsViewModel, INotifyPropertyChanged
    {
        private readonly IAccountsRepository _accountsRepository;

        private List<Account> _accounts;
        public List<Account> Accounts
        {
            get => _accounts;
            set
            {
                _accounts = value;
                OnPropertyChanged(nameof(Accounts));
            }
        }

        public ViewAccountsViewModel(List<Account> accounts, 
            IAccountsRepository accountsRepository)
        {
            Accounts = accounts;
            _accountsRepository = accountsRepository;
            InitializeCommands();
        }

        private void InitializeCommands()
        {
            AddUserCommand = new AwaitableRelayCommand(OnAddUsers, () => true);
        }

        private async Task OnAddUsers()
        {
            if(NewAccount.Email == null || NewAccount.Password == null || NewAccount.Username == null)
                throw new Exception("User is missing values");

            _accountsRepository.AddUser(NewAccount);
            NewAccount = new Account();
            await RefreshUsers();
        }

        private async Task RefreshUsers()
        {
            Accounts = await _accountsRepository.GetAllUsers();
        }

        public ICommand AddUserCommand { get; private set; }

        public Account NewAccount { get; set; } = new();
        
        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
