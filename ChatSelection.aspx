<%@ Page Language="C#" AutoEventWireup="true" CodeFile="ChatSelection.aspx.cs" Inherits="ChatSelection" %>
<%@ Register Src="~/Sidebar2.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Chat</title>
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
            background-color: #ffffff;
            color: #333;
            padding: 15px;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            z-index: 1000;
            border-bottom: 1px solid #ddd;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            display: flex;
            align-items: center;
            justify-content: space-between;
        }

        .navbar h1 {
            margin: 0;
            font-size: 24px;
            font-weight: bold;
        }

        .navbar img {
            height: 50px;
            margin-right: 15px;
        }

        /* Sidebar styling */
        .sidebar {
            width: 250px; /* Fixed width for sidebar */
            position: fixed; /* Fix sidebar to the left */
            top: 60px; /* Space for the navbar */
            left: 0;
            bottom: 0;
            background-color: #ffffff;
            padding: 20px;
            border-right: 1px solid #ddd;
            box-shadow: 2px 0 4px rgba(0, 0, 0, 0.1);
            overflow-y: auto; /* Enable scrolling if content overflows */
        }

        .content {
        display: flex; /* Use flexbox for centering */
        justify-content: center; /* Center horizontally */
        align-items: center; /* Center vertically */
        min-height: calc(100vh - 60px); /* Full height minus navbar height */
        padding: 20px;
        background-color: #f9f9f9;
    }

    .form-container {
        width: 100%;
        max-width: 600px; /* Limit form width */
        padding: 20px;
        background-color: #fff;
        border: 1px solid #ddd;
        border-radius: 4px;
        box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
    }


        .gridview {
            width: 100%;
            border-collapse: collapse; /* Remove gaps between cells */
            margin-top: 20px;
        }

        .gridview th, .gridview td {
            border: 1px solid #ccc;
            padding: 10px;
            text-align: left;
        }

        .gridview th {
            background-color: #007bff; /* Header color */
            color: white;
        }

        .gridview tr:nth-child(even) {
            background-color: #f2f2f2; /* Alternate row color */
        }

        .gridview tr:hover {
            background-color: #e9ecef; /* Highlight row on hover */
        }

        .select-button {
            background-color: #007bff; /* Button color */
            color: white;
            border: none;
            padding: 8px 12px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
        }

        .select-button:hover {
            background-color: #0056b3; /* Darker shade on hover */
        }

        /* Responsive styling */
        @media (max-width: 768px) {
            .sidebar {
                width: 100%;
                position: relative; /* Make sidebar relative */
                padding: 10px;
                display: none; /* Hide sidebar on small screens */
            }

            .content {
                margin-left: 0; /* No left margin */
                padding: 20px;
            }

            #form1 {
                width: 90%; /* Full width on smaller screens */
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
            <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1>SAK Scorecard</h1>
        </div>

       <div class="content">
    <div class="form-container">
        <h2>Select a Department or User to Chat</h2>
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" OnRowCommand="gvUsers_RowCommand" CssClass="gridview">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>' Visible="false"></asp:Label>
                        <%# Eval("UserName") %>
                    </ItemTemplate>
                </asp:TemplateField>
               <asp:TemplateField>
    <ItemTemplate>
        <asp:Button ID="btnChat" runat="server" Text="Chat" CommandName="Select" 
                    CommandArgument='<%# Container.DataItemIndex %>' CssClass="select-button" />
    </ItemTemplate>
</asp:TemplateField>

            </Columns>
        </asp:GridView>
    </div>
</div>

    </form>
</body>
</html>
