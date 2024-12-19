<%@ Page Language="C#" AutoEventWireup="true" CodeFile="fscores.aspx.cs" Inherits="fscores" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Scores Page</title>
    <style>
        /* Basic reset and responsive styling */
        body, h1, h2, h3, p, div, table {
            margin: 0;
            padding: 0;
            box-sizing: border-box;
        }
        
        body {
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            line-height: 1.6;
        }

        /* Navbar styles */
        .navbar {
            background-color: #007bff; /* Blue */
            color: white;
            padding: 15px;
            text-align: center;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            z-index: 1000;
        }

        .navbar h1 {
            margin: 0;
            font-size: 24px;
        }

        /* Sidebar styles */
        .sidebar {
            background-color: #007bff; /* Blue to match the navbar */
            color: white;
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
        }

        .sidebar a {
            color: white;
            text-decoration: none;
            display: block;
            padding: 10px;
            border-radius: 4px;
            margin-bottom: 5px;
        }

        .sidebar a:hover {
            background-color: #0056b3; /* Darker blue on hover */
        }

        .sidebar-menu {
            display: none;
        }

        .sidebar-menu.active {
            display: block;
            background-color: #007bff; /* Blue to match the sidebar */
            position: absolute;
            top: 60px;
            left: 0;
            width: 100%;
            padding: 10px;
            list-style: none; /* Remove bullets */
        }

        .sidebar-menu a {
            padding: 10px;
        }

        /* Main content styling */
        .main-content {
            margin-left: 250px; /* Sidebar width */
            padding: 80px 20px 20px; /* Adjusted for the navbar */
        }

        h2 {
            font-size: 24px;
            color: #333;
            margin-bottom: 10px;
        }

        .gridview-container {
            margin-bottom: 30px;
        }

        .gridview-container table {
            width: 100%;
            border-collapse: collapse;
            margin-bottom: 20px;
        }

        .gridview-container th, .gridview-container td {
            padding: 8px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }

        .gridview-container tr:nth-child(even) {
            background-color: #f9f9f9;
        }

        .gridview-container th {
            background-color: #007bff;
            color: white;
            text-align: left;
        }

        /* Dropdown styling */
        .week-selection {
            margin-bottom: 30px;
        }

        .week-selection label {
            display: block;
            margin-bottom: 8px;
            font-weight: bold;
        }

        .week-selection select, .week-selection button {
            padding: 8px;
            font-size: 16px;
            margin-right: 10px;
        }

        /* Responsive adjustments */
        @media (max-width: 768px) {
            .main-content {
                margin-left: 0;
                padding: 60px 10px;
            }

            .sidebar {
                width: 100%;
                height: auto;
                position: relative;
            }

            .sidebar a {
                display: inline-block;
                padding: 5px 10px;
                margin-right: 10px;
            }
        }

        @media (max-width: 600px) {
            .week-selection {
                display: flex;
                flex-direction: column;
            }

            .week-selection select, .week-selection button {
                margin-bottom: 10px;
                width: 100%;
            }
        }

        /* Active link styling */
        .sidebar a.active {
            background-color: #0056b3;
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
    <div class="navbar">
        <h1>Performance Analysis</h1>
    </div>

    <div class="sidebar">
        <ul>
            <li><a href="fscores.aspx" class="active">Scores</a></li>
            <!-- Add more links as necessary -->
        </ul>
    </div>

    <div class="main-content">
        <div class="gridview-container">
            <h2>All Weeks Comparison</h2>
            <asp:Label ID="lblErrorMessage" runat="server" Text="Label"></asp:Label>
            <asp:GridView ID="GridViewAllWeeksComparison" runat="server" AutoGenerateColumns="False" CssClass="table table-bordered" BackColor="White" BorderColor="#CC9966" BorderStyle="None" BorderWidth="1px" CellPadding="4" DataSourceID="comp" EnableModelValidation="True" AllowPaging="True">
                <Columns>
                    <asp:BoundField DataField="Week1" HeaderText="Week1" SortExpression="Week1" />
                    <asp:BoundField DataField="Week2" HeaderText="Week2" SortExpression="Week2" />
                    <asp:BoundField DataField="area_name" HeaderText="area_name" SortExpression="area_name" />
                    <asp:BoundField DataField="department_name" HeaderText="department_name" SortExpression="department_name" />
                    <asp:BoundField DataField="school_name" HeaderText="school_name" SortExpression="school_name" />
                    <asp:BoundField DataField="Week1_AvgScore" HeaderText="Week1_AvgScore" ReadOnly="True" SortExpression="Week1_AvgScore" />
                    <asp:BoundField DataField="Week2_AvgScore" HeaderText="Week2_AvgScore" ReadOnly="True" SortExpression="Week2_AvgScore" />
                    <asp:BoundField DataField="Score_Change" HeaderText="Score_Change" ReadOnly="True" SortExpression="Score_Change" />
                </Columns>
                <FooterStyle BackColor="#FFFFCC" ForeColor="#330099" />
                <HeaderStyle BackColor="#990000" Font-Bold="True" ForeColor="#FFFFCC" />
                <PagerStyle BackColor="#FFFFCC" ForeColor="#330099" HorizontalAlign="Center" />
                <RowStyle BackColor="White" ForeColor="#330099" />
                <SelectedRowStyle BackColor="#FFCC66" Font-Bold="True" ForeColor="#663399" />
            </asp:GridView>
            <asp:SqlDataSource ID="comp" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [week_comparison]"></asp:SqlDataSource>
        </div>

        <div class="week-selection">
            <h2>Select Weeks to Compare</h2>
            <label for="ddlWeek1">Week 1:</label>
            <asp:DropDownList ID="ddlWeek1" runat="server" DataSourceID="wk1" DataTextField="week_name" DataValueField="week_name"></asp:DropDownList>

            <asp:SqlDataSource ID="wk1" runat="server" ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>" SelectCommand="SELECT * FROM [weeks]"></asp:SqlDataSource>

            <label for="ddlWeek2">Week 2:</label>
            <asp:DropDownList ID="ddlWeek2" runat="server" DataSourceID="wk1" DataTextField="week_name" DataValueField="week_id"></asp:DropDownList>

            <asp:Button ID="btnCompareWeeks" runat="server" Text="Compare" OnClick="btnCompareWeeks_Click" />
        </div>

        <div class="gridview-container">
            <h2>Week Comparison Results</h2>
            <asp:GridView ID="GridViewWeekComparison" runat="server" AutoGenerateColumns="true" CssClass="table table-bordered"></asp:GridView>
        </div>
    </div>
    </form>
</body>
</html>
