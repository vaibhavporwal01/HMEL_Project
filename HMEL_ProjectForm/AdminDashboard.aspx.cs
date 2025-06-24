using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplication1
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            // You can add auth/session checks here if needed
        }

        protected void btnCreateJob_Click(object sender, EventArgs e)
        {
            // Redirect to Create Job page
            Response.Redirect("CreateJob.aspx");
        }

        protected void btnDeleteJob_Click(object sender, EventArgs e)
        {
            // Redirect to Delete Job page or show message (change as needed)
            Response.Redirect("DeleteJob.aspx");
        }

        protected void btnManageJobs_Click(object sender, EventArgs e)
        {
            // Redirect to Manage Jobs page
            Response.Redirect("ManageJobs.aspx");
        }
    }
}