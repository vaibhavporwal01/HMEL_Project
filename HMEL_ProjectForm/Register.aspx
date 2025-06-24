<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Register.aspx.cs" Inherits="WebApplication1.Register" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>HMEL Registration Form</title>
    <style>
        body {
            font-family: Arial;
            background-color: #f4f4f4;
            padding: 40px;
        }

        .container {
            background-color: white;
            max-width: 500px;
            margin: auto;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0px 0px 10px gray;
        }

        h2 {
            text-align: center;
            color: #004080;
        }

        .form-group {
            margin-bottom: 15px;
        }

        label {
            font-weight: bold;
            display: block;
        }

        input[type="text"], input[type="email"], select {
            width: 100%;
            padding: 10px;
            margin-top: 5px;
            border-radius: 5px;
            border: 1px solid #ccc;
        }

        .btn-submit {
            background-color: #004080;
            color: white;
            padding: 12px 25px;
            border: none;
            border-radius: 8px;
            cursor: pointer;
            font-size: 16px;
            display: block;
            margin: auto;
        }

        .btn-submit:hover {
            background-color: #0066cc;
        }

        .message {
            text-align: center;
            color: green;
            font-weight: bold;
            margin-top: 20px;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="container">
            <h2>HMEL Registration</h2>

            <div class="form-group">
                <label for="txtName">Full Name</label>
                <asp:TextBox ID="txtName" runat="server" placeholder="Enter your full name" />
            </div>

            <div class="form-group">
                <label for="txtEmail">Email</label>
                <asp:TextBox ID="txtEmail" runat="server" TextMode="Email" placeholder="Enter your email" />
            </div>

            <div class="form-group">
                <label for="txtMobile">Mobile Number</label>
                <asp:TextBox ID="txtMobile" runat="server" placeholder="Enter your mobile number" />
            </div>

            <div class="form-group">
                <label for="ddlRole">Role Applying For</label>
                <asp:DropDownList ID="ddlRole" runat="server">
                    <asp:ListItem Text="Select Role" Value="" />
                    <asp:ListItem Text="Software Developer" Value="Software Developer" />
                    <asp:ListItem Text="Data Analyst" Value="Data Analyst" />
                    <asp:ListItem Text="Operations Engineer" Value="Operations Engineer" />
                </asp:DropDownList>
            </div>

            <asp:Button ID="btnSubmit" runat="server" Text="Submit" CssClass="btn-submit" OnClick="btnSubmit_Click" />

            <asp:Label ID="lblMessage" runat="server" CssClass="message" Visible="false"></asp:Label>
        </div>
    </form>
</body>
</html>