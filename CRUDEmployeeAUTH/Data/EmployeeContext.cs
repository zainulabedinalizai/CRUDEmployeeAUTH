using CrudEmployeeAUTH.Models;
using CRUDEmployeeAUTH.Models;
using Microsoft.EntityFrameworkCore;

namespace CRUDEmployeeAUTH.Data
{
    public class EmployeeContext : DbContext
    {
        public EmployeeContext(DbContextOptions<EmployeeContext> options) : base(options)
        {
        }
        public DbSet<Employee> Employees { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<EmployeeT> EmployeesT { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}