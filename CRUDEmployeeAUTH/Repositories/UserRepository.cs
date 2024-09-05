using CRUDEmployeeAUTH.Data;
using Microsoft.EntityFrameworkCore;

public class UserRepository : IUserRepository
{
    private readonly EmployeeContext _context;

    public UserRepository(EmployeeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<User>> GetAllAsync()
    {
      
        return await _context.Users.ToListAsync();
    }

    public async Task<User> GetByIdAsync(int userId)
    {
       
        return await _context.Users.FindAsync(userId);
    }

    public async Task<IEnumerable<User>> GetByCompanyAsync(int companyId)
    {
       
        return await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();
    }

    public async Task<Company> GetCompanyByNameAsync(string companyName)
    {
       
        return await _context.Companies.FirstOrDefaultAsync(c => c.CompanyName == companyName);
    }

    public async Task<User> GetByUsernameAsync(string username)
    {
      
        return await _context.Users.FirstOrDefaultAsync(u => u.Email == username);
    }

    public async Task AddAsync(User user)
    {
        
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
    }

    public async Task AddCompanyAsync(Company company)
    {
        
        _context.Companies.Add(company);
        await _context.SaveChangesAsync();
    }


    public async Task<IEnumerable<User>> GetUsersByCompanyIdAsync(int companyId)
    {
        
        return await _context.Users.Where(u => u.CompanyId == companyId).ToListAsync();

        
    }
}
