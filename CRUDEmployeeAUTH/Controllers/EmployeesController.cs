using CrudEmployeeAUTH.Models;
using CRUDEmployeeAUTH.ActionFilters;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.AspNetCore.Mvc;

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



        [TypeFilter(typeof(RoleBasedAuthorizationFilter), Arguments = new object[] { "Merik" })]
        [Route("GetAllEmployees")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Employee>>> GetAll()
        {
            var employees = await _repository.GetAllAsync();
            return Ok(employees);
        }

        [HttpGet("GetById")]
        public async Task<ActionResult<Employee>> GetById( int id)
        {
            var employee = await _repository.GetByIdAsync(id);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }



        [HttpGet("GetByIdAndName")]
        public async Task<ActionResult<Employee>> GetByIdAndName([FromQuery] int id, [FromQuery] string name)
        {
            var employee = await _repository.GetByIdAndNameAsync(id, name);
            if (employee == null)
            {
                return NotFound();
            }
            return Ok(employee);
        }


        [HttpPost]
            public async Task<ActionResult> Add([FromBody] Employee employee)
            {
                await _repository.AddAsync(employee);
                return CreatedAtAction(nameof(GetById), new { id = employee.Id }, employee);
            }

            [HttpPut("{id}")]
            public async Task<ActionResult> Update(int id, Employee employee)
            {
                if (id != employee.Id)
                {
                    return BadRequest();
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
