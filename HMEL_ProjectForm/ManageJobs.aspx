<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ManageJobs.aspx.cs" Inherits="WebApplication1.ManageJobs" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Manage Jobs</title>
    <style>
        body {
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            background-color: #f5f7fa;
            padding: 40px;
        }
        .container {
            max-width: 900px;
            margin: auto;
            background: white;
            padding: 30px;
            border-radius: 12px;
            box-shadow: 0 0 20px rgba(0,0,0,0.1);
        }
        h2 {
            text-align: center;
            margin-bottom: 25px;
        }
        .btn-back {
            background-color: #007bff;
            color: white;
            border: none;
            padding: 12px 20px;
            border-radius: 8px;
            cursor: pointer;
            margin-bottom: 20px;
        }
        .btn-back:hover {
            background-color: #0056b3;
        }
        .message {
            text-align: center;
            margin: 15px 0;
            color: green;
            font-weight: bold;
        }
        .error {
            color: red;
        }
        /* GridView styling */
        .gridview {
            width: 100%;
            border-collapse: collapse;
        }
        .gridview th, .gridview td {
            padding: 12px;
            border: 1px solid #ddd;
            text-align: left;
        }
        .gridview th {
            background-color: #007bff;
            color: white;
        }
        .gridview tr:nth-child(even) {
            background-color: #f9f9f9;
        }
        .gridview tr:hover {
            background-color: #f1f1f1;
        }
        .edit-textbox {
            width: 90%;
            padding: 6px;
        }
        .btn-save, .btn-cancel {
            background-color: #28a745;
            color: white;
            border: none;
            padding: 6px 12px;
            border-radius: 6px;
            cursor: pointer;
            margin-right: 5px;
        }
        .btn-cancel {
            background-color: #dc3545;
        }
        .btn-save:hover {
            background-color: #1e7e34;
        }
        .btn-cancel:hover {
            background-color: #a71d2a;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>Manage Jobs</h2>
            <asp:Button ID="btnBack" runat="server" Text="← Back to Admin Dashboard" CssClass="btn-back" OnClick="btnBack_Click" />
            <asp:Label ID="lblMessage" runat="server" CssClass="message" />
            <asp:GridView ID="gvJobs" runat="server" AutoGenerateColumns="False" DataKeyNames="JobId"
                OnRowEditing="gvJobs_RowEditing" OnRowCancelingEdit="gvJobs_RowCancelingEdit"
                OnRowUpdating="gvJobs_RowUpdating" OnRowDeleting="gvJobs_RowDeleting" CssClass="gridview">
                <Columns>
                    <asp:BoundField DataField="JobId" HeaderText="Job ID" ReadOnly="True" />
                    <asp:BoundField DataField="Title" HeaderText="Title" />
                    <asp:BoundField DataField="Description" HeaderText="Description" />
                    <asp:BoundField DataField="Location" HeaderText="Location" />
                    <asp:BoundField DataField="JobType" HeaderText="Job Type" />
                    <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
                </Columns>
            </asp:GridView>
        </div>
    </form>
</body>
</html>
