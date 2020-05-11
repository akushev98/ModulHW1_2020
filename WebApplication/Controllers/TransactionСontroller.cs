using System;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApplication.DTO;
using WebApplication.Models;
using WebApplication.Services;

namespace WebApplication.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TransactionController : Controller
    {
        private static ITransactionService _repository;

        public TransactionController(ITransactionService iTransactionService)
        {
            _repository = iTransactionService;
        }

        [HttpPost("create")]
        public AccountNumber CreateNewAccountNumber()
        {
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            return _repository.CreateAccountNumber(email);
        }

        [HttpPost("topup")]
        public IActionResult TopUp([FromBody] TopUpParametersDTO param)
        {
            if (param.Amount <= 0)
            {
                return new BadRequestObjectResult("Incorrect amount");
            }
            
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            _repository.TopUp(email, param);
            
            AccountNumberDTO accountNumberDto = _repository.GetAccountHolder(param.Number, email);
            
            if (accountNumberDto.Number.Equals("Access denied"))
            {
                return new BadRequestObjectResult("Access denied");
            }
            
            if (param.Amount <= 0)
            {
                return new BadRequestObjectResult("Incorrect amount");
            }

            return new OkObjectResult(new
            {
                accountNumberDto.Number,
                accountNumberDto.AccountHolderEmail,
                accountNumberDto.Balance,
            });
        }

        [HttpPost("transfer")]
        public IActionResult Transfer([FromBody] TransferParametersDTO param)
        {
            if (param.Amount <= 0)
            {
                return new BadRequestObjectResult("Incorrect amount");
            }
            
            var email = User.FindFirst(ClaimTypes.NameIdentifier).Value;
            bool isEnoughMoney = _repository.Transfer(param, email);

            if (!isEnoughMoney)
            {
                return new BadRequestObjectResult("Not enough money in the account");
            }

            AccountNumberDTO accountNumberDto = _repository.GetAccountHolder(param.OwnAccount, email);
            
            if (accountNumberDto.Number.Equals("Access denied"))
            {
                return new BadRequestObjectResult("Access denied");
            }

            return new OkObjectResult(new
            {
                accountNumberDto.Number,
                accountNumberDto.AccountHolderEmail,
                accountNumberDto.Balance,
            });
        }
    }
}