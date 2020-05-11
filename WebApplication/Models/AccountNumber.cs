using System;
using Dapper;
using WebApplication.DTO;
using WebApplication.Services;

namespace WebApplication.Models
{
    public class AccountNumber
    {
        public string Number { get; }
        public decimal Balance { get; }
        public string AccountHolderEmail { get; }

        public AccountNumber(string accountHolderEmail)
        {
            AccountHolderEmail = accountHolderEmail;
            Balance = 0;
            Number = GetAccountNumber();
        }

        private static string GetAccountNumber()
        {
            Random rnd = new Random();
            string newNum;
            while (true)
            {
                newNum = (4 * Math.Pow(10, 9) + rnd.NextDouble() * Math.Pow(10, 9)).ToString().Split(',')[0];
                if (CheckUniqueNumber(newNum))
                {
                    break;
                }
            }
            return newNum;
        }

        public static bool CheckUniqueNumber(string newNum)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QuerySingleOrDefault<AccountNumberDTO>(
                    "SELECT number, accountholderemail, balance FROM accountnumbers WHERE number = @newNum;",
                    new
                    {
                        newNum
                    });
                if (result == null)
                    return true;
                return false;
            }
        }
    }
}