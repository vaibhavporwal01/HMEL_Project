using System;
using System.Configuration;
using System.Web.UI;
using MySql.Data.MySqlClient;
using System.IO;

namespace HMELApplication
{
    public partial class FirstPage : System.Web.UI.Page
    {
        // Get connection string from web.config
        private string connectionString = ConfigurationManager.ConnectionStrings["HMELConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Disable unobtrusive validation mode
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                lblMessage.Text = "";
            }
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Save application to database
                    int applicationId = SaveApplicationToDatabase();

                    if (applicationId > 0)
                    {
                        DisplaySuccessMessage(applicationId);
                        ClearForm();
                    }
                    else
                    {
                        DisplayErrorMessage("Failed to submit application. Please try again.");
                    }
                }
                catch (Exception ex)
                {
                    DisplayErrorMessage("Database Error: " + ex.Message);
                }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            ClearForm();
            DisplayCancelMessage();
        }

        protected void btnSaveDraft_Click(object sender, EventArgs e)
        {
            try
            {
                // Save draft to database with status 'Draft'
                int draftId = SaveDraftToDatabase();

                if (draftId > 0)
                {
                    DisplayDraftMessage(draftId);
                }
                else
                {
                    DisplayErrorMessage("Failed to save draft. Please try again.");
                }
            }
            catch (Exception ex)
            {
                DisplayErrorMessage("Database Error while saving draft: " + ex.Message);
            }
        }

        // Save application to MySQL database
        private int SaveApplicationToDatabase()
        {
            int applicationId = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO JobApplications (
                        FirstName, LastName, FatherName, DateOfBirth, Gender, MaritalStatus,
                        Email, Phone, Address, City, State, PinCode, HighestQualification,
                        Specialization, University, YearOfPassing, Percentage, TotalExperience,
                        CurrentCompany, CurrentDesignation, CurrentSalary, PositionApplied,
                        ExpectedSalary, PhotoPath, ResumePath, AdditionalInfo, ApplicationDate, Status
                    ) VALUES (
                        @FirstName, @LastName, @FatherName, @DateOfBirth, @Gender, @MaritalStatus,
                        @Email, @Phone, @Address, @City, @State, @PinCode, @HighestQualification,
                        @Specialization, @University, @YearOfPassing, @Percentage, @TotalExperience,
                        @CurrentCompany, @CurrentDesignation, @CurrentSalary, @PositionApplied,
                        @ExpectedSalary, @PhotoPath, @ResumePath, @AdditionalInfo, @ApplicationDate, @Status
                    );
                    SELECT LAST_INSERT_ID();";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters
                    command.Parameters.AddWithValue("@FirstName", txtFirstName.Text);
                    command.Parameters.AddWithValue("@LastName", txtLastName.Text);
                    command.Parameters.AddWithValue("@FatherName", txtFatherName.Text);
                    command.Parameters.AddWithValue("@DateOfBirth", DateTime.Parse(txtDateOfBirth.Text));
                    command.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue);
                    command.Parameters.AddWithValue("@MaritalStatus", ddlMaritalStatus.SelectedValue ?? "");
                    command.Parameters.AddWithValue("@Email", txtEmail.Text);
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text);
                    command.Parameters.AddWithValue("@Address", txtAddress.Text);
                    command.Parameters.AddWithValue("@City", txtCity.Text);
                    command.Parameters.AddWithValue("@State", txtState.Text);
                    command.Parameters.AddWithValue("@PinCode", txtPinCode.Text);
                    command.Parameters.AddWithValue("@HighestQualification", ddlHighestQualification.SelectedValue);
                    command.Parameters.AddWithValue("@Specialization", txtSpecialization.Text ?? "");
                    command.Parameters.AddWithValue("@University", txtUniversity.Text ?? "");
                    command.Parameters.AddWithValue("@YearOfPassing", txtYearOfPassing.Text ?? "");
                    command.Parameters.AddWithValue("@Percentage", txtPercentage.Text ?? "");
                    command.Parameters.AddWithValue("@TotalExperience", txtTotalExperience.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentCompany", txtCurrentCompany.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentDesignation", txtCurrentDesignation.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentSalary", txtCurrentSalary.Text ?? "");
                    command.Parameters.AddWithValue("@PositionApplied", ddlPositionApplied.SelectedValue);
                    command.Parameters.AddWithValue("@ExpectedSalary", txtExpectedSalary.Text ?? "");
                    command.Parameters.AddWithValue("@PhotoPath", SaveUploadedFile(fuPassportPhoto, "Photos"));
                    command.Parameters.AddWithValue("@ResumePath", SaveUploadedFile(fuResume, "Resumes"));
                    command.Parameters.AddWithValue("@AdditionalInfo", txtAdditionalInfo.Text ?? "");
                    command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Submitted");

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        applicationId = Convert.ToInt32(result);
                    }
                }
            }

            return applicationId;
        }

        // Save draft to database
        private int SaveDraftToDatabase()
        {
            int draftId = 0;

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    INSERT INTO JobApplications (
                        FirstName, LastName, FatherName, DateOfBirth, Gender, MaritalStatus,
                        Email, Phone, Address, City, State, PinCode, HighestQualification,
                        Specialization, University, YearOfPassing, Percentage, TotalExperience,
                        CurrentCompany, CurrentDesignation, CurrentSalary, PositionApplied,
                        ExpectedSalary, AdditionalInfo, ApplicationDate, Status
                    ) VALUES (
                        @FirstName, @LastName, @FatherName, @DateOfBirth, @Gender, @MaritalStatus,
                        @Email, @Phone, @Address, @City, @State, @PinCode, @HighestQualification,
                        @Specialization, @University, @YearOfPassing, @Percentage, @TotalExperience,
                        @CurrentCompany, @CurrentDesignation, @CurrentSalary, @PositionApplied,
                        @ExpectedSalary, @AdditionalInfo, @ApplicationDate, @Status
                    );
                    SELECT LAST_INSERT_ID();";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    // Add parameters (only non-empty fields for draft)
                    command.Parameters.AddWithValue("@FirstName", txtFirstName.Text ?? "");
                    command.Parameters.AddWithValue("@LastName", txtLastName.Text ?? "");
                    command.Parameters.AddWithValue("@FatherName", txtFatherName.Text ?? "");
                    command.Parameters.AddWithValue("@DateOfBirth", string.IsNullOrEmpty(txtDateOfBirth.Text) ? (object)DBNull.Value : DateTime.Parse(txtDateOfBirth.Text));
                    command.Parameters.AddWithValue("@Gender", ddlGender.SelectedValue ?? "");
                    command.Parameters.AddWithValue("@MaritalStatus", ddlMaritalStatus.SelectedValue ?? "");
                    command.Parameters.AddWithValue("@Email", txtEmail.Text ?? "");
                    command.Parameters.AddWithValue("@Phone", txtPhone.Text ?? "");
                    command.Parameters.AddWithValue("@Address", txtAddress.Text ?? "");
                    command.Parameters.AddWithValue("@City", txtCity.Text ?? "");
                    command.Parameters.AddWithValue("@State", txtState.Text ?? "");
                    command.Parameters.AddWithValue("@PinCode", txtPinCode.Text ?? "");
                    command.Parameters.AddWithValue("@HighestQualification", ddlHighestQualification.SelectedValue ?? "");
                    command.Parameters.AddWithValue("@Specialization", txtSpecialization.Text ?? "");
                    command.Parameters.AddWithValue("@University", txtUniversity.Text ?? "");
                    command.Parameters.AddWithValue("@YearOfPassing", txtYearOfPassing.Text ?? "");
                    command.Parameters.AddWithValue("@Percentage", txtPercentage.Text ?? "");
                    command.Parameters.AddWithValue("@TotalExperience", txtTotalExperience.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentCompany", txtCurrentCompany.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentDesignation", txtCurrentDesignation.Text ?? "");
                    command.Parameters.AddWithValue("@CurrentSalary", txtCurrentSalary.Text ?? "");
                    command.Parameters.AddWithValue("@PositionApplied", ddlPositionApplied.SelectedValue ?? "");
                    command.Parameters.AddWithValue("@ExpectedSalary", txtExpectedSalary.Text ?? "");
                    command.Parameters.AddWithValue("@AdditionalInfo", txtAdditionalInfo.Text ?? "");
                    command.Parameters.AddWithValue("@ApplicationDate", DateTime.Now);
                    command.Parameters.AddWithValue("@Status", "Draft");

                    connection.Open();
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        draftId = Convert.ToInt32(result);
                    }
                }
            }

            return draftId;
        }

        // Handle file uploads
        private string SaveUploadedFile(System.Web.UI.WebControls.FileUpload fileUpload, string folder)
        {
            if (fileUpload.HasFile)
            {
                try
                {
                    string fileName = Path.GetFileName(fileUpload.FileName);
                    string fileExtension = Path.GetExtension(fileName);
                    string uniqueFileName = DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + fileName;
                    string uploadPath = Server.MapPath("~/Uploads/" + folder + "/");

                    // Create directory if it doesn't exist
                    if (!Directory.Exists(uploadPath))
                    {
                        Directory.CreateDirectory(uploadPath);
                    }

                    string fullPath = Path.Combine(uploadPath, uniqueFileName);
                    fileUpload.SaveAs(fullPath);

                    return "~/Uploads/" + folder + "/" + uniqueFileName;
                }
                catch (Exception ex)
                {
                    return "Error: " + ex.Message;
                }
            }
            return "";
        }

        private void DisplaySuccessMessage(int applicationId)
        {
            string message = "<div class='success-message'>";
            message += "<h3>🎉 Application Submitted Successfully!</h3>";
            message += "<p><strong>Thank you for your interest in HMEL!</strong></p>";
            message += "<p><strong>Application ID:</strong> " + applicationId + "</p>";
            message += "<p><strong>Applicant:</strong> " + txtFirstName.Text + " " + txtLastName.Text + "</p>";
            message += "<p><strong>Email:</strong> " + txtEmail.Text + "</p>";
            message += "<p><strong>Position:</strong> " + ddlPositionApplied.SelectedValue + "</p>";
            message += "<p><strong>Application Date:</strong> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "</p>";
            message += "<p><strong>✅ Application saved to database successfully!</strong></p>";
            message += "<p>We will review your application and contact you soon.</p>";
            message += "</div>";

            lblMessage.Text = message;
        }

        private void DisplayDraftMessage(int draftId)
        {
            string message = "<div class='draft-message'>";
            message += "<h3>💾 Draft Saved Successfully!</h3>";
            message += "<p><strong>Your application has been saved as draft in database.</strong></p>";
            message += "<p><strong>Draft ID:</strong> " + draftId + "</p>";

            if (!string.IsNullOrEmpty(txtFirstName.Text) && !string.IsNullOrEmpty(txtLastName.Text))
            {
                message += "<p><strong>Applicant:</strong> " + txtFirstName.Text + " " + txtLastName.Text + "</p>";
            }

            if (!string.IsNullOrEmpty(txtEmail.Text))
            {
                message += "<p><strong>Email:</strong> " + txtEmail.Text + "</p>";
            }

            message += "<p><strong>Draft Saved:</strong> " + DateTime.Now.ToString("dd/MM/yyyy HH:mm") + "</p>";
            message += "<p>You can continue editing and submit when ready.</p>";
            message += "</div>";

            lblMessage.Text = message;
        }

        private void DisplayCancelMessage()
        {
            string message = "<div class='cancel-message'>";
            message += "<h3>❌ Application Cancelled</h3>";
            message += "<p><strong>Your application has been cancelled.</strong></p>";
            message += "<p>All form data has been cleared. You can start fresh anytime.</p>";
            message += "<p>Thank you for considering HMEL as your career destination.</p>";
            message += "</div>";

            lblMessage.Text = message;
        }

        private void DisplayErrorMessage(string errorMessage)
        {
            string message = "<div class='error-message'>";
            message += "<h3>❌ Error</h3>";
            message += "<p>" + errorMessage + "</p>";
            message += "<p>Please try again or contact support if the problem persists.</p>";
            message += "</div>";

            lblMessage.Text = message;
        }

        private void ClearForm()
        {
            // Clear all form fields
            txtFirstName.Text = "";
            txtLastName.Text = "";
            txtFatherName.Text = "";
            txtDateOfBirth.Text = "";
            ddlGender.SelectedIndex = 0;
            ddlMaritalStatus.SelectedIndex = 0;
            txtEmail.Text = "";
            txtPhone.Text = "";
            txtAddress.Text = "";
            txtCity.Text = "";
            txtState.Text = "";
            txtPinCode.Text = "";
            ddlHighestQualification.SelectedIndex = 0;
            txtSpecialization.Text = "";
            txtUniversity.Text = "";
            txtYearOfPassing.Text = "";
            txtPercentage.Text = "";
            txtTotalExperience.Text = "";
            txtCurrentCompany.Text = "";
            txtCurrentDesignation.Text = "";
            txtCurrentSalary.Text = "";
            ddlPositionApplied.SelectedIndex = 0;
            txtExpectedSalary.Text = "";
            txtAdditionalInfo.Text = "";
        }
    }
}
