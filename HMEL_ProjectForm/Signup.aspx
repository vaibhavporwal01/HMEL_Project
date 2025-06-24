<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Signup.aspx.cs" Inherits="WebApplication1.Signup" %>


<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Sign Up - HMEL Careers</title>
    <style>
        body {
            margin: 0;
            padding: 0;
            background: linear-gradient(to right, #004080, #0059b3);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
            height: 100vh;
            display: flex;
            justify-content: center;
            align-items: center;
        }

        .signup-box {
            background-color: #ffffff;
            padding: 40px 30px;
            border-radius: 12px;
            box-shadow: 0 6px 25px rgba(0, 0, 0, 0.2);
            width: 100%;
            max-width: 420px;
        }

        .signup-box h2 {
            text-align: center;
            margin-bottom: 30px;
            color: #004080;
        }

        .form-group {
            margin-bottom: 20px;
        }

        .form-group label {
            font-weight: 600;
            display: block;
            margin-bottom: 6px;
            color: #333;
        }

        .form-group input {
            width: 100%;
            padding: 10px;
            border: 1px solid #bbb;
            border-radius: 5px;
            font-size: 14px;
            transition: border-color 0.3s ease;
        }

        .form-group input:focus {
            border-color: #004080;
            outline: none;
        }

        .signup-btn {
            width: 100%;
            padding: 12px;
            background-color: #004080;
            color: #fff;
            border: none;
            border-radius: 6px;
            font-size: 16px;
            font-weight: bold;
            cursor: pointer;
            transition: background-color 0.3s ease;
        }

        .signup-btn:hover {
            background-color: #003060;
        }

        .success-msg {
            margin-top: 15px;
            text-align: center;
            color: green;
            font-weight: 600;
        }

        .info-text {
            text-align: center;
            margin-top: 10px;
            font-size: 13px;
            color: #666;
        }

        .info-text a {
            color: #004080;
            text-decoration: none;
        }

        .info-text a:hover {
            text-decoration: underline;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="signup-box">
            <h2>Create Your Account</h2>

            <div class="form-group">
                <asp:Label ID="lblNewUser" runat="server" AssociatedControlID="txtNewUser" Text="Username" />
                <asp:TextBox ID="txtNewUser" runat="server" CssClass="form-control" />
            </div>

            <div class="form-group">
                <asp:Label ID="lblNewPass" runat="server" AssociatedControlID="txtNewPass" Text="Password" />
                <asp:TextBox ID="txtNewPass" runat="server" TextMode="Password" CssClass="form-control" />
            </div>

            <asp:Button ID="btnSignup" runat="server" Text="Sign Up" CssClass="signup-btn" OnClick="btnSignup_Click" />

            <asp:Label ID="lblMsg" runat="server" CssClass="success-msg" />

            <div class="info-text">
                Already have an account? <a href="Login.aspx">Log in</a>
            </div>
        </div>
    </form>
</body>
</html>

