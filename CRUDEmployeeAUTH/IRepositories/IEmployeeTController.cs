using CRUDEmployeeAUTH.Models;

namespace CRUDEmployeeAUTH.IRepositories
{
    public interface IEmployeTRepository
    {
        Task<IEnumerable<EmployeeT>> GetAllAsync();
        Task<EmployeeT> GetByIdAsync(int id);
        Task<EmployeeT> GetByIdAndNameAsync(int id, string name);
        Task AddAsync(EmployeeT employee);
        Task UpdateAsync(EmployeeT employee);
        Task DeleteAsync(int id);
    }
}