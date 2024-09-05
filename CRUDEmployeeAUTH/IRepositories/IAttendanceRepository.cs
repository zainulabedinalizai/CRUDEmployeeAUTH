using CRUDEmployeeAUTH.Dto;
using System.Security.Claims;

namespace CRUDEmployeeAUTH.IRepositories
{
    public interface IAttendanceRepository
    {
        Task<IEnumerable<Attendance>> GetAllAttendancesAsync();
        Task<Attendance> GetAttendanceByIdAsync(int attendanceId);
        Task<IEnumerable<Attendance>> GetAttendancesByUserIdAsync(int userId);
        Task<Attendance> AddAttendanceAsync(AttendanceDTO attendanceDto, ClaimsPrincipal user);
        Task<Attendance> UpdateAttendanceAsync(int attendanceId, AttendanceDTO attendanceDto);
        Task<bool> DeleteAttendanceAsync(int attendanceId);
    }
}
