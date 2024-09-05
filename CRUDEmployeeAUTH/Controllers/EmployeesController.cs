using CrudEmployeeAUTH.Models;
using CRUDEmployeeAUTH.ActionFilters;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CrudEmployeeAUTH.Controllers
{
    [ServiceFilter(typeof(LoggingFilter))]
    [ServiceFilter(typeof(ExceptionHandlingFilter))]
    [ServiceFilter(typeof(ValidationFilter))]
    [ServiceFilter(typeof(ResultFilter))]
    [ServiceFilter(typeof(CacheFilter))]
    [Route("api/[controller]")]
    [ApiController]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository;
        }

        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            var employees = await _repository.GetAllAsync();
            return Ok(employees);
        }

        [Authorize(Policy = "CompanyAdminOrSystemAdmin")]
        [HttpGet("GetById")]
        public async Task<ActionResult<Employee>> GetById(int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            return Ok(employee);
        }

        [HttpGet("GetByIdAndName")]
        public async Task<ActionResult<Employee>> GetByIdAndName([FromQuery] int id, [FromQuery] string name)
        {
            var employee = await _repository.GetByIdAndNameAsync(id, name);
            return Ok(employee);
        }

        [HttpPost]
        public async Task<ActionResult> Add([FromBody] Employee employee)
        {
            await _repository.AddAsync(employee);
            return Ok(employee);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] Employee employee)
        {
            if (id != employee.Id)
            {
                return BadRequest("Employee ID mismatch.");
            }

            await _repository.UpdateAsync(employee);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
