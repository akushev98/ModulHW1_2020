using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.Services
{
    public interface ITransactionService
    {
        bool Transfer(TransferParametersDTO param, string email);
        AccountNumber CreateAccountNumber(string value);
        void TopUp(string value, TopUpParametersDTO param);
        AccountNumberDTO GetAccountHolder(string accountnumber, string email);
    }
}