public class Leave
{
    public int LeaveId { get; set; }
    public int UserId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public string Reason { get; set; }
    public bool IsApproved { get; set; }
    public User User { get; set; } 
}
