using Microsoft.Reporting.WebForms;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.Services.Description;

namespace RdlcReportViewer.RdlcReport
{
    public partial class Report : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadReportData();  // Change LoadReport to LoadReportData
            }
        }

        protected void LoadReport_Click(object sender, EventArgs e)
        {
            LoadReportData();  // Change LoadReport to LoadReportData
        }

        private void LoadReportData()  // Change LoadReport to LoadReportData
        {
            string connectionString = ConfigurationManager.ConnectionStrings["MyConnection"].ConnectionString;
            string query = @"
                SELECT e.EmployeeId, e.EmployeeName, e.Phone, e.Email, e.Salary, e.JoiningDate, e.Status, e.PositionId, e.OrganizationId, e.DepartmentId,
                p.PositionTitle, d.DepartmentName, o.OrganizationName 
                FROM Employees e
                INNER JOIN Positions p ON e.PositionId = p.PositionId
                INNER JOIN Departments d ON e.DepartmentId = d.DepartmentId
                INNER JOIN Organizations o ON e.OrganizationId = o.OrganizationId
                WHERE (1=1)"; 

            
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand();
                cmd.Connection = conn;

                
                if (!string.IsNullOrEmpty(EmpName.Value))
                {
                    query += " AND e.EmployeeName LIKE @EmployeeName";
                    cmd.Parameters.AddWithValue("@EmployeeName", "%" + EmpName.Value.Trim() + "%");
                }
                if (!string.IsNullOrEmpty(Org.Value))
                {
                    query += " AND o.OrganizationName LIKE @Organization";
                    cmd.Parameters.AddWithValue("@Organization", "%" + Org.Value.Trim() + "%");
                }
                if (!string.IsNullOrEmpty(Dept.Value))
                {
                    query += " AND d.DepartmentName LIKE @Department";
                    cmd.Parameters.AddWithValue("@Department", "%" + Dept.Value.Trim() + "%");
                }
                if (!string.IsNullOrEmpty(Post.Value))
                {
                    query += " AND p.PositionTitle LIKE @Position";
                    cmd.Parameters.AddWithValue("@Position", "%" + Post.Value.Trim() + "%");
                }
                if (!string.IsNullOrEmpty(Salary.Value))
                {
                    query += " AND e.Salary = @Salary";
                    cmd.Parameters.AddWithValue("@Salary", Convert.ToDecimal(Salary.Value.Trim()));
                }
                if (!string.IsNullOrEmpty(Status.Value))
                {
                    query += " AND e.Status LIKE @Status";
                    cmd.Parameters.AddWithValue("@Status", "%" + Status.Value.Trim() + "%");
                }

           
                cmd.CommandText = query;

                SqlDataAdapter adapter = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();

                try
                {
                    conn.Open();
                    adapter.Fill(dt);

                    // Set report path and clear previous data sources
                    ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/RdlcReport/RdlcReport.rdl");
                    ReportViewer1.LocalReport.DataSources.Clear();

                    // Add the new data source
                    ReportDataSource rds = new ReportDataSource("Employees", dt);
                    ReportViewer1.LocalReport.DataSources.Add(rds);

                    // Refresh the report
                    ReportViewer1.LocalReport.Refresh();
                }
                catch (Exception ex)
                {
                    // Handle exceptions
                    Response.Write("Error: " + ex.Message);
                }
                finally
                {
                    conn.Close();
                }
            }
        }
    }
}
