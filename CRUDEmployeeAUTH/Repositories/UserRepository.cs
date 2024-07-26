using CrudEmployeeAUTH.Models;
using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.EntityFrameworkCore;

namespace CrudEmployeeAUTH.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly EmployeeContext _context;

        public UserRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<User> GetByUsernameAsync(string username)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
        }

        public async Task AddAsync(User user)
        {
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
        }
    }
}