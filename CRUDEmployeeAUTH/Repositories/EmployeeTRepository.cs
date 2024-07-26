using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.IRepositories;
using CRUDEmployeeAUTH.Models;
using Microsoft.EntityFrameworkCore;

namespace CrudEmployeeAUTH.Repositories
{
    public class EmployeeTRepository : IEmployeTRepository
    {
        private readonly EmployeeContext _context;

        public EmployeeTRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<EmployeeT>> GetAllAsync()
        {
            return await _context.EmployeesT.ToListAsync();
        }


        public async Task<EmployeeT> GetByIdAsync(int id)
        {
            return await _context.EmployeesT.FindAsync(id);
        }

        public async Task<EmployeeT> GetByIdAndNameAsync(int id, string name)
        {
            return await _context.EmployeesT.FindAsync(id, name);

        }


        public async Task AddAsync(EmployeeT employee)
        {
            await _context.EmployeesT.AddAsync(employee);
            await _context.SaveChangesAsync();
        }


        public async Task UpdateAsync(EmployeeT employee)
        {
            _context.EmployeesT.Update(employee);
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var employee = await _context.EmployeesT.FindAsync(id);
            if (employee != null)
            {
                _context.EmployeesT.Remove(employee);
                await _context.SaveChangesAsync();
            }
        }
    }
}
