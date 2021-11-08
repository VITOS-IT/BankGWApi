using Microsoft.EntityFrameworkCore;

namespace BankingGWService.Models
{
    public class GwContext:DbContext
    {
        public GwContext(DbContextOptions options):base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>().HasData(
                new Employee() { Email = "mi@rash", Address="Ukraine", Credentials="SuperUser", Name="Misha", Phone ="+380994926006", Possition="free", Role = "Employee"}
                );
        }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
    }
}
