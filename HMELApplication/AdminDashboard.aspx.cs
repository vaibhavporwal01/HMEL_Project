using System;
using System.Configuration;
using System.Data;
using System.Web.UI;
using System.Web.UI.WebControls;
using MySql.Data.MySqlClient;
using System.Text;
using System.IO;
using iTextSharp.text;
using iTextSharp.text.pdf;
using System.Web;

namespace HMELApplication
{
    public partial class AdminDashboard : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HMELConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            // Check if admin is logged in
            if (Session["IsAdmin"] == null || !(bool)Session["IsAdmin"])
            {
                Response.Redirect("Default.aspx");
                return;
            }

            // Disable unobtrusive validation mode
            UnobtrusiveValidationMode = UnobtrusiveValidationMode.None;

            if (!IsPostBack)
            {
                LoadAdminInfo();
                LoadStatistics();
                LoadApplications();
            }
        }

        private void LoadAdminInfo()
        {
            if (Session["AdminFullName"] != null)
            {
                lblAdminName.Text = Session["AdminFullName"].ToString();
            }
        }

        private void LoadStatistics()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();

                // Total Applications
                string totalQuery = "SELECT COUNT(*) FROM JobApplications";
                using (MySqlCommand cmd = new MySqlCommand(totalQuery, connection))
                {
                    lblTotalApplications.Text = cmd.ExecuteScalar().ToString();
                }

                // Pending Applications (Submitted status)
                string pendingQuery = "SELECT COUNT(*) FROM JobApplications WHERE Status = 'Submitted'";
                using (MySqlCommand cmd = new MySqlCommand(pendingQuery, connection))
                {
                    lblPendingApplications.Text = cmd.ExecuteScalar().ToString();
                }

                // Approved Applications (Stage 1 and Stage 2)
                string approvedQuery = "SELECT COUNT(*) FROM JobApplications WHERE Status IN ('Stage 1 - Online Assessment', 'Stage 2 - Interview')";
                using (MySqlCommand cmd = new MySqlCommand(approvedQuery, connection))
                {
                    lblApprovedApplications.Text = cmd.ExecuteScalar().ToString();
                }

                // Rejected Applications
                string rejectedQuery = "SELECT COUNT(*) FROM JobApplications WHERE Status = 'Rejected'";
                using (MySqlCommand cmd = new MySqlCommand(rejectedQuery, connection))
                {
                    lblRejectedApplications.Text = cmd.ExecuteScalar().ToString();
                }
            }
        }

        private void LoadApplications()
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                StringBuilder query = new StringBuilder(@"
                    SELECT ApplicationID, FirstName, LastName, Email, Phone, PositionApplied, 
                           Status, ApplicationDate, TotalExperience, HighestQualification,
                           CurrentCompany, ExpectedSalary, City, State, PhotoPath, ResumePath
                    FROM JobApplications 
                    WHERE 1=1");

                // Apply filters
                if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                {
                    query.Append(" AND Status = @Status");
                }

                if (!string.IsNullOrEmpty(ddlPositionFilter.SelectedValue))
                {
                    query.Append(" AND PositionApplied = @Position");
                }

                if (!string.IsNullOrEmpty(txtSearch.Text))
                {
                    query.Append(" AND (FirstName LIKE @Search OR LastName LIKE @Search OR Email LIKE @Search)");
                }

                // Apply date filter
                if (!string.IsNullOrEmpty(ddlDateFilter.SelectedValue))
                {
                    switch (ddlDateFilter.SelectedValue)
                    {
                        case "Today":
                            query.Append(" AND DATE(ApplicationDate) = CURDATE()");
                            break;
                        case "Last 7 Days":
                            query.Append(" AND ApplicationDate >= DATE_SUB(NOW(), INTERVAL 7 DAY)");
                            break;
                        case "Last 30 Days":
                            query.Append(" AND ApplicationDate >= DATE_SUB(NOW(), INTERVAL 30 DAY)");
                            break;
                        case "Last 90 Days":
                            query.Append(" AND ApplicationDate >= DATE_SUB(NOW(), INTERVAL 90 DAY)");
                            break;
                    }
                }

                query.Append(" ORDER BY ApplicationDate DESC");

                using (MySqlCommand command = new MySqlCommand(query.ToString(), connection))
                {
                    // Add parameters
                    if (!string.IsNullOrEmpty(ddlStatusFilter.SelectedValue))
                    {
                        command.Parameters.AddWithValue("@Status", ddlStatusFilter.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(ddlPositionFilter.SelectedValue))
                    {
                        command.Parameters.AddWithValue("@Position", ddlPositionFilter.SelectedValue);
                    }
                    if (!string.IsNullOrEmpty(txtSearch.Text))
                    {
                        command.Parameters.AddWithValue("@Search", "%" + txtSearch.Text + "%");
                    }

                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        gvApplications.DataSource = dt;
                        gvApplications.DataBind();
                    }
                }
            }
        }

        protected void gvApplications_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int applicationId = Convert.ToInt32(e.CommandArgument);
            hdnSelectedApplicationId.Value = applicationId.ToString();

            switch (e.CommandName)
            {
                case "ViewApplication":
                    ShowApplicationDetails(applicationId);
                    break;
                case "DownloadPDF":
                    GenerateAndDownloadPDF(applicationId);
                    break;
                case "ApproveStage1":
                    hdnSelectedAction.Value = "Stage1";
                    ShowStatusModal("Approve for Stage 1 - Online Assessment");
                    break;
                case "ApproveStage2":
                    hdnSelectedAction.Value = "Stage2";
                    ShowStatusModal("Approve for Stage 2 - Interview");
                    break;
                case "RejectApplication":
                    hdnSelectedAction.Value = "Reject";
                    ShowStatusModal("Reject Application");
                    break;
            }
        }

        private void ShowApplicationDetails(int applicationId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = @"
                    SELECT * FROM JobApplications 
                    WHERE ApplicationID = @ApplicationID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    connection.Open();

                    using (MySqlDataReader reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            StringBuilder details = new StringBuilder();
                            details.Append("<div class='detail-grid'>");

                            // Personal Information
                            details.AppendFormat(@"
                                <div class='detail-item'>
                                    <div class='detail-label'>Full Name</div>
                                    <div class='detail-value'>{0} {1}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Father's Name</div>
                                    <div class='detail-value'>{2}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Date of Birth</div>
                                    <div class='detail-value'>{3:dd/MM/yyyy}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Gender</div>
                                    <div class='detail-value'>{4}</div>
                                </div>",
                                reader["FirstName"], reader["LastName"], reader["FatherName"],
                                reader["DateOfBirth"], reader["Gender"]);

                            // Contact Information
                            details.AppendFormat(@"
                                <div class='detail-item'>
                                    <div class='detail-label'>Email</div>
                                    <div class='detail-value'>{0}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Phone</div>
                                    <div class='detail-value'>{1}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Address</div>
                                    <div class='detail-value'>{2}, {3}, {4} - {5}</div>
                                </div>",
                                reader["Email"], reader["Phone"], reader["Address"],
                                reader["City"], reader["State"], reader["PinCode"]);

                            // Education & Experience
                            details.AppendFormat(@"
                                <div class='detail-item'>
                                    <div class='detail-label'>Qualification</div>
                                    <div class='detail-value'>{0}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Specialization</div>
                                    <div class='detail-value'>{1}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Experience</div>
                                    <div class='detail-value'>{2}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Current Company</div>
                                    <div class='detail-value'>{3}</div>
                                </div>",
                                reader["HighestQualification"], reader["Specialization"],
                                reader["TotalExperience"], reader["CurrentCompany"]);

                            // Position Applied
                            details.AppendFormat(@"
                                <div class='detail-item'>
                                    <div class='detail-label'>Position Applied</div>
                                    <div class='detail-value'>{0}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Expected Salary</div>
                                    <div class='detail-value'>{1}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Application Date</div>
                                    <div class='detail-value'>{2:dd/MM/yyyy HH:mm}</div>
                                </div>
                                <div class='detail-item'>
                                    <div class='detail-label'>Status</div>
                                    <div class='detail-value'><span class='status-badge {4}'>{3}</span></div>
                                </div>",
                                reader["PositionApplied"], reader["ExpectedSalary"],
                                reader["ApplicationDate"], reader["Status"],
                                GetStatusClass(reader["Status"].ToString()));

                            details.Append("</div>");

                            // Additional Information
                            if (!string.IsNullOrEmpty(reader["AdditionalInfo"].ToString()))
                            {
                                details.AppendFormat(@"
                                    <div style='margin-top: 1rem;'>
                                        <div class='detail-label'>Additional Information</div>
                                        <div class='detail-value' style='margin-top: 0.5rem; padding: 1rem; background: #f8fafc; border-radius: 6px;'>{0}</div>
                                    </div>", reader["AdditionalInfo"]);
                            }

                            // Files Section
                            details.Append("<div class='files-section'>");
                            details.Append("<h3 style='color: #374151; margin-bottom: 1rem;'>📎 Uploaded Files</h3>");
                            details.Append("<div class='files-grid'>");

                            // Photo Section
                            details.Append("<div class='file-card'>");
                            details.Append("<h4>📷 Applicant Photo</h4>");
                            details.Append("<div class='photo-container'>");

                            string photoPath = reader["PhotoPath"]?.ToString();

                            bool photoExists = false;
                            if (!string.IsNullOrEmpty(photoPath))
                            {
                                string cleanPhotoPath = photoPath.Replace("~/", "").Replace("~\\", "");
                                string fullPhotoPath = Server.MapPath("~/" + cleanPhotoPath);

                                if (File.Exists(fullPhotoPath))
                                {
                                    photoExists = true;
                                    photoPath = cleanPhotoPath;
                                }
                            }

                            if (photoExists)
                            {
                                details.AppendFormat(@"
                                    <img src='{0}' alt='Applicant Photo' class='applicant-photo' />
                                    <div class='file-actions' style='margin-top: 1rem;'>
                                        <a href='FileViewer.aspx?type=photo&id={1}' target='_blank' class='btn-file btn-view-file'>👁️ View Full Size</a>
                                        <a href='FileViewer.aspx?type=photo&id={1}&download=1' class='btn-file btn-download-file'>⬇️ Download</a>
                                    </div>",
                                    photoPath, applicationId);
                            }
                            else
                            {
                                details.Append("<div class='no-photo'>No Photo Available</div>");
                            }

                            details.Append("</div>");
                            details.Append("</div>");

                            // Resume Section
                            details.Append("<div class='file-card'>");
                            details.Append("<h4>📄 Resume</h4>");
                            details.Append("<div class='resume-container'>");

                            string resumePath = reader["ResumePath"]?.ToString();

                            bool resumeExists = false;
                            if (!string.IsNullOrEmpty(resumePath))
                            {
                                string cleanResumePath = resumePath.Replace("~/", "").Replace("~\\", "");
                                string fullResumePath = Server.MapPath("~/" + cleanResumePath);

                                if (File.Exists(fullResumePath))
                                {
                                    resumeExists = true;
                                    resumePath = cleanResumePath;
                                }
                            }

                            if (resumeExists)
                            {
                                string fileName = Path.GetFileName(resumePath);
                                string fileExtension = Path.GetExtension(resumePath).ToLower();
                                string fileIcon = GetFileIcon(fileExtension);

                                details.AppendFormat(@"
                                    <div class='file-info'>
                                        <div class='file-name'>{0} {1}</div>
                                        <div style='color: #64748b; font-size: 0.75rem;'>File Type: {2}</div>
                                    </div>
                                    <div class='file-actions'>
                                        <a href='FileViewer.aspx?type=resume&id={3}' target='_blank' class='btn-file btn-view-file'>👁️ View Resume</a>
                                        <a href='FileViewer.aspx?type=resume&id={3}&download=1' class='btn-file btn-download-file'>⬇️ Download</a>
                                    </div>",
                                    fileIcon, fileName, fileExtension.ToUpper(), applicationId);
                            }
                            else
                            {
                                details.Append("<div class='file-info'>No Resume Available</div>");
                            }

                            details.Append("</div>");
                            details.Append("</div>");
                            details.Append("</div>");
                            details.Append("</div>");

                            string script = $@"
                                document.getElementById('applicationDetails').innerHTML = `{details.ToString()}`;
                                showApplicationModal();
                            ";

                            ClientScript.RegisterStartupScript(this.GetType(), "ShowApplicationDetails", script, true);
                        }
                    }
                }
            }
        }

        private string GetFileIcon(string extension)
        {
            switch (extension)
            {
                case ".pdf":
                    return "📄";
                case ".doc":
                    return "📝";
                case ".txt":
                    return "📃";
                default:
                    return "📄";
            }
        }

        private void GenerateAndDownloadPDF(int applicationId)
        {
            try
            {
                // Get application data
                DataRow applicationData = GetApplicationData(applicationId);
                if (applicationData == null)
                {
                    ShowMessage("Application not found.", "error");
                    return;
                }

                // Create PDF with professional layout
                using (MemoryStream ms = new MemoryStream())
                {
                    Document document = new Document(PageSize.A4, 40, 40, 40, 40);
                    PdfWriter writer = PdfWriter.GetInstance(document, ms);
                    document.Open();

                    // Define professional fonts and colors
                    Font titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18, new BaseColor(25, 25, 112)); // Navy Blue
                    Font companyFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 14, new BaseColor(70, 130, 180)); // Steel Blue
                    Font headerFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(47, 79, 79)); // Dark Slate Gray
                    Font subHeaderFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10, BaseColor.WHITE);
                    Font labelFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, new BaseColor(25, 25, 25));
                    Font valueFont = FontFactory.GetFont(FontFactory.HELVETICA, 9, new BaseColor(50, 50, 50));
                    Font smallFont = FontFactory.GetFont(FontFactory.HELVETICA, 8, BaseColor.GRAY);

                    // Header Section with Logo, Title, and Photo
                    PdfPTable headerTable = new PdfPTable(3);
                    headerTable.WidthPercentage = 100;
                    headerTable.SetWidths(new float[] { 2.5f, 3f, 2.5f });
                    headerTable.SpacingAfter = 15f;

                    // Left: Company Logo
                    PdfPCell logoCell = new PdfPCell();
                    logoCell.Border = Rectangle.BOX;
                    logoCell.BorderWidth = 2f;
                    logoCell.BorderColor = new BaseColor(70, 130, 180);
                    logoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    logoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    logoCell.Padding = 10;
                    logoCell.MinimumHeight = 100;
                    logoCell.BackgroundColor = new BaseColor(248, 250, 252);

                    try
                    {
                        string logoPath = Server.MapPath("~/Images/hmel-logo.png");
                        if (File.Exists(logoPath))
                        {
                            iTextSharp.text.Image logo = iTextSharp.text.Image.GetInstance(logoPath);
                            logo.ScaleToFit(80f, 80f);
                            logoCell.AddElement(logo);
                        }
                        else
                        {
                            Paragraph logoText = new Paragraph("HMEL\nLOGO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(70, 130, 180)));
                            logoText.Alignment = Element.ALIGN_CENTER;
                            logoCell.AddElement(logoText);
                        }
                    }
                    catch
                    {
                        Paragraph logoText = new Paragraph("HMEL\nLOGO", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 12, new BaseColor(70, 130, 180)));
                        logoText.Alignment = Element.ALIGN_CENTER;
                        logoCell.AddElement(logoText);
                    }
                    headerTable.AddCell(logoCell);

                    // Center: Title and Company Info
                    PdfPCell titleCell = new PdfPCell();
                    titleCell.Border = Rectangle.BOX;
                    titleCell.BorderWidth = 2f;
                    titleCell.BorderColor = new BaseColor(70, 130, 180);
                    titleCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    titleCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    titleCell.Padding = 15;
                    titleCell.BackgroundColor = new BaseColor(245, 248, 252);

                    Paragraph title = new Paragraph("JOB APPLICATION FORM", titleFont);
                    title.Alignment = Element.ALIGN_CENTER;
                    title.SpacingAfter = 8f;
                    titleCell.AddElement(title);

                    Paragraph company = new Paragraph("Hindustan Mittal Energy Limited", companyFont);
                    company.Alignment = Element.ALIGN_CENTER;
                    company.SpacingAfter = 5f;
                    titleCell.AddElement(company);

                    Paragraph subtitle = new Paragraph("Human Resources Department", FontFactory.GetFont(FontFactory.HELVETICA, 10, new BaseColor(100, 100, 100)));
                    subtitle.Alignment = Element.ALIGN_CENTER;
                    titleCell.AddElement(subtitle);

                    headerTable.AddCell(titleCell);

                    // Right: Applicant Photo
                    PdfPCell photoCell = new PdfPCell();
                    photoCell.Border = Rectangle.BOX;
                    photoCell.BorderWidth = 2f;
                    photoCell.BorderColor = new BaseColor(70, 130, 180);
                    photoCell.HorizontalAlignment = Element.ALIGN_CENTER;
                    photoCell.VerticalAlignment = Element.ALIGN_MIDDLE;
                    photoCell.Padding = 8;
                    photoCell.MinimumHeight = 100;
                    photoCell.BackgroundColor = new BaseColor(248, 250, 252);

                    string photoPath = applicationData["PhotoPath"]?.ToString();
                    if (!string.IsNullOrEmpty(photoPath))
                    {
                        try
                        {
                            string cleanPhotoPath = photoPath.Replace("~/", "").Replace("~\\", "");
                            string fullPhotoPath = Server.MapPath("~/" + cleanPhotoPath);
                            if (File.Exists(fullPhotoPath))
                            {
                                iTextSharp.text.Image photo = iTextSharp.text.Image.GetInstance(fullPhotoPath);
                                photo.ScaleToFit(80f, 90f);
                                photo.Border = Rectangle.BOX;
                                photo.BorderWidth = 1f;
                                photo.BorderColor = BaseColor.GRAY;
                                photoCell.AddElement(photo);
                            }
                            else
                            {
                                Paragraph noPhoto = new Paragraph("APPLICANT\nPHOTO", FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY));
                                noPhoto.Alignment = Element.ALIGN_CENTER;
                                photoCell.AddElement(noPhoto);
                            }
                        }
                        catch
                        {
                            Paragraph noPhoto = new Paragraph("APPLICANT\nPHOTO", FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY));
                            noPhoto.Alignment = Element.ALIGN_CENTER;
                            photoCell.AddElement(noPhoto);
                        }
                    }
                    else
                    {
                        Paragraph noPhoto = new Paragraph("APPLICANT\nPHOTO", FontFactory.GetFont(FontFactory.HELVETICA, 10, BaseColor.GRAY));
                        noPhoto.Alignment = Element.ALIGN_CENTER;
                        photoCell.AddElement(noPhoto);
                    }
                    headerTable.AddCell(photoCell);

                    document.Add(headerTable);

                    // Application Summary Box
                    PdfPTable summaryTable = new PdfPTable(4);
                    summaryTable.WidthPercentage = 100;
                    summaryTable.SetWidths(new float[] { 1f, 1f, 1f, 1f });
                    summaryTable.SpacingAfter = 20f;

                    AddSummaryCell(summaryTable, "Application ID", applicationData["ApplicationID"].ToString(), labelFont, valueFont);
                    AddSummaryCell(summaryTable, "Application Date", Convert.ToDateTime(applicationData["ApplicationDate"]).ToString("dd-MM-yyyy"), labelFont, valueFont);
                    AddSummaryCell(summaryTable, "Position Applied", applicationData["PositionApplied"].ToString(), labelFont, valueFont);
                    AddSummaryCell(summaryTable, "Status", applicationData["Status"].ToString(), labelFont, valueFont);

                    document.Add(summaryTable);

                    // Personal Information Section
                    AddProfessionalSectionHeader(document, "PERSONAL INFORMATION", headerFont, subHeaderFont);

                    PdfPTable personalTable = new PdfPTable(4);
                    personalTable.WidthPercentage = 100;
                    personalTable.SetWidths(new float[] { 1.2f, 1.8f, 1.2f, 1.8f });
                    personalTable.SpacingAfter = 15f;

                    AddProfessionalFormRow(personalTable, "Full Name", $"{applicationData["FirstName"]} {applicationData["LastName"]}", "Father's Name", applicationData["FatherName"].ToString(), labelFont, valueFont);
                    AddProfessionalFormRow(personalTable, "Date of Birth", Convert.ToDateTime(applicationData["DateOfBirth"]).ToString("dd-MM-yyyy"), "Gender", applicationData["Gender"].ToString(), labelFont, valueFont);
                    AddProfessionalFormRow(personalTable, "Email Address", applicationData["Email"].ToString(), "Phone Number", applicationData["Phone"].ToString(), labelFont, valueFont);
                    AddProfessionalFormRow(personalTable, "Marital Status", applicationData["MaritalStatus"]?.ToString() ?? "Not Specified", "", "", labelFont, valueFont);

                    document.Add(personalTable);

                    // Address Information
                    AddProfessionalSectionHeader(document, "ADDRESS INFORMATION", headerFont, subHeaderFont);

                    PdfPTable addressTable = new PdfPTable(2);
                    addressTable.WidthPercentage = 100;
                    addressTable.SetWidths(new float[] { 1f, 3f });
                    addressTable.SpacingAfter = 15f;

                    AddProfessionalDetailRow(addressTable, "Complete Address", applicationData["Address"].ToString(), labelFont, valueFont);
                    AddProfessionalDetailRow(addressTable, "City", applicationData["City"].ToString(), labelFont, valueFont);
                    AddProfessionalDetailRow(addressTable, "State", applicationData["State"].ToString(), labelFont, valueFont);
                    AddProfessionalDetailRow(addressTable, "Pin Code", applicationData["PinCode"].ToString(), labelFont, valueFont);

                    document.Add(addressTable);

                    // Educational Information
                    AddProfessionalSectionHeader(document, "EDUCATIONAL INFORMATION", headerFont, subHeaderFont);

                    PdfPTable eduTable = new PdfPTable(4);
                    eduTable.WidthPercentage = 100;
                    eduTable.SetWidths(new float[] { 1.2f, 1.8f, 1.2f, 1.8f });
                    eduTable.SpacingAfter = 15f;

                    AddProfessionalFormRow(eduTable, "Highest Qualification", applicationData["HighestQualification"].ToString(), "Specialization", applicationData["Specialization"]?.ToString() ?? "Not Specified", labelFont, valueFont);
                    AddProfessionalFormRow(eduTable, "University/College", applicationData["University"]?.ToString() ?? "Not Specified", "Year of Passing", applicationData["YearOfPassing"]?.ToString() ?? "Not Specified", labelFont, valueFont);
                    AddProfessionalFormRow(eduTable, "Percentage/CGPA", applicationData["Percentage"]?.ToString() ?? "Not Specified", "", "", labelFont, valueFont);

                    document.Add(eduTable);

                    // Experience Information
                    AddProfessionalSectionHeader(document, "EXPERIENCE INFORMATION", headerFont, subHeaderFont);

                    PdfPTable expTable = new PdfPTable(4);
                    expTable.WidthPercentage = 100;
                    expTable.SetWidths(new float[] { 1.2f, 1.8f, 1.2f, 1.8f });
                    expTable.SpacingAfter = 15f;

                    AddProfessionalFormRow(expTable, "Total Experience", applicationData["TotalExperience"]?.ToString() ?? "Fresher", "Current Company", applicationData["CurrentCompany"]?.ToString() ?? "Not Applicable", labelFont, valueFont);
                    AddProfessionalFormRow(expTable, "Current Designation", applicationData["CurrentDesignation"]?.ToString() ?? "Not Applicable", "Current Salary", applicationData["CurrentSalary"]?.ToString() ?? "Not Specified", labelFont, valueFont);
                    AddProfessionalFormRow(expTable, "Expected Salary", applicationData["ExpectedSalary"]?.ToString() ?? "Not Specified", "", "", labelFont, valueFont);

                    document.Add(expTable);

                    // Additional Information
                    if (!string.IsNullOrEmpty(applicationData["AdditionalInfo"]?.ToString()))
                    {
                        AddProfessionalSectionHeader(document, "ADDITIONAL INFORMATION", headerFont, subHeaderFont);

                        PdfPTable additionalTable = new PdfPTable(1);
                        additionalTable.WidthPercentage = 100;
                        additionalTable.SpacingAfter = 15f;

                        PdfPCell additionalCell = new PdfPCell(new Phrase(applicationData["AdditionalInfo"].ToString(), valueFont));
                        additionalCell.Border = Rectangle.BOX;
                        additionalCell.BorderWidth = 1f;
                        additionalCell.BorderColor = new BaseColor(200, 200, 200);
                        additionalCell.Padding = 12;
                        additionalCell.MinimumHeight = 50;
                        additionalCell.BackgroundColor = new BaseColor(252, 252, 252);
                        additionalTable.AddCell(additionalCell);

                        document.Add(additionalTable);
                    }

                    // Uploaded Documents
                    AddProfessionalSectionHeader(document, "UPLOADED DOCUMENTS", headerFont, subHeaderFont);

                    PdfPTable fileTable = new PdfPTable(2);
                    fileTable.WidthPercentage = 100;
                    fileTable.SetWidths(new float[] { 1f, 3f });
                    fileTable.SpacingAfter = 20f;

                    string photoInfo = !string.IsNullOrEmpty(applicationData["PhotoPath"]?.ToString()) ?
                        Path.GetFileName(applicationData["PhotoPath"].ToString()) : "Not uploaded";
                    AddProfessionalDetailRow(fileTable, "Photograph", photoInfo, labelFont, valueFont);

                    string resumeInfo = !string.IsNullOrEmpty(applicationData["ResumePath"]?.ToString()) ?
                        Path.GetFileName(applicationData["ResumePath"].ToString()) : "Not uploaded";
                    AddProfessionalDetailRow(fileTable, "Resume/CV", resumeInfo, labelFont, valueFont);

                    document.Add(fileTable);

                    // Footer with signature area
                    PdfPTable footerTable = new PdfPTable(2);
                    footerTable.WidthPercentage = 100;
                    footerTable.SetWidths(new float[] { 1f, 1f });
                    footerTable.SpacingBefore = 30f;

                    PdfPCell applicantSigCell = new PdfPCell();
                    applicantSigCell.Border = Rectangle.NO_BORDER;
                    applicantSigCell.MinimumHeight = 60;
                    applicantSigCell.AddElement(new Paragraph("_________________________", smallFont));
                    applicantSigCell.AddElement(new Paragraph("Applicant's Signature", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.BLACK)));
                    applicantSigCell.AddElement(new Paragraph($"Date: {DateTime.Now:dd-MM-yyyy}", smallFont));
                    footerTable.AddCell(applicantSigCell);

                    PdfPCell hrSigCell = new PdfPCell();
                    hrSigCell.Border = Rectangle.NO_BORDER;
                    hrSigCell.MinimumHeight = 60;
                    hrSigCell.HorizontalAlignment = Element.ALIGN_RIGHT;
                    hrSigCell.AddElement(new Paragraph("_________________________", smallFont) { Alignment = Element.ALIGN_RIGHT });
                    hrSigCell.AddElement(new Paragraph("HR Department", FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 9, BaseColor.BLACK)) { Alignment = Element.ALIGN_RIGHT });
                    hrSigCell.AddElement(new Paragraph("For Official Use Only", smallFont) { Alignment = Element.ALIGN_RIGHT });
                    footerTable.AddCell(hrSigCell);

                    document.Add(footerTable);

                    // Final Footer
                    Paragraph finalFooter = new Paragraph("This is a computer-generated document from HMEL HR Management System.",
                        FontFactory.GetFont(FontFactory.HELVETICA_OBLIQUE, 8, BaseColor.GRAY));
                    finalFooter.Alignment = Element.ALIGN_CENTER;
                    finalFooter.SpacingBefore = 20f;
                    document.Add(finalFooter);

                    document.Close();

                    // Download PDF
                    byte[] pdfBytes = ms.ToArray();
                    string fileName = $"HMEL_JobApplication_{applicationId}_{applicationData["FirstName"]}_{applicationData["LastName"]}.pdf";

                    Response.Clear();
                    Response.ContentType = "application/pdf";
                    Response.AddHeader("Content-Disposition", $"attachment; filename={fileName}");
                    Response.BinaryWrite(pdfBytes);
                    Response.End();
                }
            }
            catch (Exception ex)
            {
                ShowMessage($"Error generating PDF: {ex.Message}", "error");
            }
        }

        // Helper methods for professional PDF formatting
        private void AddProfessionalSectionHeader(Document document, string title, Font headerFont, Font subHeaderFont)
        {
            PdfPTable headerTable = new PdfPTable(1);
            headerTable.WidthPercentage = 100;
            headerTable.SpacingAfter = 8f;

            PdfPCell headerCell = new PdfPCell(new Phrase($"  {title}", subHeaderFont));
            headerCell.BackgroundColor = new BaseColor(70, 130, 180); // Steel Blue
            headerCell.HorizontalAlignment = Element.ALIGN_LEFT;
            headerCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            headerCell.Padding = 8;
            headerCell.Border = Rectangle.BOX;
            headerCell.BorderWidth = 1f;
            headerCell.BorderColor = new BaseColor(47, 79, 79);
            headerTable.AddCell(headerCell);

            document.Add(headerTable);
        }

        private void AddSummaryCell(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            PdfPCell cell = new PdfPCell();
            cell.Border = Rectangle.BOX;
            cell.BorderWidth = 1f;
            cell.BorderColor = new BaseColor(70, 130, 180);
            cell.Padding = 8;
            cell.BackgroundColor = new BaseColor(245, 248, 252);

            Paragraph labelPara = new Paragraph(label, labelFont);
            labelPara.SpacingAfter = 3f;
            cell.AddElement(labelPara);

            Paragraph valuePara = new Paragraph(value, valueFont);
            cell.AddElement(valuePara);

            table.AddCell(cell);
        }

        private void AddProfessionalDetailRow(PdfPTable table, string label, string value, Font labelFont, Font valueFont)
        {
            PdfPCell labelCell = new PdfPCell(new Phrase(label, labelFont));
            labelCell.Border = Rectangle.BOX;
            labelCell.BorderWidth = 1f;
            labelCell.BorderColor = new BaseColor(200, 200, 200);
            labelCell.Padding = 8;
            labelCell.BackgroundColor = new BaseColor(248, 249, 250);
            labelCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(labelCell);

            PdfPCell valueCell = new PdfPCell(new Phrase(value, valueFont));
            valueCell.Border = Rectangle.BOX;
            valueCell.BorderWidth = 1f;
            valueCell.BorderColor = new BaseColor(200, 200, 200);
            valueCell.Padding = 8;
            valueCell.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(valueCell);
        }

        private void AddProfessionalFormRow(PdfPTable table, string label1, string value1, string label2, string value2, Font labelFont, Font valueFont)
        {
            // First label-value pair
            PdfPCell labelCell1 = new PdfPCell(new Phrase(label1, labelFont));
            labelCell1.Border = Rectangle.BOX;
            labelCell1.BorderWidth = 1f;
            labelCell1.BorderColor = new BaseColor(200, 200, 200);
            labelCell1.Padding = 8;
            labelCell1.BackgroundColor = new BaseColor(248, 249, 250);
            labelCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(labelCell1);

            PdfPCell valueCell1 = new PdfPCell(new Phrase(value1, valueFont));
            valueCell1.Border = Rectangle.BOX;
            valueCell1.BorderWidth = 1f;
            valueCell1.BorderColor = new BaseColor(200, 200, 200);
            valueCell1.Padding = 8;
            valueCell1.VerticalAlignment = Element.ALIGN_MIDDLE;
            table.AddCell(valueCell1);

            // Second label-value pair (if provided)
            if (!string.IsNullOrEmpty(label2))
            {
                PdfPCell labelCell2 = new PdfPCell(new Phrase(label2, labelFont));
                labelCell2.Border = Rectangle.BOX;
                labelCell2.BorderWidth = 1f;
                labelCell2.BorderColor = new BaseColor(200, 200, 200);
                labelCell2.Padding = 8;
                labelCell2.BackgroundColor = new BaseColor(248, 249, 250);
                labelCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(labelCell2);

                PdfPCell valueCell2 = new PdfPCell(new Phrase(value2, valueFont));
                valueCell2.Border = Rectangle.BOX;
                valueCell2.BorderWidth = 1f;
                valueCell2.BorderColor = new BaseColor(200, 200, 200);
                valueCell2.Padding = 8;
                valueCell2.VerticalAlignment = Element.ALIGN_MIDDLE;
                table.AddCell(valueCell2);
            }
            else
            {
                // Empty cells for alignment
                PdfPCell emptyCell1 = new PdfPCell(new Phrase("", valueFont));
                emptyCell1.Border = Rectangle.BOX;
                emptyCell1.BorderWidth = 1f;
                emptyCell1.BorderColor = new BaseColor(200, 200, 200);
                emptyCell1.Padding = 8;
                emptyCell1.BackgroundColor = new BaseColor(248, 249, 250);
                table.AddCell(emptyCell1);

                PdfPCell emptyCell2 = new PdfPCell(new Phrase("", valueFont));
                emptyCell2.Border = Rectangle.BOX;
                emptyCell2.BorderWidth = 1f;
                emptyCell2.BorderColor = new BaseColor(200, 200, 200);
                emptyCell2.Padding = 8;
                table.AddCell(emptyCell2);
            }
        }

        private DataRow GetApplicationData(int applicationId)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT * FROM JobApplications WHERE ApplicationID = @ApplicationID";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    using (MySqlDataAdapter adapter = new MySqlDataAdapter(command))
                    {
                        DataTable dt = new DataTable();
                        adapter.Fill(dt);
                        return dt.Rows.Count > 0 ? dt.Rows[0] : null;
                    }
                }
            }
        }

        protected void btnDownloadFromModal_Click(object sender, EventArgs e)
        {
            if (!string.IsNullOrEmpty(hdnSelectedApplicationId.Value))
            {
                int applicationId = Convert.ToInt32(hdnSelectedApplicationId.Value);
                GenerateAndDownloadPDF(applicationId);
            }
        }

        private void ShowStatusModal(string action)
        {
            txtStatusComments.Text = "";
            string script = $@"
                document.querySelector('#statusModal .modal-title').textContent = '📝 {action}';
                showStatusModal();
            ";
            ClientScript.RegisterStartupScript(this.GetType(), "ShowStatusModal", script, true);
        }

        protected void btnConfirmStatusUpdate_Click(object sender, EventArgs e)
        {
            int applicationId = Convert.ToInt32(hdnSelectedApplicationId.Value);
            string action = hdnSelectedAction.Value;
            string comments = txtStatusComments.Text.Trim();

            string newStatus = "";
            string newStage = "";

            switch (action)
            {
                case "Stage1":
                    newStatus = "Stage 1 - Online Assessment";
                    newStage = "Assessment";
                    break;
                case "Stage2":
                    newStatus = "Stage 2 - Interview";
                    newStage = "Interview";
                    break;
                case "Reject":
                    newStatus = "Rejected";
                    newStage = "Rejected";
                    break;
            }

            if (UpdateApplicationStatus(applicationId, newStatus, newStage, comments))
            {
                ShowMessage("Application status updated successfully!", "success");
                LoadStatistics();
                LoadApplications();
            }
            else
            {
                ShowMessage("Failed to update application status. Please try again.", "error");
            }

            string hideScript = "hideStatusModal();";
            ClientScript.RegisterStartupScript(this.GetType(), "HideStatusModal", hideScript, true);
        }

        private bool UpdateApplicationStatus(int applicationId, string newStatus, string newStage, string comments)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                connection.Open();
                using (MySqlTransaction transaction = connection.BeginTransaction())
                {
                    try
                    {
                        // Get current status for logging
                        string currentStatus = "";
                        string currentStage = "";
                        string getCurrentQuery = "SELECT Status, Stage FROM JobApplications WHERE ApplicationID = @ApplicationID";
                        using (MySqlCommand getCurrentCmd = new MySqlCommand(getCurrentQuery, connection, transaction))
                        {
                            getCurrentCmd.Parameters.AddWithValue("@ApplicationID", applicationId);
                            using (MySqlDataReader reader = getCurrentCmd.ExecuteReader())
                            {
                                if (reader.Read())
                                {
                                    currentStatus = reader["Status"].ToString();
                                    currentStage = reader["Stage"].ToString();
                                }
                            }
                        }

                        // Update application status
                        string updateQuery = @"
                            UPDATE JobApplications 
                            SET Status = @Status, Stage = @Stage, ReviewedBy = @ReviewedBy, 
                                ReviewedDate = @ReviewedDate, ReviewComments = @Comments,
                                ModifiedDate = @ModifiedDate
                            WHERE ApplicationID = @ApplicationID";

                        using (MySqlCommand updateCmd = new MySqlCommand(updateQuery, connection, transaction))
                        {
                            updateCmd.Parameters.AddWithValue("@Status", newStatus);
                            updateCmd.Parameters.AddWithValue("@Stage", newStage);
                            updateCmd.Parameters.AddWithValue("@ReviewedBy", GetCurrentAdminId());
                            updateCmd.Parameters.AddWithValue("@ReviewedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@Comments", comments);
                            updateCmd.Parameters.AddWithValue("@ModifiedDate", DateTime.Now);
                            updateCmd.Parameters.AddWithValue("@ApplicationID", applicationId);

                            updateCmd.ExecuteNonQuery();
                        }

                        // Log the status change
                        string logQuery = @"
                            INSERT INTO ApplicationStatusLog 
                            (ApplicationID, PreviousStatus, NewStatus, PreviousStage, NewStage, ChangedBy, Comments)
                            VALUES (@ApplicationID, @PreviousStatus, @NewStatus, @PreviousStage, @NewStage, @ChangedBy, @Comments)";

                        using (MySqlCommand logCmd = new MySqlCommand(logQuery, connection, transaction))
                        {
                            logCmd.Parameters.AddWithValue("@ApplicationID", applicationId);
                            logCmd.Parameters.AddWithValue("@PreviousStatus", currentStatus);
                            logCmd.Parameters.AddWithValue("@NewStatus", newStatus);
                            logCmd.Parameters.AddWithValue("@PreviousStage", currentStage);
                            logCmd.Parameters.AddWithValue("@NewStage", newStage);
                            logCmd.Parameters.AddWithValue("@ChangedBy", GetCurrentAdminId());
                            logCmd.Parameters.AddWithValue("@Comments", comments);

                            logCmd.ExecuteNonQuery();
                        }

                        transaction.Commit();
                        return true;
                    }
                    catch
                    {
                        transaction.Rollback();
                        return false;
                    }
                }
            }
        }

        private int GetCurrentAdminId()
        {
            string username = Session["AdminUsername"]?.ToString();
            if (string.IsNullOrEmpty(username)) return 1; // Default admin

            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string query = "SELECT AdminID FROM AdminUsers WHERE Username = @Username";
                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : 1;
                }
            }
        }

        public string GetStatusClass(string status)
        {
            switch (status?.ToLower())
            {
                case "submitted":
                    return "status-submitted";
                case "stage 1 - online assessment":
                    return "status-stage1";
                case "stage 2 - interview":
                    return "status-stage2";
                case "rejected":
                    return "status-rejected";
                case "draft":
                    return "status-draft";
                default:
                    return "status-submitted";
            }
        }

        protected void ApplyFilters(object sender, EventArgs e)
        {
            LoadApplications();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadStatistics();
            LoadApplications();
            ShowMessage("Data refreshed successfully!", "info");
        }

        protected void btnLogout_Click(object sender, EventArgs e)
        {
            Session.Clear();
            Session.Abandon();
            Response.Redirect("Default.aspx");
        }

        protected void gvApplications_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            // Additional row formatting can be added here if needed
        }

        protected void gvApplications_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvApplications.PageIndex = e.NewPageIndex;
            LoadApplications();
        }

        private void ShowMessage(string message, string type)
        {
            string cssClass = "";
            switch (type)
            {
                case "success":
                    cssClass = "message-success";
                    break;
                case "error":
                    cssClass = "message-error";
                    break;
                case "info":
                    cssClass = "message-info";
                    break;
            }

            lblMessage.Text = $"<div class='message {cssClass}'>{message}</div>";
        }
    }
}
