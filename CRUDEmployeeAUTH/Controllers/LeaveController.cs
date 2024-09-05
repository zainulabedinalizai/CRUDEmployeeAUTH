using CRUDEmployeeAUTH.ActionFilters;
using CRUDEmployeeAUTH.Dto;
using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class LeaveController : ControllerBase
{
    private readonly ILeaveRepository _leaveRepository;

    public LeaveController(ILeaveRepository leaveRepository)
    {
        _leaveRepository = leaveRepository;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetLeavesByUser(int userId)
    {
        var leaves = await _leaveRepository.GetLeavesByUserIdAsync(userId);
        return Ok(leaves);
    }

    [ServiceFilter(typeof(ReviewLeaveFilter))]
    [HttpPut("Review/{leaveId}")]
    public async Task<IActionResult> ReviewLeave(int leaveId, [FromQuery] string companyName, [FromBody] bool isApproved)
    {
        var result = await _leaveRepository.ApproveLeaveAsync(leaveId, isApproved);
        if (result == "Leave not found.")
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpPost("ToApplyLeave")]
    public async Task<IActionResult> ApplyLeave([FromBody] LeaveDTO leaveDto)
    {
        var result = await _leaveRepository.AddLeaveAsync(leaveDto);
        return Ok(result);
    }

    [HttpPut("{leaveId}")]
    public async Task<IActionResult> UpdateLeave(int leaveId, [FromBody] LeaveDTO leaveDto)
    {
        var result = await _leaveRepository.UpdateLeaveAsync(leaveId, leaveDto);
        if (result == "Leave not found.")
        {
            return NotFound(result);
        }
        return Ok(result);
    }

    [HttpDelete("{leaveId}")]
    public async Task<IActionResult> DeleteLeave(int leaveId)
    {
        var result = await _leaveRepository.DeleteLeaveAsync(leaveId);
        if (result == "Leave not found.")
        {
            return NotFound(result);
        }
        return Ok(result);
    }
}