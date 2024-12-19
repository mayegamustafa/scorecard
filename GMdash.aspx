<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GMdash.aspx.cs" Inherits="GMdash" %>
<%@ Register Src="~/Sidebar6.ascx" TagPrefix="uc" TagName="Sidebar" %>

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


        

    .legend-color {
        width: 20px;
        height: 20px;
        display: inline-block;
        margin-right: 10px;
    }

 

.charts-wrapper {
    display: flex;
    flex-wrap: wrap;
    gap: 20px;
    justify-content: space-between;
}

/*
.chart-item {
    flex-grow: 1;
    display: flex;
    flex-direction: column;
    justify-content: center;
    align-items: center;
    border: 1px solid #ccc;
    padding: 15px;
    border-radius: 8px;
    background-color: #f9f9f9;
} */

.chart-item {
    width: 80vw; /* Adjust based on your desired responsiveness */
    height: 50vw; /* Ensure the height is proportional to the width */
    max-width: 950px; /* Optional: Max width to prevent it from growing too large */
    max-height: 600px; /* Optional: Max height to prevent it from growing too large */
    min-width: 300px; /* Optional: Min width to prevent it from being too small */
    min-height: 200px; /* Optional: Min height to prevent it from being too small */
}


/* Smaller chart on the left */
.small-chart {
    flex: 1;
    max-width: 45%;
}

/* Wider chart on the right */
.wide-chart {
    flex: 2;
    max-width: 55%;
}

/* Responsive adjustments */
@media (max-width: 1200px) {
    .small-chart, .wide-chart {
        max-width: 100%;
        flex: 1 1 100%;
    }

    .chart-item {
        margin-bottom: 20px;
    }
}

/* Style for the chart container to ensure it adapts to different screen sizes */
.chart-container {
    width: 100%;
    max-width: 100%; /* Ensure the chart doesn’t overflow its container */
    padding: 10px;
    box-sizing: border-box;
    overflow: hidden; /* Prevent any overflow issues */
}

.custom-legend {
    margin: 20px;
    padding: 10px;
    border: 1px solid #ccc;
    border-radius: 5px;
    background-color: #f9f9f9;
}

.custom-legend h4 {
    margin-bottom: 10px;
    font-size: 1.2em;
}

.custom-legend ul {
    list-style: none;
    padding: 0;
    margin: 0;
    display: flex;
    flex-wrap: wrap;
}

.custom-legend li {
    margin-right: 15px;
    margin-bottom: 5px;
    display: flex;
    align-items: center;
}

.legend-color {
    display: inline-block;
    width: 20px;
    height: 20px;
    border-radius: 50%;
    margin-right: 8px;
}

