using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdlcReportViewer.Models
{
    public class Employees
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public int PositionId { get; set; }
        public int OrganizationId { get; set; }
        public int DepartmentId { get; set; }
        public string Address { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string UpdatedBy { get; set; }
        public DateTime CreatedAt { get; set; }
        public string CreatedBy { get; set; }
        public decimal Salary { get; set; }
        public decimal JoiningDate { get; set; }
        public decimal Status { get; set; }
        
    }
}

public class EmployeeViewModel
{
    public int EmployeeId { get; set; }
    public string EmployeeName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public decimal Salary { get; set; }
    public int PositionId { get; set; }  // Make sure this exists
    public int DepartmentId { get; set; }
    public int OrganizationId { get; set; }
    public string PositionTitle { get; set; }  // Additional info for displaying
    public string DepartmentName { get; set; }
    public string OrganizationName { get; set; }
    public int AverageSalary { get; set; }
    public int JoiningDate { get; set; }
    public decimal Status { get; set; }
}
