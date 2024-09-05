namespace CRUDEmployeeAUTH.Dto
{
    public class AttendanceDTO
    {
        public int AttendanceId { get; set; }
        public int UserId { get; set; }
        public DateTime Date { get; set; }
        public bool IsPresent { get; set; }
    }
}