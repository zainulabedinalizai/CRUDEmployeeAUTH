using CRUDEmployeeAUTH.Dto;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class AttendanceController : ControllerBase
{
    private readonly IAttendanceRepository _attendanceRepository;

    public AttendanceController(IAttendanceRepository attendanceRepository)
    {
        _attendanceRepository = attendanceRepository;
    }

    [HttpGet("user/{userId}")]
    public async Task<IActionResult> GetAttendancesByUser(int userId)
    {
        var attendances = await _attendanceRepository.GetAttendancesByUserIdAsync(userId);
        return Ok(attendances);
    }

    [HttpPost]
    public async Task<IActionResult> MarkAttendance([FromBody] AttendanceDTO attendanceDto)
    {
        var result = await _attendanceRepository.AddAttendanceAsync(attendanceDto, User);
        return Ok("Attendance marked successfully.");
    }

    [HttpPut("{attendanceId}")]
    public async Task<IActionResult> UpdateAttendance(int attendanceId, [FromBody] AttendanceDTO attendanceDto)
    {
        var updatedAttendance = await _attendanceRepository.UpdateAttendanceAsync(attendanceId, attendanceDto);
        if (updatedAttendance == null)
        {
            return NotFound("Attendance not found.");
        }
        return Ok("Attendance updated successfully.");
    }

    [HttpDelete("{attendanceId}")]
    public async Task<IActionResult> DeleteAttendance(int attendanceId)
    {
        var result = await _attendanceRepository.DeleteAttendanceAsync(attendanceId);
        if (!result)
        {
            return NotFound("Attendance not found.");
        }
        return Ok("Attendance deleted successfully.");
    }
}