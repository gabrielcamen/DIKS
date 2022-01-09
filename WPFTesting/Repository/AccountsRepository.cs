using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using Dapper;
using Npgsql;
using WPFTesting.Models;

namespace WPFTesting.Repository
{
    public interface IAccountsRepository
    {
        Task<List<Account>> GetAllUsers();
        void AddUser(Account newAccount);
    }
    
    public class AccountsRepository : IAccountsRepository
    {
        
        const string connString = "Host=localhost;Username=postgres;Password=1234;Database=DapperTestDB";
        public async Task<List<Account>> GetAllUsers()
        {
            await using var connection = new NpgsqlConnection(connString);
            connection.Open();
            var query = "Select * from accounts;";
            return connection.Query<Account>(query).ToList();
        }

        public void AddUser(Account newAccount)
        {
            using (var connection = new NpgsqlConnection(connString))
            {
                connection.Open();
                var query = "INSERT INTO accounts (username, password, email) VALUES (@Username, @Password, @Email);";
                var linesAffected = connection.Execute(query, newAccount);
                MessageBox.Show($"Affected lines: {linesAffected}");
            }
            
            
        }
    }
}
