using CRUDEmployeeAUTH.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CRUDEmployeeAUTH.IRepositories
{
    public interface IEmployeeStoredProcedureRepository
    {
        Task<IEnumerable<EmployeeT>> GetAllEmployeesAsync();
        Task<EmployeeT> GetEmployeeByIdAsync(int id);
        Task<EmployeeT> GetEmployeeByIdAndNameAsync(int id, string name);
        Task AddEmployeeAsync(EmployeeT employee);
        Task UpdateEmployeeAsync(EmployeeT employee);
        Task DeleteEmployeeAsync(int id);

        //Task<IEnumerable<EmployeeWithEmployeeT>> JoinAsync();
        //Task<IEnumerable<EmployeeWithEmployeeTLeftJoin>> LeftJoin();
        //Task<IEnumerable<EmployeeTWithEmployeeRightJoin>> RightJoin();
   } 
}