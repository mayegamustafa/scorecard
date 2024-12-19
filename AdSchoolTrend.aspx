<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AdSchoolTrend.aspx.cs" Inherits="fSchoolTrend" %>
<%@ Register Src="~/Sidebar5.ascx" TagPrefix="uc" TagName="Sidebar" %>

<link rel="stylesheet" href="fontawesome-free-6.6.0-web/css/all.min.css" />

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>School Trend Analysis</title>
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
            <h2>School Performance Trend</h2>

            <label for="YearDropdown">Select Year:</label>
            <asp:DropDownList ID="YearDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>

            <label for="TermDropdown">Select Term:</label>
            <asp:DropDownList ID="TermDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>

            <label for="SchoolDropdown">Select School:</label>
            <asp:DropDownList ID="SchoolDropdown" runat="server" AutoPostBack="true" OnSelectedIndexChanged="Dropdowns_SelectedIndexChanged">
            </asp:DropDownList>

            <hr />
                <div class="container2">
            <asp:GridView ID="GridViewProgressiveScores1" runat="server" AutoGenerateColumns="False" GridLines="Both" Width="100%" />
                    </div>
            <hr /> <asp:Label ID="Label1" runat="server" ForeColor="Red"></asp:Label>

            <asp:Chart ID="ScoresChart" runat="server" Width="1000px" Height="400px">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1">
                    </asp:ChartArea>
                </ChartAreas>
                <Legends>
                    <asp:Legend Name="Legend1" />
                </Legends>
                <Series>
                    <asp:Series Name="Series1" ChartType="Line" />
                </Series>
            </asp:Chart>
            <br />
           <asp:Label ID="lblMessage" runat="server" ForeColor="Red"></asp:Label>

                </div>
        </div>
    </form>

</body>
</html>
