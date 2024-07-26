using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.IRepositories;
using CRUDEmployeeAUTH.Models;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace CRUDEmployeeAUTH.Repositories
{
    public class EmployeeStoredProcedureRepository : IEmployeeStoredProcedureRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeStoredProcedureRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeT>> GetAllEmployeesAsync()
        {
            return await _context.EmployeesT.FromSqlRaw("EXEC GetAllEmployees").ToListAsync();
        }

        public async Task<EmployeeT> GetEmployeeByIdAsync(int id)
        {
            return await _context.EmployeesT.FromSqlRaw("EXEC GetEmployeeById @Id = {0}", id).FirstOrDefaultAsync();
        }

        public async Task<EmployeeT> GetEmployeeByIdAndNameAsync(int id, string name)
        {
            return await _context.EmployeesT.FromSqlRaw("EXEC GetEmployeeByIdAndName @Id = {0}, @Name = {1}", id, name).FirstOrDefaultAsync();
        }

        public async Task AddEmployeeAsync(EmployeeT employee)
        {
            SqlParameter[] parameters =
            {
                new ("@Name", employee.Name),
                new ("@Position", employee.Position),
                new ("@Salary", employee.Salary)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC AddEmployee @Name, @Position, @Salary", parameters);
        }

        public async Task UpdateEmployeeAsync(EmployeeT employee)
        {
            SqlParameter[] parameters =
            {
                new ("@Id", employee.Id),
                new ("@Name", employee.Name),
                new ("@Position", employee.Position),
                new ("@Salary", employee.Salary)
            };

            await _context.Database.ExecuteSqlRawAsync("EXEC UpdateEmployee @Id, @Name, @Position, @Salary", parameters);
        }

        public async Task DeleteEmployeeAsync(int id)
        {
            SqlParameter parameter = new SqlParameter("@Id", id);

            await _context.Database.ExecuteSqlRawAsync("EXEC DeleteEmployee @Id", parameter);
        }

        //    public async Task<IEnumerable<EmployeeWithEmployeeT>> JoinAsync()
        //    {
        //        return await (from employee in _context.Employees
        //                      join employeeT in _context.EmployeesT
        //                      on employee.Id equals employeeT.EmployeeId
        //                      select new EmployeeWithEmployeeT
        //                      {
        //                          EmployeeId = employee.Id,
        //                          EmployeeName = employee.Name,
        //                          EmployeePosition = employee.Position,
        //                          EmployeeSalary = employee.Salary,
        //                          EmployeeTId = employeeT.Id,
        //                          EmployeeTName = employeeT.Name,
        //                          EmployeeTPosition = employeeT.Position,
        //                          EmployeeTSalary = employeeT.Salary
        //                      }).ToListAsync();
        //    }

        //    public async Task<IEnumerable<EmployeeWithEmployeeTLeftJoin>> LeftJoin()
        //    {
        //        return await (from employee in _context.Employees
        //                      join employeeT in _context.EmployeesT
        //                      on employee.Id equals employeeT.EmployeeId into employeeTs
        //                      from employeeT in employeeTs.DefaultIfEmpty()
        //                      select new EmployeeWithEmployeeTLeftJoin
        //                      {
        //                          EmployeeId = employee.Id,
        //                          EmployeeName = employee.Name,
        //                          EmployeePosition = employee.Position,
        //                          EmployeeSalary = employee.Salary,
        //                          EmployeeTId = employeeT != null ? employeeT.Id : (int?)null,
        //                          EmployeeTName = employeeT != null ? employeeT.Name : null,
        //                          EmployeeTPosition = employeeT != null ? employeeT.Position : null,
        //                          EmployeeTSalary = employeeT != null ? employeeT.Salary : null
        //                      }).ToListAsync();
        //    }

        //    public async Task<IEnumerable<EmployeeTWithEmployeeRightJoin>> RightJoin()
        //    {
        //        return await (from employeeT in _context.EmployeesT
        //                      join employee in _context.Employees
        //                      on employeeT.EmployeeId equals employee.Id into employees
        //                      from employee in employees.DefaultIfEmpty()
        //                      select new EmployeeTWithEmployeeRightJoin
        //                      {
        //                          EmployeeTId = employeeT.Id,
        //                          EmployeeTName = employeeT.Name,
        //                          EmployeeTPosition = employeeT.Position,
        //                          EmployeeTSalary = employeeT.Salary,
        //                          EmployeeId = employee != null ? employee.Id : (int?)null,
        //                          EmployeeName = employee != null ? employee.Name : null,
        //                          EmployeePosition = employee != null ? employee.Position : null,
        //                          EmployeeSalary = employee != null ? employee.Salary : null
        //                      }).ToListAsync();
        //    }
        //}
    }
}