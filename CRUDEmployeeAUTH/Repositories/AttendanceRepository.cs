using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.Dto;
using CRUDEmployeeAUTH.IRepositories;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace CRUDEmployeeAUTH.Repositories
{
    public class AttendanceRepository : IAttendanceRepository
    {
        private readonly EmployeeContext _context;

        public AttendanceRepository(EmployeeContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Attendance>> GetAllAttendancesAsync()
        {
            return await _context.Attendances.Include(a => a.User).ToListAsync();
        }

        public async Task<Attendance> GetAttendanceByIdAsync(int attendanceId)
        {
            return await _context.Attendances.Include(a => a.User).FirstOrDefaultAsync(a => a.AttendanceId == attendanceId);
        }

        public async Task<IEnumerable<Attendance>> GetAttendancesByUserIdAsync(int userId)
        {
            return await _context.Attendances.Where(a => a.UserId == userId).ToListAsync();
        }

        public async Task<Attendance> AddAttendanceAsync(AttendanceDTO attendanceDto, ClaimsPrincipal user)
        {
            var userIdFromTokenClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(userIdFromTokenClaim, out int userIdFromToken))
            {
                throw new UnauthorizedAccessException("User ID claim is missing or invalid in the token.");
            }

            if (userIdFromToken != attendanceDto.UserId)
            {
                throw new UnauthorizedAccessException("User ID in the token does not match the user ID in the attendance data.");
            }

            var attendance = new Attendance
            {
                UserId = attendanceDto.UserId,
                Date = attendanceDto.Date,
                IsPresent = attendanceDto.IsPresent
            };

            _context.Attendances.Add(attendance);
            await _context.SaveChangesAsync();
            return attendance;
        }

        public async Task<Attendance> UpdateAttendanceAsync(int attendanceId, AttendanceDTO attendanceDto)
        {
            var existingAttendance = await _context.Attendances.FindAsync(attendanceId);
            if (existingAttendance != null)
            {
                existingAttendance.UserId = attendanceDto.UserId;
                existingAttendance.Date = attendanceDto.Date;
                existingAttendance.IsPresent = attendanceDto.IsPresent;
                await _context.SaveChangesAsync();
            }
            return existingAttendance;
        }

        public async Task<bool> DeleteAttendanceAsync(int attendanceId)
        {
            var attendance = await _context.Attendances.FindAsync(attendanceId);
            if (attendance != null)
            {
                _context.Attendances.Remove(attendance);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
    }
}
