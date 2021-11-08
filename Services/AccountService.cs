using BankingGWService.Models;
using BankingGWService.Models.Account;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BankingGWService.Services
{
    public class AccountService
    {
        private string accountApi = "https://bankaccountapi.azurewebsites.net/api/account/";
        // private string accountApi = "http://localhost:41001/api/account/";
        public async Task<List<AccountDTO>> GetAccountsByCustIDAsync(string id)
        {
            List<AccountDTO> accounts = new List<AccountDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                var getTask = await client.GetAsync("getCustomerAccounts/" + id);
                if (getTask.IsSuccessStatusCode)
                {
                    var data = getTask.Content.ReadFromJsonAsync<List<AccountDTO>>();
                    data.Wait();
                    accounts = data.Result;
                }
            }
            return accounts;
        }

        public async Task<AccountDTO> GetAccount(int id)
        {
            AccountDTO account = new AccountDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                var getTask = await client.GetAsync("getAccount/" + id);
                if (getTask.IsSuccessStatusCode)
                {
                    account = await getTask.Content.ReadFromJsonAsync<AccountDTO>();
                }
            }
            return account;
        }

        public async Task<AccountCreationStatus> AddAccountAsync(AccountDTO account)
        {
            AccountCreationStatus creationStatus = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                var postTask = await client.PostAsJsonAsync("createAccount", account);
                if (postTask.IsSuccessStatusCode)
                {
                    creationStatus = await postTask.Content.ReadFromJsonAsync<AccountCreationStatus>();
                }
            }
            return creationStatus;
        }

        public async Task<List<StatementDTO>> GetStatements(int accId, DateTime fromDate, DateTime toDate)
        {
            List<StatementDTO> statements = new List<StatementDTO>();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                var getTask = await client.GetAsync($"getAccountStatement?accId={accId}&fromDate={fromDate}&toDate={toDate}");
                if (getTask.IsSuccessStatusCode)
                {
                    statements = await getTask.Content.ReadFromJsonAsync<List<StatementDTO>>();
                }
            }
            return statements;
        }
        public async Task<TransactionStatusDTO> Withdraw(int accId, int amount)
        {
            TransactionStatusDTO transactionStatus = new TransactionStatusDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                var postTask = await client.PostAsync($"withdraw?accId={accId}&amount={amount}", null);
                if (postTask.IsSuccessStatusCode)
                {
                    transactionStatus = await postTask.Content.ReadFromJsonAsync<TransactionStatusDTO>();
                }
            }
            return transactionStatus;
        }

        public async Task<TransactionStatusDTO> Deposit(int accId, int amount)
        {
            TransactionStatusDTO depositStatus = new TransactionStatusDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                var postTask = await client.PostAsync($"deposit?accId={accId}&amount={amount}", null);
                if (postTask.IsSuccessStatusCode)
                {
                    depositStatus = await postTask.Content.ReadFromJsonAsync<TransactionStatusDTO>();
                }
            }
            return depositStatus;
        }

        public async Task<TransactionStatusDTO> Transfer(int fromAccId, int toAccId, float amount)
        {
            TransactionStatusDTO transferStatus = new TransactionStatusDTO();
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(accountApi);
                var postTask = await client.PostAsync($"Transfer?fromAccId={fromAccId}&toAccId={toAccId}&amount={amount}", null);
                if (postTask.IsSuccessStatusCode)
                {
                    transferStatus = await postTask.Content.ReadFromJsonAsync<TransactionStatusDTO>();
                }
            }
            return transferStatus;
        }
    }
}
