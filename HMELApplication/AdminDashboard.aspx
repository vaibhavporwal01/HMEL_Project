<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="AdminDashboard.aspx.cs" Inherits="HMELApplication.AdminDashboard" %>
//Vaibhav Porwal

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">

<head runat="server">

    <title>HMEL - Admin Dashboard</title>

    <meta name="viewport" content="width=device-width, initial-scale=1.0"/>

    <style type="text/css">

        * {

            margin: 0;

            padding: 0;

            box-sizing: border-box;

        }

        body {

            font-family: 'Inter', 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;

            background: linear-gradient(135deg, #f8fafc 0%, #e0f2fe 100%);

            min-height: 100vh;

            color: #334155;

        }

        /* Header Styles */

        .header {

            background: linear-gradient(135deg, #1e293b 0%, #0f172a 100%);

            color: white;

            padding: 1rem 2rem;

            box-shadow: 0 4px 20px rgba(0,0,0,0.1);

        }

        .header-content {

            max-width: 1400px;

            margin: 0 auto;

            display: flex;

            justify-content: space-between;

            align-items: center;

        }

        .logo-section {

            display: flex;

            align-items: center;

            gap: 1rem;

        }

        .company-logo {

            height: 40px;

            width: auto;

        }

        .admin-title {

            font-size: 1.5rem;

            font-weight: bold;

            color: #f59e0b;

        }

        .user-section {

            display: flex;

            align-items: center;

            gap: 1rem;

        }

        .user-info {

            text-align: right;

        }

        .user-name {

            font-weight: 600;

            color: #f8fafc;

        }

        .user-role {

            font-size: 0.875rem;

            color: #cbd5e1;

        }

        /* Main Container */

        .main-container {

            max-width: 1400px;

            margin: 0 auto;

            padding: 2rem;

        }

        /* Stats Cards */

        .stats-grid {

            display: grid;

            grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));

            gap: 1.5rem;

            margin-bottom: 2rem;

        }

        .stat-card {

            background: white;

            border-radius: 12px;

            padding: 1.5rem;

            box-shadow: 0 4px 20px rgba(0,0,0,0.08);

            border-left: 4px solid;

            transition: all 0.3s ease;

        }

        .stat-card:hover {

            transform: translateY(-2px);

            box-shadow: 0 8px 30px rgba(0,0,0,0.12);

        }

        .stat-card.total { border-left-color: #3b82f6; }

        .stat-card.pending { border-left-color: #f59e0b; }

        .stat-card.approved { border-left-color: #10b981; }

        .stat-card.rejected { border-left-color: #ef4444; }

        .stat-number {

            font-size: 2rem;

            font-weight: bold;

            margin-bottom: 0.5rem;

        }

        .stat-label {

            color: #64748b;

            font-size: 0.875rem;

            font-weight: 500;

        }

        /* Applications Table */

        .applications-section {

            background: white;

            border-radius: 12px;

            box-shadow: 0 4px 20px rgba(0,0,0,0.08);

            overflow: hidden;

        }

        .section-header {

            background: linear-gradient(135deg, #1e293b 0%, #334155 100%);

            color: white;

            padding: 1.5rem;

            display: flex;

            justify-content: space-between;

            align-items: center;

        }

        .section-title {

            font-size: 1.25rem;

            font-weight: 600;

        }

        .filters-section {

            padding: 1.5rem;

            border-bottom: 1px solid #e2e8f0;

            background: #f8fafc;

        }

        .filters-grid {

            display: grid;

            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));

            gap: 1rem;

            align-items: end;

        }

        .filter-group {

            display: flex;

            flex-direction: column;

        }

        .filter-label {

            font-weight: 600;

            color: #374151;

            margin-bottom: 0.5rem;

            font-size: 0.875rem;

        }

        .filter-select, .filter-input {

            padding: 0.75rem;

            border: 2px solid #e5e7eb;

            border-radius: 6px;

            font-size: 0.875rem;

            transition: all 0.3s ease;

        }

        .filter-select:focus, .filter-input:focus {

            outline: none;

            border-color: #3b82f6;

            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);

        }

        /* Table Styles */

        .table-container {

            overflow-x: auto;

        }

        .applications-table {

            width: 100%;

            border-collapse: collapse;

        }

        .applications-table th {

            background: #f1f5f9;

            padding: 1rem;

            text-align: left;

            font-weight: 600;

            color: #374151;

            border-bottom: 2px solid #e2e8f0;

            font-size: 0.875rem;

        }

        .applications-table td {

            padding: 1rem;

            border-bottom: 1px solid #e2e8f0;

            font-size: 0.875rem;

        }

        .applications-table tr:hover {

            background: #f8fafc;

        }

        /* Status Badges */

        .status-badge {

            padding: 0.5rem 1rem;

            border-radius: 20px;

            font-size: 0.75rem;

            font-weight: 600;

            text-transform: uppercase;

            border: 1px solid;

            display: inline-block;

        }

        .status-submitted {

            background: #dbeafe;

            color: #1e40af;

            border-color: #93c5fd;

        }

        .status-stage1 {

            background: #fef3c7;

            color: #92400e;

            border-color: #fbbf24;

        }

        .status-stage2 {

            background: #d1fae5;

            color: #065f46;

            border-color: #34d399;

        }

        .status-rejected {

            background: #fee2e2;

            color: #991b1b;

            border-color: #fca5a5;

        }

        .status-draft {

            background: #f3f4f6;

            color: #374151;

            border-color: #d1d5db;

        }

        /* Action Buttons */

        .action-buttons {

            display: flex;

            gap: 0.5rem;

            flex-wrap: wrap;

        }

        .btn {

            padding: 0.5rem 1rem;

            border: none;

            border-radius: 6px;

            font-size: 0.75rem;

            font-weight: 600;

            cursor: pointer;

            transition: all 0.3s ease;

            text-decoration: none;

            display: inline-block;

            text-align: center;

        }

        .btn-view {

            background: #3b82f6;

            color: white;

        }

        .btn-view:hover {

            background: #2563eb;

            transform: translateY(-1px);

        }

        .btn-approve1 {

            background: #f59e0b;

            color: white;

        }

        .btn-approve1:hover {

            background: #d97706;

            transform: translateY(-1px);

        }

        .btn-approve2 {

            background: #10b981;

            color: white;

        }

        .btn-approve2:hover {

            background: #059669;

            transform: translateY(-1px);

        }

        .btn-reject {

            background: #ef4444;

            color: white;

        }

        .btn-reject:hover {

            background: #dc2626;

            transform: translateY(-1px);

        }

        .btn-secondary {

            background: #64748b;

            color: white;

        }

        .btn-secondary:hover {

            background: #475569;

        }

        .btn-download {

            background: #8b5cf6;

            color: white;

        }

        .btn-download:hover {

            background: #7c3aed;

            transform: translateY(-1px);

        }

        /* Modal Styles */

        .modal-overlay {

            position: fixed;

            top: 0;

            left: 0;

            right: 0;

            bottom: 0;

            background: rgba(0,0,0,0.5);

            display: none;

            align-items: center;

            justify-content: center;

            z-index: 2000;

        }

        .modal-content {

            background: white;

            border-radius: 15px;

            padding: 2rem;

            max-width: 800px;

            width: 90%;

            max-height: 90vh;

            overflow-y: auto;

        }

        .modal-header {

            text-align: center;

            margin-bottom: 2rem;

        }

        .modal-title {

            font-size: 1.5rem;

            font-weight: bold;

            color: #1e293b;

            margin-bottom: 0.5rem;

        }

        .form-group {

            margin-bottom: 1.5rem;

        }

        .form-label {

            display: block;

            font-weight: 600;

            color: #374151;

            margin-bottom: 0.5rem;

            font-size: 0.875rem;

        }

        .form-input, .form-textarea {

            width: 100%;

            padding: 0.75rem;

            border: 2px solid #e5e7eb;

            border-radius: 6px;

            font-size: 0.875rem;

            transition: all 0.3s ease;

        }

        .form-input:focus, .form-textarea:focus {

            outline: none;

            border-color: #3b82f6;

            box-shadow: 0 0 0 3px rgba(59, 130, 246, 0.1);

        }

        .form-textarea {

            height: 100px;

            resize: vertical;

            font-family: inherit;

        }

        .modal-actions {

            display: flex;

            gap: 1rem;

            margin-top: 2rem;

            flex-wrap: wrap;

        }

        /* File Display Styles */

        .files-section {

            margin-top: 2rem;

            padding-top: 2rem;

            border-top: 2px solid #e2e8f0;

        }

        .files-grid {

            display: grid;

            grid-template-columns: repeat(auto-fit, minmax(300px, 1fr));

            gap: 2rem;

            margin-top: 1rem;

        }

        .file-card {

            background: #f8fafc;

            border: 2px solid #e2e8f0;

            border-radius: 12px;

            padding: 1.5rem;

            text-align: center;

        }

        .file-card h4 {

            color: #374151;

            margin-bottom: 1rem;

            font-size: 1.1rem;

        }

        .photo-container {

            margin-bottom: 1rem;

        }

        .applicant-photo {

            max-width: 150px;

            max-height: 200px;

            border-radius: 8px;

            box-shadow: 0 4px 12px rgba(0,0,0,0.1);

            border: 3px solid #e2e8f0;

        }

        .no-photo {

            width: 150px;

            height: 200px;

            background: #e2e8f0;

            border-radius: 8px;

            display: flex;

            align-items: center;

            justify-content: center;

            color: #64748b;

            font-size: 0.875rem;

            margin: 0 auto;

        }

        .resume-container {

            display: flex;

            flex-direction: column;

            gap: 1rem;

        }

        .file-info {

            background: white;

            padding: 1rem;

            border-radius: 8px;

            border: 1px solid #e2e8f0;

        }

        .file-name {

            font-weight: 600;

            color: #374151;

            margin-bottom: 0.5rem;

        }

        .file-actions {

            display: flex;

            gap: 0.5rem;

            justify-content: center;

            flex-wrap: wrap;

        }

        .btn-file {

            padding: 0.5rem 1rem;

            border: none;

            border-radius: 6px;

            font-size: 0.75rem;

            font-weight: 600;

            cursor: pointer;

            transition: all 0.3s ease;

            text-decoration: none;

            display: inline-block;

        }

        .btn-view-file {

            background: #3b82f6;

            color: white;

        }

        .btn-view-file:hover {

            background: #2563eb;

        }

        .btn-download-file {

            background: #10b981;

            color: white;

        }

        .btn-download-file:hover {

            background: #059669;

        }

        /* Responsive Design */

        @media (max-width: 1024px) {

            .main-container {

                padding: 1rem;

            }

            

            .header-content {

                flex-direction: column;

                gap: 1rem;

            }

        }

        @media (max-width: 768px) {

            .stats-grid {

                grid-template-columns: 1fr;

            }

            

            .filters-grid {

                grid-template-columns: 1fr;

            }

            

            .action-buttons {

                flex-direction: column;

            }

            

            .applications-table {

                font-size: 0.75rem;

            }

            

            .applications-table th,

            .applications-table td {

                padding: 0.5rem;

            }

            .files-grid {

                grid-template-columns: 1fr;

            }

            .modal-actions {

                flex-direction: column;

            }

        }

        /* Success/Error Messages */

        .message {

            padding: 1rem;

            border-radius: 8px;

            margin-bottom: 1rem;

            font-weight: 600;

        }

        .message-success {

            background: #d1fae5;

            color: #065f46;

            border: 1px solid #34d399;

        }

        .message-error {

            background: #fee2e2;

            color: #991b1b;

            border: 1px solid #fca5a5;

        }

        .message-info {

            background: #dbeafe;

            color: #1e40af;

            border: 1px solid #93c5fd;

        }

        /* Priority Indicators */

        .priority-high {

            color: #ef4444;

            font-weight: bold;

        }

        .priority-normal {

            color: #64748b;

        }

        .priority-low {

            color: #10b981;

        }

        /* Application Details */

        .detail-grid {

            display: grid;

            grid-template-columns: repeat(auto-fit, minmax(200px, 1fr));

            gap: 1rem;

            margin-bottom: 1rem;

        }

        .detail-item {

            display: flex;

            flex-direction: column;

        }

        .detail-label {

            font-weight: 600;

            color: #374151;

            font-size: 0.75rem;

            text-transform: uppercase;

            margin-bottom: 0.25rem;

        }

        .detail-value {

            color: #1f2937;

            font-size: 0.875rem;

        }

    </style>

</head>

<body>

    <form id="form1" runat="server">

        <!-- Header -->

        <div class="header">

            <div class="header-content">

                <div class="logo-section">

                    <img src="Images/hmel-logo.png" alt="HMEL" class="company-logo" />

                    <div class="admin-title">Admin Dashboard</div>

                </div>

                <div class="user-section">

                    <div class="user-info">

                        <div class="user-name">

                            <asp:Label ID="lblAdminName" runat="server" Text="Administrator"></asp:Label>

                        </div>

                        <div class="user-role">System Administrator</div>

                    </div>

                    <asp:Button ID="btnLogout" runat="server" Text="Logout" CssClass="btn btn-secondary" OnClick="btnLogout_Click" />

                </div>

            </div>

        </div>

        <!-- Main Content -->

        <div class="main-container">

            <!-- Success/Error Messages -->

            <asp:Label ID="lblMessage" runat="server"></asp:Label>

            <!-- Statistics Cards -->

            <div class="stats-grid">

                <div class="stat-card total">

                    <div class="stat-number">

                        <asp:Label ID="lblTotalApplications" runat="server" Text="0"></asp:Label>

                    </div>

                    <div class="stat-label">Total Applications</div>

                </div>

                <div class="stat-card pending">

                    <div class="stat-number">

                        <asp:Label ID="lblPendingApplications" runat="server" Text="0"></asp:Label>

                    </div>

                    <div class="stat-label">Pending Review</div>

                </div>

                <div class="stat-card approved">

                    <div class="stat-number">

                        <asp:Label ID="lblApprovedApplications" runat="server" Text="0"></asp:Label>

                    </div>

                    <div class="stat-label">Approved</div>

                </div>

                <div class="stat-card rejected">

                    <div class="stat-number">

                        <asp:Label ID="lblRejectedApplications" runat="server" Text="0"></asp:Label>

                    </div>

                    <div class="stat-label">Rejected</div>

                </div>

            </div>

            <!-- Applications Section -->

            <div class="applications-section">

                <div class="section-header">

                    <div class="section-title">📋 Job Applications Management</div>

                    <asp:Button ID="btnRefresh" runat="server" Text="🔄 Refresh" CssClass="btn btn-secondary" OnClick="btnRefresh_Click" />

                </div>

                <!-- Filters -->

                <div class="filters-section">

                    <div class="filters-grid">

                        <div class="filter-group">

                            <label class="filter-label">Status Filter</label>

                            <asp:DropDownList ID="ddlStatusFilter" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="ApplyFilters">

                                <asp:ListItem Value="">All Status</asp:ListItem>

                                <asp:ListItem Value="Submitted">Submitted</asp:ListItem>

                                <asp:ListItem Value="Stage 1 - Online Assessment">Stage 1 - Online Assessment</asp:ListItem>

                                <asp:ListItem Value="Stage 2 - Interview">Stage 2 - Interview</asp:ListItem>

                                <asp:ListItem Value="Rejected">Rejected</asp:ListItem>

                                <asp:ListItem Value="Draft">Draft</asp:ListItem>

                            </asp:DropDownList>

                        </div>

                        <div class="filter-group">

                            <label class="filter-label">Position Filter</label>

                            <asp:DropDownList ID="ddlPositionFilter" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="ApplyFilters">

                                <asp:ListItem Value="">All Positions</asp:ListItem>

                                <asp:ListItem Value="Process Engineer">Process Engineer</asp:ListItem>

                                <asp:ListItem Value="Mechanical Engineer">Mechanical Engineer</asp:ListItem>

                                <asp:ListItem Value="Electrical Engineer">Electrical Engineer</asp:ListItem>

                                <asp:ListItem Value="Chemical Engineer">Chemical Engineer</asp:ListItem>

                                <asp:ListItem Value="Safety Officer">Safety Officer</asp:ListItem>

                                <asp:ListItem Value="Other">Other</asp:ListItem>

                            </asp:DropDownList>

                        </div>

                        <div class="filter-group">

                            <label class="filter-label">Date Range</label>

                            <asp:DropDownList ID="ddlDateFilter" runat="server" CssClass="filter-select" AutoPostBack="true" OnSelectedIndexChanged="ApplyFilters">

                                <asp:ListItem Value="">All Dates</asp:ListItem>

                                <asp:ListItem Value="Today">Today</asp:ListItem>

                                <asp:ListItem Value="Last 7 Days">Last 7 Days</asp:ListItem>

                                <asp:ListItem Value="Last 30 Days">Last 30 Days</asp:ListItem>

                                <asp:ListItem Value="Last 90 Days">Last 90 Days</asp:ListItem>

                            </asp:DropDownList>

                        </div>

                        <div class="filter-group">

                            <label class="filter-label">Search</label>

                            <asp:TextBox ID="txtSearch" runat="server" CssClass="filter-input" placeholder="Search by name, email..." AutoPostBack="true" OnTextChanged="ApplyFilters"></asp:TextBox>

                        </div>

                    </div>

                </div>

                <!-- Applications Table -->

                <div class="table-container">

                    <asp:GridView ID="gvApplications" runat="server" CssClass="applications-table" 

                        AutoGenerateColumns="false" OnRowCommand="gvApplications_RowCommand" 

                        OnRowDataBound="gvApplications_RowDataBound" AllowPaging="true" PageSize="10" 

                        OnPageIndexChanging="gvApplications_PageIndexChanging">

                        <Columns>

                            <asp:BoundField DataField="ApplicationID" HeaderText="ID" />

                            <asp:TemplateField HeaderText="Applicant">

                                <ItemTemplate>

                                    <div>

                                        <strong><%# Eval("FirstName") %> <%# Eval("LastName") %></strong><br />

                                        <small style="color: #64748b;"><%# Eval("Email") %></small>

                                    </div>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:BoundField DataField="PositionApplied" HeaderText="Position" />

                            <asp:TemplateField HeaderText="Status">

                                <ItemTemplate>

                                    <span class='status-badge <%# GetStatusClass(Eval("Status").ToString()) %>'>

                                        <%# Eval("Status") %>

                                    </span>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Applied Date">

                                <ItemTemplate>

                                    <%# Convert.ToDateTime(Eval("ApplicationDate")).ToString("dd/MM/yyyy") %><br />

                                    <small style="color: #64748b;"><%# Convert.ToDateTime(Eval("ApplicationDate")).ToString("HH:mm") %></small>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Experience">

                                <ItemTemplate>

                                    <%# Eval("TotalExperience") ?? "Fresher" %>

                                </ItemTemplate>

                            </asp:TemplateField>

                            <asp:TemplateField HeaderText="Actions">

                                <ItemTemplate>

                                    <div class="action-buttons">

                                        <asp:Button ID="btnView" runat="server" Text="👁️ View" CssClass="btn btn-view" 

                                            CommandName="ViewApplication" CommandArgument='<%# Eval("ApplicationID") %>' />

                                        <asp:Button ID="btnDownloadPDF" runat="server" Text="📄 PDF" CssClass="btn btn-download" 

                                            CommandName="DownloadPDF" CommandArgument='<%# Eval("ApplicationID") %>' />

                                        <asp:Button ID="btnApproveStage1" runat="server" Text="✅ Stage 1" CssClass="btn btn-approve1" 

                                            CommandName="ApproveStage1" CommandArgument='<%# Eval("ApplicationID") %>' 

                                            Visible='<%# Eval("Status").ToString() == "Submitted" %>' />

                                        <asp:Button ID="btnApproveStage2" runat="server" Text="🎯 Stage 2" CssClass="btn btn-approve2" 

                                            CommandName="ApproveStage2" CommandArgument='<%# Eval("ApplicationID") %>' 

                                            Visible='<%# Eval("Status").ToString() == "Stage 1 - Online Assessment" %>' />

                                        <asp:Button ID="btnReject" runat="server" Text="❌ Reject" CssClass="btn btn-reject" 

                                            CommandName="RejectApplication" CommandArgument='<%# Eval("ApplicationID") %>' 

                                            Visible='<%# Eval("Status").ToString() != "Rejected" %>' 

                                            OnClientClick="return confirm('Are you sure you want to reject this application?');" />

                                    </div>

                                </ItemTemplate>

                            </asp:TemplateField>

                        </Columns>

                        <PagerStyle CssClass="pager" />

                    </asp:GridView>

                </div>

            </div>

        </div>

        <!-- Application Details Modal -->

        <div class="modal-overlay" id="applicationModal">

            <div class="modal-content">

                <div class="modal-header">

                    <h3 class="modal-title">📋 Application Details</h3>

                </div>

                

                <div id="applicationDetails">

                    <!-- Application details will be loaded here -->

                </div>

                

                <div class="modal-actions">

                    <button type="button" class="btn btn-secondary" onclick="hideApplicationModal()">Close</button>

                    <asp:Button ID="btnDownloadFromModal" runat="server" Text="📄 Download PDF" CssClass="btn btn-download" OnClick="btnDownloadFromModal_Click" />

                </div>

            </div>

        </div>

        <!-- Status Update Modal -->

        <div class="modal-overlay" id="statusModal">

            <div class="modal-content">

                <div class="modal-header">

                    <h3 class="modal-title">📝 Update Application Status</h3>

                </div>

                

                <div class="form-group">

                    <label class="form-label">Comments</label>

                    <asp:TextBox ID="txtStatusComments" runat="server" CssClass="form-textarea" TextMode="MultiLine" 

                        placeholder="Enter comments about this status change..."></asp:TextBox>

                </div>

                

                <div class="modal-actions">

                    <button type="button" class="btn btn-secondary" onclick="hideStatusModal()">Cancel</button>

                    <asp:Button ID="btnConfirmStatusUpdate" runat="server" Text="Confirm" CssClass="btn btn-view" OnClick="btnConfirmStatusUpdate_Click" />

                </div>

            </div>

        </div>

        <asp:HiddenField ID="hdnSelectedApplicationId" runat="server" />

        <asp:HiddenField ID="hdnSelectedAction" runat="server" />

        <script type="text/javascript">

            function showApplicationModal() {

                document.getElementById('applicationModal').style.display = 'flex';

            }



            function hideApplicationModal() {

                document.getElementById('applicationModal').style.display = 'none';

            }



            function showStatusModal() {

                document.getElementById('statusModal').style.display = 'flex';

            }



            function hideStatusModal() {

                document.getElementById('statusModal').style.display = 'none';

            }



            function openFileInNewWindow(url) {

                window.open(url, '_blank', 'width=800,height=600,scrollbars=yes,resizable=yes');

            }



            // Close modals when clicking outside

            window.onclick = function (event) {

                var applicationModal = document.getElementById('applicationModal');

                var statusModal = document.getElementById('statusModal');



                if (event.target == applicationModal) {

                    hideApplicationModal();

                }

                if (event.target == statusModal) {

                    hideStatusModal();

                }

            }

        </script>

    </form>

</body>

</html>
