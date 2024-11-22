<%--<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="RdlcReportViewer.RdlcReport.Report" %>--%>

<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Report.aspx.cs" Inherits="RdlcReportViewer.RdlcReport.Report" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=15.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <meta http-equiv="X-UA-Compatible" content="IE=edge" />
    <title>Employee Report</title>
    <style type="text/css">
        #Text1 {
            height: 26px;
            width: 144px;
        }
        #Text2 {
            width: 143px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Employee Report</h2>
           <p>
                Search Employees: <input id="EmpName" runat="server" name="EmpName" class="search-input" type="text" placeholder="Employee Name" />
                Search Organizations: <input id="Org" runat="server" class="search-input" type="text" placeholder="Organization" /> 
                Search Department: <input id="Dept" runat="server" class="search-input" type="text" placeholder="Department" />
                Search Positions: <input id="Post" runat="server"  class="search-input" type="text" placeholder="Position" />
                Search Salary: <input id="Salary" runat="server" class="search-input" type="text" placeholder="Salary" />
                Search Status: <input id="Status" runat="server" class="search-input" type="text" placeholder="Status" />
            </p>
            <asp:Button ID="LoadReport" runat="server" Text="Refresh Report" OnClick="LoadReport_Click" />
            <asp:ScriptManager runat="server"></asp:ScriptManager>
            <rsweb:ReportViewer ID="ReportViewer1" runat="server" Width="100%" Height="600px" ProcessingMode="Local">
            </rsweb:ReportViewer>
            
        </div>
    </form>
</body>
</html>
