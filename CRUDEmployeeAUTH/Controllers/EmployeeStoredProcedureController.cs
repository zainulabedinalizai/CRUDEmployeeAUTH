//using CRUDEmployeeAUTH.IRepositories;
//using CRUDEmployeeAUTH.Models;
//using Microsoft.AspNetCore.Mvc;

//namespace CRUDEmployeeAUTH.Controllers
//{
//    [Route("api/[controller]")]
//    [ApiController]
//    public class EmployeeStoredProcedureController : ControllerBase
//    {
//        private readonly IEmployeeStoredProcedureRepository _repository;

//        public EmployeeStoredProcedureController(IEmployeeStoredProcedureRepository repository)
//        {
//            _repository = repository;
//        }

//        [HttpGet("GetAllEmployees")]
//        public async Task<ActionResult<IEnumerable<EmployeeT>>> GetAllEmployees()
//        {
//            var employees = await _repository.GetAllEmployeesAsync();
//            return Ok(employees);
//        }

//        [HttpGet("GetEmployeeById")]
//        public async Task<ActionResult<EmployeeT>> GetEmployeeById([FromQuery] int id)
//        {
//            var employee = await _repository.GetEmployeeByIdAsync(id);
//            if (employee == null)
//            {
//                return NotFound();
//            }

//            return Ok(employee);
//        }

//        [HttpGet("GetEmployeeByIdAndName")]
//        public async Task<ActionResult<EmployeeT>> GetEmployeeByIdAndName([FromQuery] int id, [FromQuery] string name)
//        {
//            var employee = await _repository.GetEmployeeByIdAndNameAsync(id, name);
//            if (employee == null)
//            {
//                return NotFound();
//            }

//            return Ok(employee);
//        }

//        [HttpPost("AddEmployee")]
//        public async Task<ActionResult> AddEmployee([FromBody] EmployeeT employee)
//        {
//            await _repository.AddEmployeeAsync(employee);
//            return Ok();
//        }

//        [HttpPut("UpdateEmployee")]
//        public async Task<ActionResult> UpdateEmployee([FromBody] EmployeeT employee)
//        {
//            await _repository.UpdateEmployeeAsync(employee);
//            return Ok("Updated");
//        }

//        [HttpDelete("DeleteEmployee")]
//        public async Task<ActionResult> DeleteEmployee([FromQuery] int id)
//        {
//            await _repository.DeleteEmployeeAsync(id);
//            return NoContent();
//        }

//        //[HttpGet("Join")]
//        //public async Task<ActionResult<IEnumerable<EmployeeWithEmployeeT>>> JoinAsync()
//        //{
//        //    var result = await _repository.JoinAsync();
//        //    return Ok(result);
//        //}

//        //[HttpGet("LeftJoin")]
//        //public async Task<ActionResult<IEnumerable<EmployeeWithEmployeeTLeftJoin>>> LeftJoin()
//        //{
//        //    var result = await _repository.LeftJoin();
//        //    return Ok(result);
//        //}

//        //[HttpGet("RightJoin")]
//        //public async Task<ActionResult<IEnumerable<EmployeeTWithEmployeeRightJoin>>> RightJoin()
//        //{
//        //    var result = await _repository.RightJoin();
//        //    return Ok(result);
//        //}
//    }
//}