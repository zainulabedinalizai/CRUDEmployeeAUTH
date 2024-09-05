//using CRUDEmployeeAUTH.IRepositories;
//using CRUDEmployeeAUTH.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace CrudEmployeeAUTH.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeesTController : ControllerBase
//    {
//        private readonly IEmployeTRepository _repository;

//        public EmployeesTController(IEmployeTRepository repository)
//        {
//            _repository = repository;
//        }

//        [TypeFilter(typeof(RoleBasedAuthorizationFilter), Arguments = new object[] { "Merik" })]
//        [Route("GetAllEmployees")]
//        [HttpGet]
//        public async Task<ActionResult<IEnumerable<EmployeeT>>> GetAll()
//        {
//            var employees = await _repository.GetAllAsync();
//            return Ok(employees);
//        }

//        [TypeFilter(typeof(RoleBasedAuthorizationFilter), Arguments = new object[] { "Devsinc" })]
//        [HttpGet("GetByid")]
//        public async Task<ActionResult<EmployeeT>> GetById([FromQuery] int id)
//        {
//            var employee = await _repository.GetByIdAsync(id);
//            if (employee == null)
//            {
//                return NotFound();
//            }

//            return Ok(employee);
//        }

//        [HttpGet("{name}/zainapi/{id}")]
//        public async Task<ActionResult<EmployeeT>> GetByIdAndNameAsync([FromRoute] int id, [FromRoute] string name)
//        {
//            var employee = await _repository.GetByIdAsync(id);
//            if (employee == null)
//            {
//                return NotFound();
//            }
//            if (!string.Equals(employee.Name, name))
//            {
//                return BadRequest("Invalid Employee Name.");
//            }

//            return Ok(employee);
//        }

//        [HttpPost]
//        public async Task<ActionResult> Add([FromBody] EmployeeT employee)
//        {
//            await _repository.AddAsync(employee);
//            return Ok(employee);
//        }

//        [HttpPut("{id}")]
//        public async Task<ActionResult> Update(int id, EmployeeT employee)
//        {
//            if (id != employee.Id)
//            {
//                return BadRequest();
//            }

//            await _repository.UpdateAsync(employee);
//            return NoContent();
//        }

//        [HttpDelete("{id}")]
//        public async Task<ActionResult> Delete(int id)
//        {
//            await _repository.DeleteAsync(id);
//            return NoContent();
//        }

//    }
//}