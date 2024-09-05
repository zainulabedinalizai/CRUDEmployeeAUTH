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
        public DbSet<Company> Companies { get; set; }
        public DbSet<Leave> Leaves { get; set; }
        public DbSet<Attendance> Attendances { get; set; }
        public DbSet<EmployeeT> EmployeesT { get; set; }  

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            modelBuilder.Entity<User>()
                .HasOne(u => u.Company)
                .WithMany(c => c.Users)
                .HasForeignKey(u => u.CompanyId);


            modelBuilder.Entity<Leave>()
                .HasOne(l => l.User)
                .WithMany(u => u.Leaves)
                .HasForeignKey(l => l.UserId);


           
            modelBuilder.Entity<Attendance>()
                .HasOne(a => a.User)
                .WithMany(u => u.Attendances)
                .HasForeignKey(a => a.UserId);  


            base.OnModelCreating(modelBuilder);
        }
    }
}
