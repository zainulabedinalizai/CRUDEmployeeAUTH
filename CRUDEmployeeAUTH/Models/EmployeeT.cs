using CrudEmployeeAUTH.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CRUDEmployeeAUTH.Models
{
    public class EmployeeT
    {
       // [Key]
        public int Id { get; set; }

        //[ForeignKey("EmployeeFKey")]
       // public int EmployeeId { get; set; }
        public string Name { get; set; }
        public string Position { get; set; }
        public string Salary { get; set; }
    }

    //public class EmployeeWithEmployeeT
    //{
    //    public int EmployeeId { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmployeePosition { get; set; }
    //    public string EmployeeSalary { get; set; }
    //    public int EmployeeTId { get; set; }
    //    public string EmployeeTName { get; set; }
    //    public string EmployeeTPosition { get; set; }
    //    public string EmployeeTSalary { get; set; }
    //}

    //public class EmployeeWithEmployeeTLeftJoin
    //{
    //    public int EmployeeId { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmployeePosition { get; set; }
    //    public string EmployeeSalary { get; set; }
    //    public int? EmployeeTId { get; set; }
    //    public string EmployeeTName { get; set; }
    //    public string EmployeeTPosition { get; set; }
    //    public string EmployeeTSalary { get; set; }
    //}

    //public class EmployeeTWithEmployeeRightJoin
    //{
    //    public int EmployeeTId { get; set; }
    //    public string EmployeeTName { get; set; }
    //    public string EmployeeTPosition { get; set; }
    //    public string EmployeeTSalary { get; set; }
    //    public int? EmployeeId { get; set; }
    //    public string EmployeeName { get; set; }
    //    public string EmployeePosition { get; set; }
    //    public string EmployeeSalary { get; set; }
    //}
}