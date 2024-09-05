public class Attendance
{
    public int AttendanceId { get; set; }
    public int UserId { get; set; }
    public DateTime Date { get; set; }
    public bool IsPresent { get; set; }
    public User User { get; set; }
}
