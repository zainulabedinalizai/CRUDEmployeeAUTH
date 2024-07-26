using CrudEmployeeAUTH.Models;

namespace CRUDEmployeeAUTH.IRepositories
{
    public interface IUserRepository
    {
        Task<User> GetByUsernameAsync(string username);
        Task AddAsync(User user);
    }
}