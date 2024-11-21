using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace RdlcReportViewer.Models
{
    public class Departments
    {
        public int DepartmentId { get; set; }
        public string DepartmentName { get; set; }

        public int OrganizationId { get; set; }
    }
}