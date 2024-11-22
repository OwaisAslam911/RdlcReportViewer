using RdlcReportViewer.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IdentityModel.Metadata;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using Dapper;

namespace RdlcReportViewer.Controllers
{
    public class HomeController : Controller
    {
       

        public HomeController(  )
        {
            
        }
        public ActionResult filter()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            string connectionStatusMessage = string.Empty;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    connectionStatusMessage = "Connection successful!";
                }
                catch (Exception ex)
                {
                    connectionStatusMessage = "Connection failed: " + ex.Message;
                    // Optionally log the exception using a logging framework like Serilog or NLog
                }
            }

            // Pass the connection status message to the view
            ViewBag.ConnectionStatus = connectionStatusMessage;

            return View();
        }


        public async Task<ActionResult> Index()
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            using (SqlConnection db = new SqlConnection(connectionString))
            {

                var organizations = await db.QueryAsync<Organizations>("SELECT * FROM Organizations");
                var departments = await db.QueryAsync<Departments>("SELECT * FROM Departments");
                var positions = await db.QueryAsync<Positions>("SELECT * FROM Positions");

                ViewBag.Organizations = new SelectList(organizations, "OrganizationId", "OrganizationName");
                ViewBag.Departments = new SelectList(departments, "DepartmentId", "DepartmentName");
                ViewBag.Positions = new SelectList(positions, "PositionId", "PositionTitle");
            }
            return View();
        }

        [HttpGet]
        public JsonResult GetDepartments(int organizationId)
        {
                 string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            string connectionStatusMessage = string.Empty;

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                // Query to fetch departments based on the selected organization
                string query = @"
            SELECT d.DepartmentId, d.DepartmentName 
            FROM Departments d
            INNER JOIN PositionMapping pm ON d.DepartmentId = pm.DepartmentId
            WHERE pm.OrganizationId = @OrganizationId";

                var departments = db.Query<Departments>(query, new { OrganizationId = organizationId }).ToList();
                return Json(departments);
            }
        }

        [HttpGet]
        public JsonResult GetPositions(int organizationId, int departmentId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            string connectionStatusMessage = string.Empty;

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                // Query to fetch positions based on the selected organization and department
                var query = @"
            SELECT p.PositionId, p.PositionTitle 
            FROM Positions p
            INNER JOIN PositionMapping pm ON p.PositionId = pm.PositionId
            WHERE pm.OrganizationId = @OrganizationId AND pm.DepartmentId = @DepartmentId";

                // Execute the query with the provided organizationId and departmentId
                var positions = db.Query<Positions>(query, new { OrganizationId = organizationId, DepartmentId = departmentId }).ToList();

                return Json(positions);
            }
        }


        [HttpGet]
        public async Task<ActionResult> GetFilter(int? organizationId, int? departmentId, int? positionId)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;

            string connectionStatusMessage = string.Empty;

            using (SqlConnection db = new SqlConnection(connectionString))
            {
                // Base SQL query to get average salaries by position in each department of every organization
                var sql = @"
            WITH AvgSalaries AS (
                SELECT 
                    o.OrganizationId,
                    o.OrganizationName,
                    d.DepartmentId,
                    d.DepartmentName,
                    p.PositionId,
                    p.PositionTitle,
                    
                    AVG(e.Salary) AS AverageSalary,
                    ROW_NUMBER() OVER (PARTITION BY o.OrganizationId ORDER BY AVG(e.Salary) DESC) AS SalaryRank
                FROM 
                    Employees e
                JOIN 
                    Positions p ON e.PositionId = p.PositionId
                JOIN 
                    Departments d ON p.DepartmentId = d.DepartmentId
                JOIN 
                    Organizations o ON e.OrganizationId = o.OrganizationId
                GROUP BY 
                    
                    o.OrganizationId, o.OrganizationName,
                    d.DepartmentId, d.DepartmentName,
                    p.PositionId, p.PositionTitle
            )
            SELECT 
                OrganizationId,
                OrganizationName,
                DepartmentId,
                DepartmentName,
                PositionId,
                PositionTitle,
                AverageSalary
            FROM 
                AvgSalaries
            WHERE 
                SalaryRank <= 2";

                var parameters = new DynamicParameters();

                // Apply filters based on optional parameters
                if (organizationId.HasValue)
                {
                    sql += " AND OrganizationId = @OrganizationId";
                    parameters.Add("@OrganizationId", organizationId.Value);
                }
                if (departmentId.HasValue)
                {
                    sql += " AND DepartmentId = @DepartmentId";
                    parameters.Add("@DepartmentId", departmentId.Value);
                }
                if (positionId.HasValue)
                {
                    sql += " AND PositionId = @PositionId";
                    parameters.Add("@PositionId", positionId.Value);
                }


                var result = await db.QueryAsync<EmployeeViewModel>(sql, parameters);




                return Json(result);
            }
        }


        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
   
    }
}
        
