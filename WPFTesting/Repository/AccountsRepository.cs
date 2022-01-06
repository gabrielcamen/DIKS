using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Npgsql;
using WPFTesting.Models;

namespace WPFTesting.Repository
{
    public interface IAccountsRepository
    {
        Task<List<Account>> GetAllUsers();
    }
    
    public class AccountsRepository : IAccountsRepository
    {
        
        const string connString = "Host=localhost;Username=postgres;Password=1234;Database=DapperTestDB";
        public async Task<List<Account>> GetAllUsers()
        {
            
            await using var connection = new NpgsqlConnection(connString);
            connection.Open();
            var query = "Select * from accounts;";
            var storedProc = "dbo.accounts_getall";
            return connection.Query<Account>(storedProc).ToList();
            
        }
    }
}
