<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gmanage.aspx.cs" Inherits="Gmanage" %>
<%@ Register Src="~/Sidebar6.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users</title>
    <style>
        body {
            font-family: Arial, sans-serif;
            margin: 0;
            padding: 0;
            display: flex;
            flex-direction: column;
            min-height: 100vh;
        }

        .container {
            display: flex;
            flex-direction: row;
            flex: 1;
        }

        .sidebar {
            width: 250px;
            background-color: #f4f4f4;
            padding: 20px;
            box-shadow: 2px 0 5px rgba(0, 0, 0, 0.1);
            box-sizing: border-box;
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


        .content {
            flex: 1;
            padding: 20px;
            box-sizing: border-box;
        }

        .form-container1 {
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
            background-color: #f9f9f9;
            max-width: 500px;
            width: 100%;
            box-sizing: border-box;
            margin: 0 auto 20px auto;
        }

        .form-container1 h3 {
            margin-top: 0;
            text-align: center;
        }

        .form-group1 {
            margin-bottom: 15px;
        }

        .form-group1 label {
            display: block;
            font-weight: bold;
            margin-bottom: 5px;
        }

        .form-control {
            width: 100%;
            padding: 8px;
            border: 1px solid #ccc;
            border-radius: 4px;
            box-sizing: border-box;
        }

        .grid-container {
            overflow-x: auto;
        }

        .modal {
            display: none;
            position: fixed;
            top: 0;
            left: 0;
            width: 100%;
            height: 100%;
            background-color: rgba(0, 0, 0, 0.5);
            justify-content: center;
            align-items: center;
        }

        .modal-content {
            background: #f9f9f9;
            border: 1px solid #ddd;
            border-radius: 5px;
            padding: 20px;
            max-width: 500px;
            width: 100%;
            box-sizing: border-box;
        }

        .modal-content h3 {
            margin-top: 0;
            text-align: center;
        }

        .notification-popup {
            position: fixed;
            top: 10px;
            right: 10px;
            background: #007bff;
            color: #fff;
            padding: 10px;
            border-radius: 5px;
            display: none;
            z-index: 1000;
        }

        .notification-popup.error {
            background: #dc3545;
        }

        @media (max-width: 768px) {
            .container {
                flex-direction: column;
            }

            .sidebar {
                width: 100%;
                box-shadow: none;
                padding: 10px;
            }

            .navbar {
                font-size: 1.2em;
                padding: 10px;
            }
        }

        @media (max-width: 576px) {
            .form-container1 {
                padding: 15px;
                margin: 0 auto;
            }

            .form-control {
                padding: 6px;
            }

            .grid-container {
                padding: 10px;
            }
        }
    </style>
    <script type="text/javascript">
        function openModal() {
            document.getElementById('modalEditUser').style.display = 'flex';
        }

        function closeModal() {
            document.getElementById('modalEditUser').style.display = 'none';
        }

        function showNotification(message, isError) {
            var notification = document.getElementById('notificationPopup');
            notification.textContent = message;
            notification.classList.toggle('error', isError);
            notification.style.display = 'block';
            setTimeout(function () {
                notification.style.display = 'none';
            }, 300); // Adjust this value as needed
        }

    </script>
</head>
<body>
    <form id="form1" runat="server">
         <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <div class="sidebar">
            <uc:Sidebar ID="Sidebar" runat="server" />
        </div>

             <div class="navbar">
            <button class="menu-toggle" onclick="toggleSidebar()">☰</button>
             <img src="assets/images/school-badge.jpg" alt="School Badge" />
            <h1> <asp:Label ID="lblDepartmentName" runat="server" CssClass="department-name-label" />
 - SAK Scorecard</h1>
        </div>

        <div class="container">
            <div class="content">
                <div class="notification-popup" id="notificationPopup"></div>

                 <div id="notification" class="notification"></div>

                <!-- Add this inside your form -->
<asp:Label ID="lblNotification" runat="server" CssClass="notification-label" Style="display:none;"></asp:Label>


                <!-- Add User Section -->
                <div class="form-container1">
                    <h3>Add New User</h3>

                    <!-- Form Controls -->
                    <div class="form-group1">
                        <asp:Label ID="lblUsername" runat="server" Text="Username:"></asp:Label>
                        <asp:TextBox ID="txtUsername" runat="server" CssClass="form-control" Placeholder="Username"></asp:TextBox>
                    </div>

                    <div class="form-group1">
                        <asp:Label ID="lblPassword" runat="server" Text="Password:"></asp:Label>
                        <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" CssClass="form-control" Placeholder="Password"></asp:TextBox>
                    </div>

                    <div class="form-group1">
                        <asp:Label ID="lblDepartment" runat="server" Text="Department:"></asp:Label>
                        <asp:DropDownList ID="ddlDepartment" runat="server" CssClass="form-control"></asp:DropDownList>
                    </div>

                    <div class="form-group1">
                        <asp:Button ID="btnAddUser" runat="server" Text="Add User" OnClick="btnAddUser_Click" 
                                    Style="background-color: green; color: white; cursor:pointer;" />
                    </div>

                    <!-- Hidden Field for Selected User ID -->
                    <asp:HiddenField ID="hfSelectedUserID" runat="server" />
                </div>
                
                <!-- GridView Section -->
               <!-- GridView Section -->
<div class="grid-container">
    <asp:GridView ID="GridView1" runat="server" OnRowCommand="GridView1_RowCommand" AutoGenerateColumns="False" AllowPaging="True" PageSize="10" OnPageIndexChanging="GridView1_PageIndexChanging">
        <Columns>
            <asp:BoundField DataField="Username" HeaderText="Username" />
            <asp:BoundField DataField="Password" HeaderText="Password" />
            <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnEdit" runat="server" CommandName="EditUser" Text="Edit" 
                        CommandArgument='<%# Eval("UserID") %>' OnClientClick="openModal(); return false;" 
                        Style="background-color: blue; color: white; cursor:pointer;" />
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField>
                <ItemTemplate>
                    <asp:Button ID="btnDelete" runat="server" CommandName="DeleteUser" Text="Delete" 
                        CommandArgument='<%# Eval("UserID") %>' Style="background-color: red; color: white; cursor:pointer;" />
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</div>


                <!-- Modal for Editing Users -->
                <asp:Panel ID="modalEditUser" runat="server" CssClass="modal">
                    <div class="modal-content">
                        <h3>Edit User</h3>
                        <div class="form-group1">
                            <asp:Label ID="lblModalUsername" runat="server" Text="Username:"></asp:Label>
                            <asp:TextBox ID="txtModalUsername" runat="server" placeholder="Username" CssClass="form-control" />
                        </div>
                        <div class="form-group1">
                            <asp:Label ID="lblModalPassword" runat="server" Text="Password:"></asp:Label>
                            <asp:TextBox ID="txtModalPassword" runat="server" placeholder="Password" CssClass="form-control" />
                        </div>
                        <div class="form-group1">
                            <asp:Label ID="lblModalDepartment" runat="server" Text="Department:"></asp:Label>
                            <asp:DropDownList ID="ddlModalDepartment" runat="server" CssClass="form-control" />
                        </div>
                        <div class="form-group1">
                            <asp:Button ID="btnModalUpdateUser" runat="server" Text="Update User" OnClick="btnModalUpdateUser_Click" 
                                        Style="background-color: green; color: white; cursor:pointer;" />
                            <asp:Button ID="btnCloseModal" runat="server" Text="Close" OnClientClick="closeModal(); return false;" 
                                        Style="background-color: orange; color: white; cursor:pointer;" />
                        </div>
                        <asp:Label ID="lblModalError" runat="server" Visible="false" ForeColor="Red" />
                    </div>
                </asp:Panel>
            </div>
        </div>
    </form>
     <script>
         function openModal() {
             document.getElementById('editModal').style.display = 'block';
         }

         function closeModal() {
             document.getElementById('editModal').style.display = 'none';
         }
    </script>
    <script>
        function showNotification(message, isError) {
            var notification = document.getElementById('notificationPopup');
            notification.textContent = message;
            notification.classList.toggle('error', isError);
            notification.style.display = 'block';
            setTimeout(function () {
                notification.style.display = 'none';
            }, 300); // Adjust this value as needed
        }

    
    </script>
</body>
</html>
