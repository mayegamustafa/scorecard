<%@ Page Language="C#" AutoEventWireup="true" CodeFile="administration.aspx.cs" Inherits="finance" %>
<%@ Register Src="~/Sidebar5.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finance Page</title>
    <style>
        /* Reset and basic styling */
        body, h1, h2, h3, p, div, table, form {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }

        body {
            font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
            background-color: #f4f4f4;
            line-height: 1.6;
        }

    
         /* Navbar styling */
        .navbar {
            background-color: white;
            color: black;
            padding: 15px;
            text-align: center;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            z-index: 1000;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .navbar h1 {
            margin: 0;
            font-size: 24px;
            font-weight: bold;
            display: inline;
        }

        .navbar img {
            height: 50px;
            vertical-align: middle;
            margin-right: 15px;
        }

        .menu-toggle {
            display: none;
            background: none;
            border: none;
            color: white;
            font-size: 24px;
            cursor: pointer;
            position: absolute;
            top: 15px;
            left: 15px;
        }

        /* Sidebar styling */
        .sidebar {
            background-color: white;
            color: black;
            width: 250px;
            height: 100vh;
            position: fixed;
            top: 60px; /* Adjusted for the navbar */
            left: 0;
            overflow-y: auto;
            padding: 20px;
            transition: width 0.3s;
            z-index: 1000;
            list-style: none; /* Remove bullets */
            font-weight: bold;

            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .sidebar a {
            color: black;
            text-decoration: none;
            display: block;
            padding: 10px;
            border-radius: 4px;
            margin-bottom: 5px;
        }

        .sidebar a:hover, .sidebar a:active, .sidebar a:focus {
            background-color: #007bff;
            color: white;
        }

        .sidebar-menu ul {
            list-style-type: none; /* Remove bullets */
            padding: 0;
        }

        .sidebar-menu li {
            margin-bottom: 10px;
        }

        /* Main content styling */
        .content {
            margin-left: 250px;
            padding: 80px 20px 20px;
            background-color: #f9f9f9;
            min-height: 100vh;
        }

        .content h2 {
            margin-bottom: 20px;
            font-weight: bold;
        }

        .content label, .content select, .content input {
            display: block;
            margin-bottom: 15px;
            font-size: 14px;
        }

        .content select, .content input {
            width: 100%;
            padding: 10px;
            border: 1px solid #ddd;
            border-radius: 4px;
        }

        .content button {
            background-color: #007bff;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
        }

        .content button:hover {
            background-color: #0056b3;
        }

        .gridview-container {
            margin-top: 20px;
            overflow-x: auto;
        }

        table {
            width: 100%;
            border-collapse: collapse;
        }

        th, td {
            border: 1px solid #ddd;
            padding: 8px;
            text-align: left;
        }

        th {
            background-color: #007bff;
            color: white;
            text-decoration: none;
        }

        tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        /* Responsive styling */
        @media (max-width: 768px) {
            .sidebar {
                width: 100%;
                height: auto;
                position: relative;
                top: 0;
                padding: 10px;
                display: none; /* Hide sidebar on small screens */
            }

            .content {
                margin-left: 0;
                padding: 20px;
            }

            .sidebar-menu {
                display: block;
                background-color: white;
                color: black;
                padding: 10px;
                border: none;
                cursor: pointer;
            }
        }

        .form-container {
            width: 500px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .form-container h2 {
            text-align: center;
            margin-bottom: 20px;
        }

        .form-group {
            margin-bottom: 15px;
        }

        .form-group label {
            display: block;
            margin-bottom: 5px;
            font-weight: bold;
        }

        .form-control {
            width: 100%;
            padding: 8px;
            box-sizing: border-box;
            border: 1px solid #ccc;
            border-radius: 4px;
            font-size: 14px;
        }

        .btn {
            display: block;
            width: 50px;
            padding: 10px;
            background-color: #007bff;
            color: #fff;
            border: none;
            border-radius: 4px;
            font-size: 16px;
            cursor: pointer;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        /* Notification styling */
        #notification {
            display: none;
            width: 300px;
            margin: 20px auto;
            padding: 15px;
            background-color: #4CAF50;
            color: white;
            text-align: center;
            border-radius: 4px;
            position: fixed;
            top: 20px;
            left: 50%;
            transform: translateX(-50%);
            z-index: 1001;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }
    </style>

    <script type="text/javascript">
        function showNotification(message) {
            var notification = document.getElementById("notification");
            notification.innerHTML = message;
            notification.style.display = "block";
            setTimeout(function () {
                notification.style.display = "none";
            }, 3000); // 3 seconds
        }

        function clearTextBox() {
            document.getElementById('<%= TextBox1.ClientID %>').value = "";
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
       
        <div class="sidebar">
            <uc:Sidebar ID="Sidebar" runat="server" />
        </div>

        <div class="navbar">
           <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1>SAK Scorecard</h1>
        </div>

        <div class="content">
            
            <asp:Label ID="lblUsername" runat="server" Text="Username"></asp:Label>
            <asp:Label ID="lblDepartment" runat="server" Text="Department"></asp:Label>

            <div class="form">
                <div class="form-container">
                    <h2>SCORE CARD</h2>

                    <div class="form-group">
                        <label for="DropDownList1">School</label>
                        <asp:DropDownList ID="DropDownList1" runat="server" DataSourceID="score" DataTextField="school_name" DataValueField="school_id" CssClass="form-control"></asp:DropDownList>
                        <asp:SqlDataSource ID="score" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [schools]"></asp:SqlDataSource>
                    </div>

                    <div class="form-group">
                        <label for="DropDownList2">Area</label>
                        <asp:DropDownList ID="DropDownList2" runat="server" DataSourceID="area" DataTextField="area_name" DataValueField="area_id" CssClass="form-control"></asp:DropDownList>
                        <asp:SqlDataSource ID="area" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" 
                            SelectCommand="SELECT area_id, area_name FROM areas WHERE department_id = @DepartmentID">
                            <SelectParameters>
                                <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" Type="Int32" />
                            </SelectParameters>
                        </asp:SqlDataSource>
                    </div>

                    <div class="form-group">
                        <label for="DropDownList3">Week</label>
                        <asp:DropDownList ID="DropDownList3" runat="server" DataSourceID="week" DataTextField="week_name" DataValueField="week_id" CssClass="form-control"></asp:DropDownList>
                        <asp:SqlDataSource ID="week" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT week_id, week_name FROM weeks"></asp:SqlDataSource>
                    </div>

                    <div class="form-group">
                        <label for="TextBox1">Score</label>
                        <asp:TextBox ID="TextBox1" runat="server" CssClass="form-control" CausesValidation="True" ValidateRequestMode="Enabled"></asp:TextBox>
                    </div>

                    <div class="form-group">
                        <asp:Button ID="Button1" runat="server" Text="Save" OnClick="Button1_Click" CssClass="btn" />
                    </div>

                    <asp:Label ID="lblMessage" runat="server" Text="" CssClass="notification"></asp:Label>

                   
                    <div id="notification"></div>
                </div>
            </div>

            <div class="gridview-container">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" 
    DataKeyNames="score_id" AllowSorting="True" 
    OnSorting="GridView1_Sorting" OnRowEditing="GridView1_RowEditing"
    OnRowUpdating="GridView1_RowUpdating" OnRowCancelingEdit="GridView1_RowCancelingEdit"
    OnRowDeleting="GridView1_RowDeleting">
    
    <Columns>
        <asp:BoundField DataField="school_name" HeaderText="School" SortExpression="school_name" />
        <asp:BoundField DataField="area_name" HeaderText="Area" SortExpression="area_name" />
        <asp:BoundField DataField="week_name" HeaderText="Week" SortExpression="week_name" />
        <asp:BoundField DataField="score" HeaderText="Score" SortExpression="score" />
        
       
        <asp:CommandField ShowEditButton="True" ShowDeleteButton="True" />
    </Columns>
</asp:GridView>

            </div>
        </div>
    </form>
</body>
</html>
