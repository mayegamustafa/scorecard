<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Gmanage.aspx.cs" Inherits="Gmanage" %>
<%@ Register Src="~/Sidebar.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html>
<head runat="server">
    <title>Manage Users</title>
    <style>
        /* Add styles for the modal and notification */

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <uc:Sidebar runat="server" />

            <!-- Main Content -->
            <div class="container">
                <asp:GridView ID="GridView1" runat="server" AutoGenerateColumns="False" OnRowCommand="GridView1_RowCommand" OnPageIndexChanging="GridView1_PageIndexChanging">
                    <Columns>
                        <asp:BoundField DataField="Username" HeaderText="Username" />
                        <asp:BoundField DataField="DepartmentName" HeaderText="Department" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Button ID="btnEdit" runat="server" CommandName="EditUser" CommandArgument='<%# Eval("UserID") %>' Text="Edit" />
                                <asp:Button ID="btnDelete" runat="server" CommandName="DeleteUser" CommandArgument='<%# Eval("UserID") %>' Text="Delete" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

                <asp:Label ID="lblNotification" runat="server" CssClass="notification" Style="display:none;" />

                <!-- Add User Form -->
                <div class="form-container">
                    <h3>Add User</h3>
                    <asp:TextBox ID="txtUsername" runat="server" placeholder="Username" />
                    <asp:TextBox ID="txtPassword" runat="server" TextMode="Password" placeholder="Password" />
                    <asp:DropDownList ID="ddlDepartment" runat="server" />
                    <asp:Button ID="btnAddUser" runat="server" Text="Add User" OnClick="btnAddUser_Click" />
                </div>
            </div>
        </div>

        <!-- Edit User Modal -->
        <div id="editModal" class="modal">
            <div class="modal-content">
                <span class="close" onclick="closeModal()">&times;</span>
                <h3>Edit User</h3>
                <asp:Label ID="lblModalError" runat="server" CssClass="error" Style="display:none;" />
                <asp:TextBox ID="txtModalUsername" runat="server" placeholder="Username" />
                <asp:TextBox ID="txtModalPassword" runat="server" TextMode="Password" placeholder="Password" />
                <asp:DropDownList ID="ddlModalDepartment" runat="server" />
                <asp:Button ID="btnModalUpdateUser" runat="server" Text="Update" OnClick="btnModalUpdateUser_Click" />
                <asp:Button ID="btnCloseModal" runat="server" Text="Close" OnClick="btnCloseModal_Click" />
                <asp:HiddenField ID="hfSelectedUserID" runat="server" />
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
</body>
</html>