
public interface IUserRepository
{
    Task<Company> GetCompanyByNameAsync(string companyName);
    Task<IEnumerable<User>> GetByCompanyAsync(int companyId); 
    Task<IEnumerable<User>> GetAllAsync(); 
    Task<User> GetByIdAsync(int userId);
    Task<User> GetByUsernameAsync(string username);
    Task AddAsync(User user);
    Task AddCompanyAsync(Company company);
}