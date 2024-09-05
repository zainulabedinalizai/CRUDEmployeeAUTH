using CRUDEmployeeAUTH.Dto;

public interface ILeaveRepository
{
    Task<IEnumerable<Leave>> GetAllLeavesAsync();
    Task<Leave> GetLeaveByIdAsync(int leaveId);
    Task<IEnumerable<Leave>> GetLeavesByUserIdAsync(int userId);
    Task<string> AddLeaveAsync(LeaveDTO leaveDto);
    Task<string> UpdateLeaveAsync(int leaveId, LeaveDTO leaveDto);
    Task<string> DeleteLeaveAsync(int leaveId);
    Task<string> ApproveLeaveAsync(int leaveId, bool isApproved);
}
