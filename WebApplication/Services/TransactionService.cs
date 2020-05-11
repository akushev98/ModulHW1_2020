using System;
using System.Linq;
using Dapper;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Services
{
    public class TransactionService : ITransactionService
    {
        public AccountNumberDTO GetAccountHolder(string accountnumber, string email)
        {
            AccountNumberDTO accountNumber = new AccountNumberDTO();
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QuerySingleOrDefault<AccountNumberDTO>(
                    "SELECT number, accountholderemail, balance FROM accountnumbers WHERE number = @accountnumber;",
                    new
                    {
                        accountnumber
                    });
                
                if (!(result?.AccountHolderEmail.Equals(email) ?? false))
                {
                    result = new AccountNumberDTO();
                    result.AccountHolderEmail = "Access denied";
                    result.Balance = 0;
                    result.Number = "Access denied";
                }
                return result;
            }
        }
        
        

        private AccountNumberDTO GetAccount(string accountnumber)
        {
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();
                var result = conn.QuerySingleOrDefault<AccountNumberDTO>(
                    "SELECT number, accountholderemail, balance FROM accountnumbers WHERE number = @accountnumber;",
                    new
                    {
                        accountnumber
                    });
                return result;
            }
        }

        public bool Transfer(TransferParametersDTO param, string email)
        {
            var accountNumber = GetAccountHolder(param.OwnAccount, email);

            if (GetAccount(param.OwnAccount).Balance < param.Amount)
                return false;

            if (accountNumber.AccountHolderEmail == email)
            {
                using (var conn = Connection.CreateConnection())
                {
                    conn.Open();
                    conn.Execute("UPDATE accountnumbers SET balance = @balance WHERE number = @number;",
                        new
                        {
                            balance = accountNumber.Balance - param.Amount,
                            number = param.OwnAccount
                        });
                    conn.Execute("UPDATE accountnumbers SET balance = @balance WHERE number = @number;",
                        new
                        {
                            balance = GetAccount(param.PayeeAccount).Balance + param.Amount,
                            number = param.PayeeAccount
                        });
                }
            }
            return true;
        }

        public void TopUp(string email, TopUpParametersDTO param)
        {
            AccountNumberDTO accountNumber = GetAccountHolder(param.Number, email);
            if (accountNumber.AccountHolderEmail == email)
            {
                using (var conn = Connection.CreateConnection())
                {
                    conn.Open();
                    conn.Execute("UPDATE accountnumbers SET balance = @balance WHERE number = @number;",
                        new
                        {
                            balance = param.Amount + accountNumber.Balance,
                            number = param.Number
                        });
                }
            }
        }

        public AccountNumber CreateAccountNumber(string value)
        {
            var employee = new AccountNumber(value);
            using (var conn = Connection.CreateConnection())
            {
                conn.Open();

                conn.Execute(
                    "INSERT INTO accountnumbers (number, accountholderemail, balance) VALUES(@number, @accountholderemail, @balance);",
                    new
                    {
                        number = employee.Number,
                        accountholderemail = employee.AccountHolderEmail,
                        balance = employee.Balance,
                    });
            }

            return employee;
        }
    }
}