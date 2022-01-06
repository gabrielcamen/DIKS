using System.Collections.Generic;
using WPFTesting.Models;

namespace WPFTesting.ViewModels
{
    public interface IViewAccountsViewModelFactory
    {
        IViewAccountsViewModel Create(List<Account> accounts);
    }
    
    public interface IViewAccountsViewModel
    {
        public List<Account> Accounts { get; set; }
    }
    
    public class ViewAccountsViewModel : IViewAccountsViewModel
    {
        public List<Account> Accounts { get; set; }

        public ViewAccountsViewModel(List<Account> accounts)
        {
            Accounts = accounts;
        }
    }
}
