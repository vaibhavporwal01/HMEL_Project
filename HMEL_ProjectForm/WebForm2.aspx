<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="WebForm2.aspx.cs" Inherits="WebApplication1.WebForm2" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HMEL Dashboard</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f9f9f9;
            margin: 0;
            padding: 0;
            text-align: center;
        }

        .dashboard-container {
            margin-top: 100px;
        }

        .btn {
            background-color: #004080;
            color: white;
            padding: 15px 40px;
            margin: 20px;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            transition: 0.3s;
        }

        .btn:hover {
            background-color: #0066cc;
        }

        h1 {
            margin-top: 40px;
            color: #333;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="dashboard-container">
            <h1>Welcome to HMEL Careers Dashboard</h1>
            <asp:Button ID="btnRegister" runat="server" Text="Register" CssClass="btn" OnClick="btnRegister_Click" />
            <asp:Button ID="btnApproval" runat="server" Text="Approval" CssClass="btn" OnClick="btnApproval_Click" />
            <asp:Button ID="btnAdmin" runat="server" Text="Admin Form" CssClass="btn" OnClick="btnAdmin_Click" />
        </div>
    </form>
</body>
</html>