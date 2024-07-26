using CrudEmployeeAUTH.Models;

namespace CRUDEmployeeAUTH.IRepositories
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Employee>> GetAllAsync();
        Task <Employee> GetByIdAsync(int id);
        Task<Employee> GetByIdAndNameAsync(int id, string name);
        Task AddAsync(Employee employee);
        Task UpdateAsync(Employee employee);
        Task DeleteAsync(int id);
    }
}