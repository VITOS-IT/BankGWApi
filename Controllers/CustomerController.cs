using BankingGWService.Models;
using BankingGWService.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace BankingGWService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("MyPolicy")]
    [Authorize]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _servise;

        public CustomerController(CustomerService service)
        {
            _servise = service;
        }

        [HttpGet("/getCustomerDetails/{id}")]
        public async Task<CustomerDTO> Get(string id)
        {
            return await _servise.Get(id);
        }

        [HttpPost("/createCustomer")]
        public async Task<CustomerCreationStatusDTO> Post(CustomerDTO customer)
        {
            return await _servise.Post(customer);
        }
    }
}
