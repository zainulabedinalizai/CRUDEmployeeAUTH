using CRUDEmployeeAUTH.Data;
using CRUDEmployeeAUTH.Dto;
using Microsoft.EntityFrameworkCore;

public class LeaveRepository : ILeaveRepository
{
    private readonly EmployeeContext _context;

    public LeaveRepository(EmployeeContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Leave>> GetAllLeavesAsync()
    {
        return await _context.Leaves.Include(l => l.User).ToListAsync();
    }

    public async Task<Leave> GetLeaveByIdAsync(int leaveId)
    {
        return await _context.Leaves.Include(l => l.User).FirstOrDefaultAsync(l => l.LeaveId == leaveId);
    }

    public async Task<IEnumerable<Leave>> GetLeavesByUserIdAsync(int userId)
    {
        return await _context.Leaves.Where(l => l.UserId == userId).ToListAsync();
    }

    public async Task<string> AddLeaveAsync(LeaveDTO leaveDto)
    {
        var leave = new Leave
        {
            UserId = leaveDto.UserId,
            StartDate = leaveDto.StartDate,
            EndDate = leaveDto.EndDate,
            Reason = leaveDto.Reason,
            User = await _context.Users.FindAsync(leaveDto.UserId)
        };

        _context.Leaves.Add(leave);
        await _context.SaveChangesAsync();
        return "Leave applied successfully.";
    }

    public async Task<string> UpdateLeaveAsync(int leaveId, LeaveDTO leaveDto)
    {
        var existingLeave = await _context.Leaves.FindAsync(leaveId);
        if (existingLeave == null)
        {
            return "Leave not found.";
        }

        existingLeave.StartDate = leaveDto.StartDate;
        existingLeave.EndDate = leaveDto.EndDate;
        existingLeave.Reason = leaveDto.Reason;
        await _context.SaveChangesAsync();
        return "Leave updated successfully.";
    }

    public async Task<string> DeleteLeaveAsync(int leaveId)
    {
        var leave = await _context.Leaves.FindAsync(leaveId);
        if (leave == null)
        {
            return "Leave not found.";
        }

        _context.Leaves.Remove(leave);
        await _context.SaveChangesAsync();
        return "Leave deleted successfully.";
    }

    public async Task<string> ApproveLeaveAsync(int leaveId, bool isApproved)
    {
        var leave = await _context.Leaves.FindAsync(leaveId);
        if (leave == null)
        {
            return "Leave not found.";
        }

        leave.IsApproved = isApproved;
        await _context.SaveChangesAsync();
        return isApproved ? "Leave approved successfully." : "Leave rejected successfully.";
    }
}
