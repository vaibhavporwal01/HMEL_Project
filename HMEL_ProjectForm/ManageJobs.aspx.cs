using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using static WebApplication1.Homepage;

namespace WebApplication1
{
    public partial class ManageJobs : System.Web.UI.Page
    {
        private const string SessionJobsKey = "JobsList";

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Initialize with sample data if session empty
                if (Session[SessionJobsKey] == null)
                {
                    var jobs = new List<Job>
                    {
                        new Job { JobId = "JOB-1001", Title = "Software Engineer", Description = "Develop cool apps", Location = "New York", JobType = "Full-Time" },
                        new Job { JobId = "JOB-1002", Title = "Data Analyst", Description = "Analyze data trends", Location = "San Francisco", JobType = "Part-Time" },
                        new Job { JobId = "JOB-1003", Title = "Intern", Description = "Assist teams", Location = "Remote", JobType = "Internship" }
                    };
                    Session[SessionJobsKey] = jobs;
                }

                BindGrid();
            }
        }

        private void BindGrid()
        {
            gvJobs.DataSource = Session[SessionJobsKey] as List<Job>;
            gvJobs.DataBind();
        }

        protected void gvJobs_RowEditing(object sender, GridViewEditEventArgs e)
        {
            gvJobs.EditIndex = e.NewEditIndex;
            BindGrid();
            lblMessage.Text = "";
        }

        protected void gvJobs_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            gvJobs.EditIndex = -1;
            BindGrid();
            lblMessage.Text = "";
        }

        protected void gvJobs_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            var jobs = Session[SessionJobsKey] as List<Job>;

            string jobId = gvJobs.DataKeys[e.RowIndex].Value.ToString();

            // Find the job in list
            Job job = jobs.Find(j => j.JobId == jobId);
            if (job == null) return;

            // Get new values from GridView edit row controls
            GridViewRow row = gvJobs.Rows[e.RowIndex];
            string newTitle = (row.Cells[1].Controls[0] as TextBox)?.Text.Trim();
            string newDesc = (row.Cells[2].Controls[0] as TextBox)?.Text.Trim();
            string newLoc = (row.Cells[3].Controls[0] as TextBox)?.Text.Trim();
            string newType = (row.Cells[4].Controls[0] as TextBox)?.Text.Trim();

            // Basic validation
            if (string.IsNullOrEmpty(newTitle) || string.IsNullOrEmpty(newDesc) ||
                string.IsNullOrEmpty(newLoc) || string.IsNullOrEmpty(newType))
            {
                lblMessage.Text = "All fields are required to update.";
                lblMessage.CssClass = "message error";
                return;
            }

            // Update job info
            job.Title = newTitle;
            job.Description = newDesc;
            job.Location = newLoc;
            job.JobType = newType;

            gvJobs.EditIndex = -1;
            BindGrid();

            lblMessage.Text = $"Job {job.JobId} updated successfully.";
            lblMessage.CssClass = "message";
        }

        protected void gvJobs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            var jobs = Session[SessionJobsKey] as List<Job>;
            string jobId = gvJobs.DataKeys[e.RowIndex].Value.ToString();

            Job job = jobs.Find(j => j.JobId == jobId);
            if (job != null)
            {
                jobs.Remove(job);
                BindGrid();
                lblMessage.Text = $"Job {job.JobId} deleted successfully.";
                lblMessage.CssClass = "message";
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("AdminDashboard.aspx");
        }
    }
}