# HMEL Careers Portal – Internship Project

> 🏢 Summer Internship Project at **HPCL-Mittal Energy Limited (HMEL)**  

---

## 📌 Project Overview

The **HMEL Careers Portal** is a web-based application built using **ASP.NET WebForms** and **MySQL**, developed as part of a summer internship at HPCL-Mittal Energy Limited. It aims to digitize and streamline internal job postings, applicant management, and resume handling across different user roles (Admin, Applicant, and Employer).

---

## 🧰 Technologies Used

| Category         | Tools / Frameworks              |
|------------------|----------------------------------|
| Frontend         | ASP.NET WebForms, HTML, CSS     |
| Backend          | C# (.NET Framework)             |
| Database         | MySQL                           |
| IDE              | Visual Studio 2022              |
| Version Control  | Git, GitHub                     |

---

## 📁 Project Structure

```
HMEL_Project/
│
├── App_Data/                    # Application database folder
├── bin/                         # Build binaries
├── Images/                      # Image resources
├── Models/                      # (Optional) C# data models
├── obj/                         # Build-related temp files
│
├── scripts/                     # SQL Scripts for DB setup
│   ├── add-constraints-and-indexes.sql
│   ├── admin-functionality-update.sql
│   ├── create-mysql-database.sql
│   ├── create-status-log-table.sql
│   └── update-database-admin.sql
│
├── Uploads/                     # Uploaded files from users
│   ├── Photos/
│   └── Resumes/
│
├── AdminDashboard.aspx          # Admin dashboard UI
├── AdminDashboard.aspx.cs
├── AdminDashboard.aspx.designer.cs
│
├── Dashboard.aspx               # Shared user dashboard
│
├── Default.aspx                 # Home or landing page
├── Default.aspx.cs
├── Default.aspx.designer.cs
│
├── FileHandler.ashx             # Async file handler (e.g., resumes)
│
├── FileViewer.aspx              # Resume or photo viewer
├── FileViewer.aspx.cs
├── FileViewer.aspx.designer.cs
│
├── FirstPage.aspx               # First-time landing/intro
├── FirstPage.aspx.cs
├── FirstPage.aspx.designer.cs
│
├── Global.asax                  # App-level config and routing
├── Web.config                   # Application and DB configuration
└── packages.config              # NuGet dependencies
```

---

## ✅ Core Features

- 🔐 **Role-Based Login**
  - Admins can post jobs and view applicants
  - Applicants can upload resumes and view listings

- 📝 **Resume Uploading**
  - Integrated upload functionality with file validation

- 🗂️ **File Viewer**
  - Preview uploaded resumes or profile images

- ⚙️ **Custom SQL Scripts**
  - DB setup, constraints, and admin-specific updates

- 🌐 **Session Management & Routing**
  - Maintains secure navigation flow and login status

---

## 🚀 How to Run the Project

1. **Clone the Repository**
   ```bash
   git clone https://github.com/vaibhavporwal01/HMEL_Project.git
   ```

2. **Open in Visual Studio**
   - Open the `.sln` or full folder in Visual Studio 2022

3. **Set Up MySQL Database**
   - Use files under `/scripts` to:
     - Create the database
     - Add tables, constraints, and seed data

4. **Configure Database Connection**
   - Edit `Web.config` and set your MySQL connection string

5. **Run on Localhost/IIS Express**
   - Press `F5` or use Visual Studio's IIS Express button

---

## 🎯 Internship Outcomes

- Built a scalable and modular web system using ASP.NET WebForms
- Handled real-world database integration using SQL scripts
- Managed file upload, validation, and viewing
- Understood session-based authentication and routing
- Gained experience working under enterprise development workflows

---


