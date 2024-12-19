<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Qdash.aspx.cs" Inherits="Afdash" %>
<%@ Register Src="~/Sidebar4.ascx" TagPrefix="uc" TagName="Sidebar" %>

<link rel="stylesheet" href="fontawesome-free-6.6.0-web/css/all.min.css" />

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>SAK Scorecard</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            background-color: #f4f4f4;
            overflow-x: hidden; /* Prevent horizontal scrolling */
        }

        .sidebar {
            background-color: #333;
            color: #fff;
            padding: 15px;
            width: 250px;
            height: 100vh;
            float: left;
            position: fixed;
            top: 0;
            left: 0;
            transition: transform 0.3s ease;
        }

        .sidebar.hidden {
            transform: translateX(-100%);
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

        .container {
            margin: 70px 0 0 250px; /* Adjust for sidebar and navbar */
            padding: 20px;
            display: flex;
            justify-content: space-between;
            flex-wrap: wrap; /* Wrap content on small screens */
        }

        .form-group {
            width: 45%; /* Adjust width for desktop view */
            display: flex;
            flex-direction: column;
        }

        .form-container2 {
            width: 100%;
            max-width: 100%;
            margin: 0 auto;
            padding: 10px;
            background-color: #fff;
            border-radius: 8px;
            box-shadow: 0 0 10px rgba(0, 0, 0, 0.1);
            margin-bottom: 20px; /* Spacing between containers */
        }

        .form-container2 h3 {
            text-align: center;
            color: #333;
        }

        .gridview {
            width: 100%;
            border-collapse: collapse;
            margin-top: 10px;
        }

        .gridview th, .gridview td {
            border: 1px solid #ddd;
            padding: 8px;
        }

        .gridview tr:nth-child(even) {
            background-color: #f2f2f2;
        }

        .gridview tr:hover {
            background-color: #ddd;
        }

        .gridview th {
            background-color: #4CAF50;
            color: white;
            text-align: center;
        }

        .gridview.worst th {
            background-color: #f44336; /* Red header for worst performers */
            color: white;
            text-align: center;
        }

         /* Responsive styles */
        @media (max-width: 768px) {
            .sidebar {
                width: 200px;
            }

            .container {
                margin-left: 0; /* Full width for mobile */
            }

            .menu-toggle {
                display: block;
                color: black;
            }

            .form-group {
                width: 100%;
            }

             .dashboard-widget {
                flex: 1 1 100%; /* Full width for smaller screens */
                margin: 10px 0;
            }
             .dashboard-widget i {
                font-size: 36px; /* Adjust icon size for smaller screens */
            }
        }

      /* Dashboard Widget Container */
.dashboard-widget {
    background-color: #ffffff;
    border: 1px solid #ddd;
    border-radius: 8px;
    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
    padding: 20px;
    margin: 10px 10px 20px 10px; /* Ensure margin-bottom to separate from other elements */
    text-align: center; /* Center-align the text */
    width: 200px; /* Ensure it takes full width available */
    box-sizing: border-box; /* Include padding and border in element's total width and height */
    float: left;
}

/* Dashboard Label */
.dashboard-label {
    font-size: 24px; /* Increased font size for prominence */
    font-weight: bold;
    color: #f44336;
    display: block; /* Ensure the label is displayed as a block element */
}
 /* Increase Icon Sizes */
        .dashboard-widget i {
            font-size: 48px; /* Increase icon size */
            color: #f44336;
            margin-bottom: 10px;
        }


    </style>
</head>
<body>
    <form id="form1" runat="server">
       

        <div class="sidebar" id="sidebar">
            <uc:Sidebar ID="Sidebar1" runat="server" />
        </div>

           <div class="navbar">
            <button class="menu-toggle" onclick="toggleSidebar()">☰</button>
             <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1> <asp:Label ID="lblDepartmentName" runat="server" CssClass="department-name-label" />
 - SAK Scorecard</h1>
        </div>



        <!---Recent week  --->

         <div class="container">
            <!-- Left Column: Schools -->
                  <div class="form-container2">
             <div class="dashboard-widget">
                 <h3>Number of Areas  </h3> <i class="fas fa-clipboard-list"></i>
    <asp:Label ID="lblTotalAreas" runat="server" CssClass="dashboard-label" />
</div>
                    <div class="dashboard-widget">
                 <h3> Schools  </h3><i class="fas fa-school"></i>
    <asp:Label ID="lblTotalschools" runat="server" CssClass="dashboard-label" />
</div> 
                         <div class="dashboard-widget">
                 <h3> Users  </h3><i class="fas fa-users"></i>
    <asp:Label ID="lblTotalUser" runat="server" CssClass="dashboard-label" />
</div> 
                      </div>

            <div class="form-group">
                <!-- Best Performing Schools -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek" runat="server"></asp:Label> - Best Performing Schools  </h3>
                       <asp:GridView ID="GridViewBestPerformersRecentWeek" runat="server" AutoGenerateColumns="false" >
                           <Columns>
                            <asp:BoundField DataField="school_name" HeaderText="School Name" /> 
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" /> 
                        </Columns>
                       </asp:GridView>
                     
                </div>

                <!-- Worst Performing Schools -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek2" runat="server"></asp:Label> - Worst Performing Schools</h3>
                     <asp:GridView ID="GridViewWorstPerformersRecentWeek" runat="server" AutoGenerateColumns="false" CssClass="gridview worst">
                         <Columns>
                            <asp:BoundField DataField="school_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                     </asp:GridView>
                        
                </div>
            </div>

            <!-- Right Column: Areas -->
            <div class="form-group">
                <!-- Best Performing Areas -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek3" runat="server"></asp:Label> - Best Performing Areas</h3>
                    <asp:GridView ID="GridViewBestAreasRecentWeek" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="area_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>

                </div>

                <!-- Worst Performing Areas -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek4" runat="server"></asp:Label> - Worst Performing Areas</h3>
                    <asp:GridView ID="GridViewWorstAreasRecentWeek" runat="server" AutoGenerateColumns="false" CssClass="gridview worst">
                        <Columns>
                            <asp:BoundField DataField="area_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>

                    </asp:GridView>
                </div>
            </div>
        </div>

        <div class="container">
            <!-- Left Column: Schools -->
            <div class="form-group">
                <!-- Best Performing Schools -->
                <div class="form-container2">
                      <h3>Term <asp:Label ID="lblRecentWeek11" runat="server"></asp:Label> <asp:Label ID="lblRecentWeek111" runat="server"></asp:Label> - Best Performing Schools</h3>
                    <asp:GridView ID="GridViewBestPerformers" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                        <Columns>
                            <asp:BoundField DataField="school_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>
                </div>
            
       

                <!-- Worst Performing Schools -->
                <div class="form-container2">
                    <h3>Term <asp:Label ID="lblRecentWeek12" runat="server"></asp:Label> <asp:Label ID="lblRecentWeek112" runat="server"></asp:Label> - Overrall Worst Performing Schools</h3>
                    <asp:GridView ID="GridViewWorstPerformers" runat="server" AutoGenerateColumns="False" CssClass="gridview worst">
                        <Columns>
                            <asp:BoundField DataField="school_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

                <!-- Right Column: Areas -->
          <div class="form-group">
                <!-- Best Performing Areas -->
                <div class="form-container2">
                    <h3>Term <asp:Label ID="lblRecentWeek13" runat="server"></asp:Label> <asp:Label ID="lblRecentWeek113" runat="server"></asp:Label> - Overrall Best Performing Areas</h3>
                    <asp:GridView ID="GridViewBestAreas" runat="server" AutoGenerateColumns="False" CssClass="gridview">
                        <Columns>
                            <asp:BoundField DataField="area_name" HeaderText="Area Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>
                </div>
          
                <!-- Worst Performing Areas -->
                <div class="form-container2">
                    <h3>Term <asp:Label ID="lblRecentWeek14" runat="server"></asp:Label> <asp:Label ID="lblRecentWeek114" runat="server"></asp:Label> - Overrall Worst Performing Areas</h3>
                    <asp:GridView ID="GridViewWorstAreas" runat="server" AutoGenerateColumns="False" CssClass="gridview worst">
                        <Columns>
                            <asp:BoundField DataField="area_name" HeaderText="Area Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>
                </div> 
            </div>
        </div>
    </form>

    <script>
        function toggleSidebar() {
            var sidebar = document.getElementById('sidebar');
            if (sidebar.classList.contains('hidden')) {
                sidebar.classList.remove('hidden');
            } else {
                sidebar.classList.add('hidden');
            }
        }
    </script>
</body>
</html>
