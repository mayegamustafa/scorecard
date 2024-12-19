<%@ Control Language="C#" AutoEventWireup="true" CodeFile="Sidebar4.ascx.cs" Inherits="Sidebar" %>
<%@ Import Namespace="System.IO" %>

<link rel="stylesheet" href="https://cdnjs.cloudflare.com/ajax/libs/font-awesome/6.0.0-beta3/css/all.min.css" integrity="sha384-k6RqeWeci5ZR/Lv4MR0sA0FfDOMo4/6iFfbWq2IkNdh6DvU9jt1ZKZ3xv5iKk" crossorigin="anonymous" />

<link rel="stylesheet" href="fontawesome-free-6.6.0-web/css/all.min.css" />

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
        /*color: lightblue; */ /* Default icon color */
        
    
        color: darkblue; /* Set the icon color to dark blue */
   
    }

    .sidebar a:hover, .sidebar a:active, .sidebar a:focus {
        background-color: #007bff;
        color: white;
    }

    .sidebar .active {
        background-color: #007bff;
        color: white;
    }

    .sidebar .active i {
        color: white; /* Active icon color */
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

    .logout-btn {
        margin-top: auto; /* Pushes the button to the bottom */
        background-color: #dc3545; /* Red background for logout */
        color: white;
        border: none;
        border-radius: 4px;
        padding: 10px;
        cursor: pointer;
        text-align: center;
        font-size: 16px;
    }

    .logout-btn:hover {
        background-color: #c82333; /* Darker red on hover */
    }

    .sidebar a i {
        color: blue; /* Set the icon color to light blue */
    }

    .sidebar a:hover i, .sidebar a:active i, .sidebar a:focus i {
        color: white; /* Ensure color changes on hover, active, or focus */
    }

    .sidebar .active a i {
        color: white; /* Active icon color */
    }

</style>

<% 
    string currentPage = Path.GetFileName(Request.Path);
%>

<div class="sidebar-menu">
    <ul>
        <li><a href="Qdash.aspx" class="<%= currentPage == "Qdash.aspx" ? "active" : "" %>"><i class="fas fa-tachometer-alt"></i>Dashboard</a></li>
        <li><a href="QA.aspx" class="<%= currentPage == "QA.aspx" ? "active" : "" %>"><i class="fas fa-chart-bar"></i>Score</a></li>
        <li><a href="fscores.aspx" class="<%= currentPage == "fscores.aspx" ? "active" : "" %>"><i class="fas fa-dollar-sign"></i>#Scores</a></li>
        <li><a href="Qanaly.aspx" class="<%= currentPage == "Qanaly.aspx" ? "active" : "" %>"><i class="fas fa-chart-line"></i>Moving Average</a></li>
        <li><a href="Qwek.aspx" class="<%= currentPage == "Qwek.aspx" ? "active" : "" %>"><i class="fas fa-calendar-week"></i>Weekly Score</a></li>
          <li><a href="QSchoolTrend.aspx" class="<%= currentPage == "QSchoolTrend.aspx" ? "active" : "" %>"><i class="fas fa-pie-chart"></i>Schools Trend</a></li>
        <li><a href="Qmanage.aspx" class="<%= currentPage == "Qmanage.aspx" ? "active" : "" %>"><i class="fas fa-cog"></i>Manage Areas</a></li>
        <!-- Add more sidebar links here -->
    </ul>
    <!-- Logout button -->
   <a href="Login.aspx" class="logout-btn"><i class="fas fa-sign-out-alt"></i>Logout</a>
</div>

<script>
    function toggleSidebar() {
        var sidebar = document.querySelector('.sidebar');
        if (sidebar.style.display === 'block') {
            sidebar.style.display = 'none';
        } else {
            sidebar.style.display = 'block';
        }
    }
</script>
