<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gwek.aspx.cs" Inherits="Gwek" %>
<%@ Register Src="~/Sidebar6.ascx" TagPrefix="uc" TagName="Sidebar" %>
<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Finance Dashboard</title>
    <link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" integrity="sha384-k6RqeWeci5ZR/Lv4MR0sA0FfDOMo4/6iFfbWq2IkNdh6DvU9jt1ZKZ3xv5iKk" crossorigin="anonymous" />
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
            display: flex;
            align-items: center;
            padding: 10px;
            border-radius: 4px;
            margin-bottom: 5px;
        }

        .sidebar a i {
            margin-right: 10px;
        }

        .sidebar a:hover, .sidebar a:active, .sidebar a:focus {
            background-color: #007bff;
            color: white;
        }

        .sidebar .active {
            background-color: #007bff;
            color: white;
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

        .container1, .container2 {
            background-color: white;
            padding: 20px;
            margin-bottom: 20px;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            position: relative;
        }

        .add-score {
            position: absolute;
            top: 20px;
            right: 20px;
            background-color: #007bff;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 4px;
            text-decoration: none;
            font-size: 14px;
            cursor: pointer;
        }

        .add-score:hover {
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
        }

        tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        /* Chart container styling */
        .chart-container {
            width: 100%;
            height: 400px; /* Adjust height as needed */
            margin: 20px 0;
            position: relative;
        }

        .chart-container canvas {
            width: 100% !important; /* Ensure the canvas fits its container */
            height: 100% !important; /* Ensure the canvas height fits its container */
        }

        @media (max-width: 768px) {
            .content {
                margin-left: 0;
                padding-top: 70px; /* Adjust padding for smaller screens */
            }

            .sidebar {
                width: 100%;
                height: auto;
                position: relative;
                top: 0;
                overflow-y: visible;
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div class="sidebar">
            <uc:Sidebar ID="Sidebar" runat="server" />
        </div>
          <div class="navbar">
            <button class="menu-toggle" onclick="toggleSidebar()">☰</button>
             <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1> <asp:Label ID="lblDepartmentName" runat="server" CssClass="department-name-label" />
 - SAK Scorecard</h1>
        </div>

        

        <div class="content">
            <div class="container1">
                <a href="academic.aspx" class="add-score"><i class="fas fa-plus"></i> Add Score</a>
                <h2>View Department Weekly Score</h2>

                <asp:DropDownList ID="DepartmentDropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DepartmentDropdown_SelectedIndexChanged">
                    <asp:ListItem Text="Finance" Value="finance"></asp:ListItem>
                    <asp:ListItem Text="Academic" Value="academic"></asp:ListItem>
                    <asp:ListItem Text="Theology" Value="theology"></asp:ListItem>
                    <asp:ListItem Text="Administration" Value="administration"></asp:ListItem>
                    <asp:ListItem Text="Quality Assurance" Value="quality assurance"></asp:ListItem>
                </asp:DropDownList>

                <asp:DropDownList ID="YearDropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="YearDropdown_SelectedIndexChanged">
                    <asp:ListItem Text="2024" Value="2024"></asp:ListItem>
                    <asp:ListItem Text="2025" Value="2025"></asp:ListItem>
                    <asp:ListItem Text="2026" Value="2026"></asp:ListItem>
                </asp:DropDownList>

                <asp:DropDownList ID="TermDropdown" runat="server" AutoPostBack="True" OnSelectedIndexChanged="TermDropdown_SelectedIndexChanged">
                    <asp:ListItem Text="Term 1" Value="1"></asp:ListItem>
                    <asp:ListItem Text="Term 2" Value="2"></asp:ListItem>
                    <asp:ListItem Text="Term 3" Value="3"></asp:ListItem>
                </asp:DropDownList>

                

                <p>Select Week:</p>
                <asp:DropDownList ID="DropDownList2" runat="server" AutoPostBack="True" OnSelectedIndexChanged="DropDownList2_SelectedIndexChanged"></asp:DropDownList>
                <asp:SqlDataSource ID="get_week" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [weeks]"></asp:SqlDataSource>

                <div class="container2"> 
              <h3>  <asp:Label ID="lblPivotedHeader" runat="server" Text=""></asp:Label> </h3>
                <asp:GridView ID="gvWeekData" runat="server" AutoGenerateColumns="True"></asp:GridView>
                    </div>

                <div class="container2">
                    <h3><asp:Label ID="lblVarianceHeader" runat="server" Text=""></asp:Label></h3>
                <asp:GridView ID="gvVarianceData" runat="server" AutoGenerateColumns="True"></asp:GridView>
                    </div>

                 <div class="container2">
                     <asp:Label ID="lblRankHeader" runat="server" Text=""></asp:Label>
                <asp:GridView ID="gvRankData" runat="server" AutoGenerateColumns="True"></asp:GridView>
                     </div>
                
                
                <asp:Label ID="lblVarianceHeader1" runat="server" Text=""></asp:Label>
                
                

                  <div class="container2">
                      <asp:Label ID="lblRankHeader1" runat="server" Text=""></asp:Label>

                              <asp:Chart ID="SchoolChart" runat="server" Width="600px" Height="400px">
                    <Series>
                        <asp:Series Name="Series1" ChartType="Column" />
                    </Series>
                    <ChartAreas>
                        <asp:ChartArea Name="ChartArea1" />
                    </ChartAreas>
                </asp:Chart>

                </div>


                <div class="content">
    <div class="container1">
        <!-- Existing code... -->

        <!-- Error message label -->
        <asp:Label ID="lblErrorMessage" runat="server" ForeColor="Red" Visible="False"></asp:Label>

        <!-- Existing code... -->
    </div>
</div>

            </div>
        </div>
    </form>
</body>
</html>
