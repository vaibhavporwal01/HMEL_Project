using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class Register : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            string name = txtName.Text.Trim();
            string email = txtEmail.Text.Trim();
            string mobile = txtMobile.Text.Trim();
            string role = ddlRole.SelectedValue;

            if (name == "" || email == "" || mobile == "" || role == "")
            {
                lblMessage.ForeColor = System.Drawing.Color.Red;
                lblMessage.Text = "Please fill in all fields.";
                lblMessage.Visible = true;
            }
            else
            {
                // Future: Save to DB or send email here
                lblMessage.ForeColor = System.Drawing.Color.Green;
                lblMessage.Text = "Registration successful! Thank you, " + name + ".";
                lblMessage.Visible = true;

                // Optionally clear fields
                txtName.Text = "";
                txtEmail.Text = "";
                txtMobile.Text = "";
                ddlRole.SelectedIndex = 0;
            }
        }
    }
}