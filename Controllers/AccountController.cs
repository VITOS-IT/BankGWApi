using BankingGWService.Models;
using BankingGWService.Models.Account;
using BankingGWService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BankingGWService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _service;

        public AccountController(AccountService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public async Task<AccountDTO> GetAccount(int id)
        {
            return await _service.GetAccount(id);
        }

        [HttpPost]
        public async Task<AccountCreationStatus> Post([FromBody] AccountDTO account)
        {
            return  await _service.AddAccountAsync(account);
        }

        [HttpGet("GetCustomerAccounts/{id}")]
        public async Task<List<AccountDTO>> GetCustomerAccounts(string id)
        {
            return await _service.GetAccountsByCustIDAsync(id);
        }

        [HttpGet("getAccountStatement")]
        public async Task<IEnumerable<StatementDTO>> Transactions(int accId, DateTime fromDate, DateTime toDate)
        {
            return await _service.GetStatements(accId, fromDate, toDate);
        }

        [HttpPost("Withdraw")]
        public async Task<TransactionStatusDTO> Withdraw(int accId, int amount)
        {
            return await _service.Withdraw(accId, amount);
        }

        [HttpPost("Deposit")]
        public async Task<TransactionStatusDTO> Deposit(int accId, int amount)
        {
            return await _service.Deposit(accId, amount);
        }

        [HttpPost("Transfer")]
        public async Task<TransactionStatusDTO> Transfer(int fromAccId, int toAccId, int amount)
        {
            return await _service.Transfer(fromAccId, toAccId, amount);
        }
    }
}
