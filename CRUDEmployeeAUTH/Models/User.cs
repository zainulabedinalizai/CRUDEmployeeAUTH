public class User
{
    public int UserId { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public string Role { get; set; }
    public int CompanyId { get; set; }
    public Company Company { get; set; }
    public ICollection<Leave> Leaves { get; set; }
    public ICollection<Attendance> Attendances { get; set; }
}
