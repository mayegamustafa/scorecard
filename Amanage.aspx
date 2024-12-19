<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Amanage.aspx.cs" Inherits="fmanage" Trace="false" %>
<%@ Register Src="~/Sidebar2.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head id="Head1" runat="server">
    <title>Manage Areas</title>
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

        /* Form and GridView styling */
        .form-container {
            width: 100%;
            max-width: 1500px;
            margin: 0 auto;
            padding: 20px;
            background-color: white;
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

        .form-group input[type="text"] {
            width: 100px;
            padding: 30px;
            border: 1px solid #ddd;
            border-radius: 4px;
            font-size: 14px;
        }

        .form-group button {
            background-color: #007bff;
            color: white;
            padding: 10px 15px;
            border: none;
            border-radius: 4px;
            cursor: pointer;
            font-size: 14px;
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

        .form-group button:hover {
            background-color: #0056b3;
        }

        /* GridView styling */
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

        /* Responsive styling */
        @media (max-width: 768px) {
            .sidebar {
                width: 100%;
                height: auto;
                position: relative;
                top: 0;
                padding: 10px;
                display: none; /* Hide sidebar on small screens */

               list-style: none; /* Remove bullets */
            font-weight: bold;

            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
            }

            .content {
                margin-left: 0;
                padding: 20px;
            }
        }


        .btn {
            border-style: none;
            border-color: inherit;
            border-width: medium;
            display: block;
            padding: 10px;
            background-color: #007bff;
            color: #fff;
            border-radius: 4px;
            font-size: 16px;
            cursor: pointer;
        }

        .btn:hover {
            background-color: #0056b3;
        }

        .form-container1 {
            width:500px;
            margin: 0 auto;
            padding: 20px;
            background-color: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 4px;
            box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
        }

        .form-container1 h2 {
            text-align: center;
            margin-bottom: 20px;
        }



        .form-control {}



    </style>
    <script type="text/javascript">
        function showNotification() {
            var notification = document.getElementById('<%= lblNotification.ClientID %>');
            notification.style.display = 'block';
            setTimeout(function () {
                notification.style.display = 'none';
            }, 3000); // Hide after 3 seconds (3000 ms)
        }
    </script>
</head>
<body>
    <form id="form1" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>

       

        <div class="sidebar">
            <uc:Sidebar ID="Sidebar" runat="server" />
        </div>
        <div class="navbar">
           <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1>SAK Scorecard</h1>
        </div>

        <div class="content" aria-atomic="True" aria-checked="false">
            <div class="form-container">
                <h2>Manage Areas</h2>

                
                <!-- Add Area Form -->
                 <div class="form-container1">
                    <h3>Add New Area</h3>
                     
                <!-- Notification Label -->
                <asp:Label ID="lblNotification" runat="server" Text="Area added successfully!" 
                           ForeColor="Green" Font-Bold="True" Style="display:none;"></asp:Label>

                    <asp:Label ID="lblAreaName" runat="server" Text="Area Name:"></asp:Label>
                    <asp:TextBox ID="txtAreaName" runat="server" CssClass="form-control" Width="415px"></asp:TextBox>
                   

                    <div class="form-group1">
                        <asp:Button ID="btnAddArea" runat="server" Text="Add Area" OnClick="btnAddArea_Click" CssClass="btn btn-primary" Width="438px" />
              </div>
                </div>

                <!-- GridView to display and manage areas -->
                <div class="gridview-container">
                    <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" DataSourceID="SqlDataSource1"
                                  DataKeyNames="area_id" AllowPaging="True" AllowSorting="True" AutoGenerateEditButton="True" EnableModelValidation="True">
                        <Columns>
                            <asp:CommandField ShowDeleteButton="True" />
                            <asp:BoundField DataField="area_id" HeaderText="Area ID" ReadOnly="True" SortExpression="area_id" />
                            <asp:BoundField DataField="area_name" HeaderText="Area Name" SortExpression="area_name" />
                            <asp:BoundField DataField="department_id" HeaderText="Department ID" SortExpression="department_id" ReadOnly="True" />
                        </Columns>
                    </asp:GridView>
                </div>



                <!-- SqlDataSource to manage areas -->
                <asp:SqlDataSource ID="SqlDataSource1" runat="server"
                    ConnectionString="<%$ ConnectionStrings:s_cardConnectionString %>"
                    SelectCommand="SELECT area_id, area_name, department_id FROM areas WHERE department_id = @DepartmentID"
                    InsertCommand="INSERT INTO areas (area_name, department_id) VALUES (@area_name, @department_id)"
                    UpdateCommand="UPDATE areas SET area_name = @area_name WHERE area_id = @area_id AND department_id = @department_id"
                    DeleteCommand="DELETE FROM areas WHERE area_id = @area_id AND department_id = @department_id">
                    
                    <SelectParameters>
                        <asp:SessionParameter Name="DepartmentID" SessionField="DepartmentID" Type="Int32" />
                    </SelectParameters>
                    <InsertParameters>
                        <asp:Parameter Name="area_name" Type="String" />
                        <asp:SessionParameter Name="department_id" SessionField="DepartmentID" Type="Int32" />
                    </InsertParameters>
                    <UpdateParameters>
                        <asp:Parameter Name="area_name" Type="String" />
                        <asp:Parameter Name="area_id" Type="Int32" />
                        <asp:SessionParameter Name="department_id" SessionField="DepartmentID" Type="Int32" />
                    </UpdateParameters>
                    <DeleteParameters>
                        <asp:Parameter Name="area_id" Type="Int32" />
                        <asp:SessionParameter Name="department_id" SessionField="DepartmentID" Type="Int32" />
                    </DeleteParameters>
                </asp:SqlDataSource>
                
                <br />
                <asp:Label ID="lblError" runat="server" Text=""></asp:Label>
                <asp:Label ID="lblDebug" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </form>
</body>
</html>
