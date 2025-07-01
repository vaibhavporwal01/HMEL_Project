using System;
using System.IO;
using System.Web;
using System.Configuration;
using MySql.Data.MySqlClient;

namespace HMELApplication
{
    public class FileHandler : IHttpHandler
    {
        private string connectionString = ConfigurationManager.ConnectionStrings["HMELConnectionString"].ConnectionString;

        public void ProcessRequest(HttpContext context)
        {
            try
            {
                string type = context.Request.QueryString["type"];
                string idStr = context.Request.QueryString["id"];
                string download = context.Request.QueryString["download"];

                if (string.IsNullOrEmpty(type) || string.IsNullOrEmpty(idStr))
                {
                    context.Response.StatusCode = 400;
                    context.Response.Write("Invalid parameters");
                    return;
                }

                int applicationId = Convert.ToInt32(idStr);
                string filePath = GetFilePath(applicationId, type);

                if (string.IsNullOrEmpty(filePath))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write("File not found in database");
                    return;
                }

                // Clean the file path
                string cleanPath = filePath.Replace("~/", "").Replace("~\\", "");
                string fullPath = context.Server.MapPath("~/" + cleanPath);

                if (!File.Exists(fullPath))
                {
                    context.Response.StatusCode = 404;
                    context.Response.Write($"File not found on server: {fullPath}");
                    return;
                }

                string fileName = Path.GetFileName(fullPath);
                string contentType = GetContentType(Path.GetExtension(fullPath));

                context.Response.Clear();
                context.Response.ContentType = contentType;

                if (download == "1")
                {
                    context.Response.AddHeader("Content-Disposition", $"attachment; filename=\"{fileName}\"");
                }
                else
                {
                    context.Response.AddHeader("Content-Disposition", $"inline; filename=\"{fileName}\"");
                }

                context.Response.WriteFile(fullPath);
                context.Response.End();
            }
            catch (Exception ex)
            {
                context.Response.StatusCode = 500;
                context.Response.Write($"Error: {ex.Message}");
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

        public bool IsReusable
        {
            get { return false; }
        }
    }
}
