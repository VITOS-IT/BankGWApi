using BankingGWService.Models;
using BankingGWService.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BankingGWService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class EmployeeController : ControllerBase
    {
        private readonly EmployeeService _servise;

        public EmployeeController(EmployeeService service)
        {
            _servise = service;
        }

        [HttpGet("/getEmployeeDetails/{id}")]
        public async Task<Employee> Get(string id)
        {
            return await _servise.Get(id);
        }

        [HttpPost("/createEmployee")]
        public async Task<Employee> Post(Employee customer)
        {
            return await _servise.Add(customer);
        }
    }
}
