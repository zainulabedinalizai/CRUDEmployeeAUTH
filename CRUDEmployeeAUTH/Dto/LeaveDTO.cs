﻿namespace CRUDEmployeeAUTH.Dto
{
    public class LeaveDTO
    {
        public int LeaveId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string Reason { get; set; }
       
    }
}