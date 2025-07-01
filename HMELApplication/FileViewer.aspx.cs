using System;
using System.IO;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace HMELApplication
{
    public partial class FileViewer : System.Web.UI.Page
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HMELConnectionString"].ConnectionString;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                string type = Request.QueryString["type"];
                string idStr = Request.QueryString["id"];
                string download = Request.QueryString["download"];

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(idStr))
                {
                    Response.StatusCode = 400;
                    Response.Write("Invalid parameters");
                    return;
                }

                int applicationId = Convert.ToInt32(idStr);
                string filePath = GetFilePath(applicationId, type);

                if (string.IsNullOrEmpty(filePath))
                {
                    Response.StatusCode = 404;
                    Response.Write("File not found in database");
                    return;
                }

                // Clean the file path
                string cleanPath = filePath.Replace("~/", "").Replace("~\\", "");
                string fullPath = Server.MapPath("~/" + cleanPath);

                if (!File.Exists(fullPath))
                {
                    Response.StatusCode = 404;
                    Response.Write($"File not found on server: {fullPath}");
                    return;
                }

                string fileName = Path.GetFileName(fullPath);
                string contentType = GetContentType(Path.GetExtension(fullPath));

                Response.Clear();
                Response.ContentType = contentType;

                if (download == "1")
                {
                    Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                }
                else
                {
                    Response.AddHeader("Content-Disposition", $"inline; filename=\"{fileName}\"");
                }

                Response.WriteFile(fullPath);
                Response.End();
            }
            catch (Exception ex)
            {
                Response.StatusCode = 500;
                Response.Write($"Error: {ex.Message}");
            }
        }

        private string GetFilePath(int applicationId, string type)
        {
            using (MySqlConnection connection = new MySqlConnection(connectionString))
            {
                string column = type == "photo" ? "PhotoPath" : "ResumePath";
                string query = $"SELECT {column} FROM JobApplications WHERE ApplicationID = @ApplicationID";

                using (MySqlCommand command = new MySqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ApplicationID", applicationId);
                    connection.Open();
                    object result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }

        private string GetContentType(string extension)
        {
            switch (extension.ToLower())
            {
                case ".pdf":
                    return "application/pdf";
                case ".doc":
                    return "application/msword";
                case ".docx":
                    return "application/vnd.openxmlformats-officedocument.wordprocessingml.document";
                case ".jpg":
                case ".jpeg":
                    return "image/jpeg";
                case ".png":
                    return "image/png";
                case ".gif":
                    return "image/gif";
                case ".txt":
                    return "text/plain";
                default:
                    return "application/octet-stream";
            }
        }
    }
}
