<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="WebApplication1.AdminDashboard" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Admin Dashboard</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background: #f5f7fa;
            margin: 0;
            padding: 0;
        }
        .dashboard-container {
            max-width: 700px;
            margin: 50px auto;
            background: white;
            padding: 40px 30px;
            border-radius: 10px;
            box-shadow: 0 0 25px rgba(0,0,0,0.1);
            text-align: center;
        }
        h2 {
            margin-bottom: 30px;
            color: #003366;
        }
        .dashboard-btn {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 15px 30px;
            margin: 12px 10px;
            font-size: 1.1rem;
            border-radius: 8px;
            cursor: pointer;
            transition: background-color 0.3s ease;
            min-width: 220px;
        }
        .dashboard-btn:hover {
            background-color: #0056b3;
        }
        .dashboard-btn.secondary {
            background-color: #dc3545;
        }
        .dashboard-btn.secondary:hover {
            background-color: #a71d2a;
        }
        .btn-back {
            background-color: #28a745;
            margin-top: 25px;
        }
        .btn-back:hover {
            background-color: #1e7e34;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <h2>Admin Dashboard</h2>

            <asp:Button ID="btnCreateJob" runat="server" Text="Create Job" CssClass="dashboard-btn" OnClick="btnCreateJob_Click" />
            <asp:Button ID="btnDeleteJob" runat="server" Text="Delete Job" CssClass="dashboard-btn secondary" OnClick="btnDeleteJob_Click" />
            <asp:Button ID="btnManageJobs" runat="server" Text="Manage Job Applications" CssClass="dashboard-btn" OnClick="btnManageJobs_Click" />
            <br />
        </div>
    </form>
</body>
</html>
