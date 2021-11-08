using BankingGWService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace BankingGWService.Services
{
    public class EmployeeService
    {
        private readonly GwContext _context;
        public EmployeeService(DbContextOptions<GwContext> options)
        {
            _context = new GwContext(options);
        }

        public async Task<Employee> Add(Employee employee)
        {
            try
            {
                Employee empl = await _context.Employees.FirstOrDefaultAsync(p => p.Email == employee.Email);
                if (empl == null)
                {
                    var entry = await _context.Employees.AddAsync(employee);
                    await _context.SaveChangesAsync();
                    return entry.Entity;
                }
            }
            catch (DbUpdateConcurrencyException Dbce)
            {
                Console.WriteLine(Dbce.Message);
            }
            catch (DbUpdateException Dbe)
            {
                Console.WriteLine(Dbe.Message);
            }
            return null;
        }

        public async Task<Employee> Get(string email)
        {
            return await _context.Employees.FirstOrDefaultAsync(u => u.Email == email);
        }
    }
}
