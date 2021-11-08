using BankingGWService.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace BankingGWService.Services
{
    public class CustomerService
    {
        // private string customerApiUrl = "http://localhost:40001/api/customer/";
        private string customerApiUrl = "https://bankcustomerapi.azurewebsites.net/api/customer/";
        public async Task<CustomerDTO> Get(string id)
        {
            CustomerDTO customer = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(customerApiUrl);
                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer");
                var getTask = await client.GetAsync("getCustomerDetails/" + id);
                if (getTask.IsSuccessStatusCode)
                {
                    var data = getTask.Content.ReadFromJsonAsync<CustomerDTO>();
                    data.Wait();
                    customer = data.Result;
                }
            }
            return customer;
        }

        public async Task<CustomerCreationStatusDTO> Post([FromBody]CustomerDTO customer)
        {
            CustomerCreationStatusDTO status = null;
            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(customerApiUrl);
                var getTask = await client.PostAsJsonAsync("createCustomer", customer);
                if (getTask.IsSuccessStatusCode)
                {
                    var data = getTask.Content.ReadFromJsonAsync<CustomerCreationStatusDTO>();
                    data.Wait();
                    status = data.Result;
                }
            }
            return status;
        }
    }
}
