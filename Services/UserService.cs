using BankingGWService.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace BankingGWService.Services
{
    public class UserService : IUserRepo<UserDTO>
    {
        private readonly GwContext _context;
        private readonly ITokenService _tokenService;

        public UserService(DbContextOptions<GwContext> options, ITokenService tokenService)
        {
            _tokenService = tokenService;
            _context = new GwContext(options);
        }

        public async Task<UserDTO> RegisterAsync(UserDTO userDTO)
        {
            try
            {
                var customer = new CustomerDTO();
                var employee = new Employee();              

                if (await _context.Employees.Where(emp => emp.Email == userDTO.Email).FirstOrDefaultAsync() != null)
                {
                    employee = await _context.Employees.Where(emp => emp.Email == userDTO.Email).FirstOrDefaultAsync();
                    using var hmac = new HMACSHA512();
                    var user = new User()
                    {
                        Email = userDTO.Email,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                        PasswordSalt = hmac.Key,
                        Name = employee.Name,
                        Role = "Employee"
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    userDTO.JwtToken = _tokenService.CreateToken(userDTO);
                    userDTO.Password = "";
                    return userDTO;
                }
                else if (await new CustomerService().Get(userDTO.Email) != null)
                {
                    customer = await new CustomerService().Get(userDTO.Email);
                    using var hmac = new HMACSHA512();
                    var user = new User()
                    {
                        Email = userDTO.Email,
                        Name = customer.Name,
                        PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(userDTO.Password)),
                        PasswordSalt = hmac.Key,
                        Role = "Customer"
                    };
                    await _context.Users.AddAsync(user);
                    await _context.SaveChangesAsync();
                    userDTO.JwtToken = _tokenService.CreateToken(userDTO);
                    userDTO.Password = "";
                    return userDTO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<UserDTO> LoginAsync(LoginDTO loginDTO)
        {
            try
            {
                var myUser = await _context.Users.SingleOrDefaultAsync(u => u.Email == loginDTO.Email);
                if (myUser != null)
                {
                    using var hmac = new HMACSHA512(myUser.PasswordSalt);
                    var userPass = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDTO.Password));
                    for (int i = 0; i < userPass.Length; i++)
                    {
                        if (userPass[i] != myUser.PasswordHash[i])
                        {
                            return null;
                        }
                    }
                    UserDTO userDTO = new() { Email = myUser.Email, Password = "", Role = myUser.Role, Name = myUser.Name};
                    userDTO.JwtToken = _tokenService.CreateToken(userDTO);
                    return userDTO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }

        public async Task<UserDTO> Get(string email)
        {
            try
            {
                UserDTO userDTO = new();
                var user = await _context.Users.FindAsync(email);
                if (user != null)
                {
                    userDTO.Email = user.Email;
                    userDTO.Role = user.Role;
                    return userDTO;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return null;
        }
    }
}