/* Media query for smaller screens */
@media (max-width: 600px) {
    .custom-legend ul {
        flex-direction: column;
    }

    .custom-legend li {
        margin-right: 0;
        margin-bottom: 10px;
    }
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
        <h3>Areas in Finance</h3><i class="fas fa-wallet"></i>
        <asp:Label ID="lblTotalAreasf" runat="server" CssClass="dashboard-label" />
    </div>
    <div class="dashboard-widget">
        <h3>Academic Areas</h3><i class="fas fa-graduation-cap"></i>
        <asp:Label ID="lblTotalAreasA" runat="server" CssClass="dashboard-label" />
    </div>
    <div class="dashboard-widget">
        <h3>Theology Areas</h3><i class="fas fa-mosque"></i>
        <asp:Label ID="lblTotalAreasT" runat="server" CssClass="dashboard-label" />
    </div> 
    <div class="dashboard-widget">
        <h3>Areas in QA</h3><i class="fas fa-clipboard-check"></i>
        <asp:Label ID="lblTotalAreasQ" runat="server" CssClass="dashboard-label" />
    </div>
    <div class="dashboard-widget">
        <h3>Areas in Admin</h3><i class="fas fa-building"></i>
        <asp:Label ID="lblTotalAreasAd" runat="server" CssClass="dashboard-label" />
    </div> 
    <div class="dashboard-widget">
        <h3>Schools</h3><i class="fas fa-school"></i>
        <asp:Label ID="lblTotalschools" runat="server" CssClass="dashboard-label" />
    </div> 
    <div class="dashboard-widget">
        <h3>Users</h3><i class="fas fa-users"></i>
        <asp:Label ID="lblTotalUsers" runat="server" CssClass="dashboard-label" />
    </div> 
</div>


            <div class="form-group">
                <!-- Best Performing Schools -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek" runat="server"></asp:Label> - Overall Best Performing Schools </h3>
                       <asp:GridView ID="GridViewBestPerformersRecentWeek" runat="server" AutoGenerateColumns="false" >
                           <Columns>
                            <asp:BoundField DataField="school_name" HeaderText="School Name" /> 
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" /> 
                        </Columns>
                       </asp:GridView>
                     
                </div>

                <!-- Worst Performing Schools -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek2" runat="server"></asp:Label> - Overall Poor Performing Schools</h3>
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
                    <h3><asp:Label ID="lblRecentWeek3" runat="server"></asp:Label> - Overall Best Performing Areas</h3>
                    <asp:GridView ID="GridViewBestAreasRecentWeek" runat="server" AutoGenerateColumns="false">
                        <Columns>
                            <asp:BoundField DataField="area_name" HeaderText="School Name" />
                            <asp:BoundField DataField="total_score" HeaderText="Total Score" />
                        </Columns>
                    </asp:GridView>

                </div>

                <!-- Worst Performing Areas -->
                <div class="form-container2">
                    <h3><asp:Label ID="lblRecentWeek4" runat="server"></asp:Label> - Overall Poorly Performed Areas</h3>
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
                              <!-- Improvement and Decline Data -->
<div class="form-container2">
    <h3> <asp:Label ID="lblRecentWeek5" runat="server"></asp:Label> - ASSESSMENT </h3>
    <asp:GridView ID="GridViewImprovementDecline" runat="server" AutoGenerateColumns="False" CssClass="gridview">
       
    <Columns>
        <asp:BoundField DataField="department_name" HeaderText="Department Name" />
        <asp:BoundField DataField="Most_Improved_School" HeaderText="Most Improved School" />
        <asp:BoundField DataField="Improvement_Percentage" HeaderText="Improvement Percentage" />
        <asp:BoundField DataField="Most_Declined_School" HeaderText="Most Declined School" />
        <asp:BoundField DataField="Decline_Percentage" HeaderText="Decline Percentage" />
        <asp:BoundField DataField="Overall_Most_Improved_School" HeaderText="Overall Most Improved School" />
        <asp:BoundField DataField="Overall_Most_Improved_Percentage" HeaderText="Overall Most Improved Percentage" />
        <asp:BoundField DataField="Overall_Most_Declined_School" HeaderText="Overall Most Declined School" />
        <asp:BoundField DataField="Overall_Most_Declined_Percentage" HeaderText="Overall Most Declined Percentage" />
    </Columns>
</asp:GridView>

    
</div>


            <div class="form-container2">
                   <div class="custom-legend">
    <h4>Key:</h4>
    <ul>
        <li><span class="legend-color" style="background-color: blue;"></span> Finance</li>
        <li><span class="legend-color" style="background-color: orange;"></span> Quality Assurance</li>
        <li><span class="legend-color" style="background-color: red;"></span> Academic</li>
        <li><span class="legend-color" style="background-color: purple;"></span> Theology</li>
    </ul>
</div>
    <div class="charts-wrapper">
        <!-- First Chart: Slightly smaller on the left side -->
        <div class="chart-item small-chart">
            <h3><asp:Label ID="lblRecentWeek7" runat="server"></asp:Label> - Dpt Average Chart</h3>
            <asp:Chart ID="SchoolScoresChart1" runat="server" Width="400px" Height="400px">
                <Series>
                    <asp:Series Name="Series1" ChartType="Column" XValueMember="school_name" YValueMembers="Average_Score"></asp:Series>
                </Series>
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                </ChartAreas>
            </asp:Chart>
        </div>

        <!-- Second Chart: Wider and flexible on the right side -->
        <div class="chart-item wide-chart">
            <asp:Chart ID="SchoolTrendChart" runat="server" Width="1050px" Height="400px">
                <ChartAreas>
                    <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
                </ChartAreas>
                <Series>
                    <asp:Series Name="AverageScore" ChartType="Line" ChartArea="ChartArea1"></asp:Series>
                </Series>
            </asp:Chart>

    

        </div>
    </div>
</div>
                 

 <!-- Responsive chart container -->
<div class="chart-container">
   
</div>
               <div class="form-container2">
    <div class="charts-wrapper">

          <h3><asp:Label ID="lblRecentWeek6" runat="server"></asp:Label> - Average Chart</h3>
    <asp:Chart ID="SchoolScoresChart" runat="server" Width="950px" Height="400px">
        <ChartAreas>
            <asp:ChartArea Name="ChartArea1"></asp:ChartArea>
        </ChartAreas>
        <Series>
            <asp:Series Name="Series1" ChartType="Column" XValueMember="school_name" YValueMembers="Average_Score">
            </asp:Series>
        </Series>
    </asp:Chart>
            
            </div>
                   </div>

            

            <!-- Left Column: Schools -->

                              <asp:Label ID="lblErrorMessage" runat="server" Text="Label" Visible="false"></asp:Label>

            <div class="form-group">
                <!-- Best Performing Schools -->
                <div class="form-container2">
                      <h3>Term <asp:Label ID="lblRecentWeek11" runat="server"></asp:Label> <asp:Label ID="lblRecentWeek111" runat="server"></asp:Label> - Overrall Best Performing Schools</h3>
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

    <script type="text/javascript">
        function resizeCharts() {
            var smallChart = document.getElementById('<%= SchoolScoresChart1.ClientID %>');
        var wideChart = document.getElementById('<%= SchoolTrendChart.ClientID %>');

            // Set dynamic width and height based on the container size
            if (smallChart) {
                smallChart.style.width = (smallChart.parentElement.offsetWidth * 0.95) + 'px';
                smallChart.style.height = (smallChart.parentElement.offsetHeight * 0.95) + 'px';
            }
            if (wideChart) {
                wideChart.style.width = (wideChart.parentElement.offsetWidth * 0.95) + 'px';
                wideChart.style.height = (wideChart.parentElement.offsetHeight * 0.95) + 'px';
            }
        }

        // Call resizeCharts on page load and on window resize
        window.onload = resizeCharts;
        window.onresize = resizeCharts;
</script>

    <script type="text/javascript">
        // Function to adjust the chart width
        function adjustChartWidth() {
            var chart = document.getElementById('<%= SchoolScoresChart.ClientID %>');
            if (chart) {
                chart.style.width = (window.innerWidth - 20) + 'px'; // Adjust width as needed
            }
        }

        // Adjust width on window resize
        window.addEventListener('resize', adjustChartWidth);

        // Initialize width on page load
        window.addEventListener('load', adjustChartWidth);
</script>

</body>
</html>
