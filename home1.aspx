<%@ Page Language="C#" AutoEventWireup="true" CodeFile="home1.aspx.cs" Inherits="home1" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
        <div>
            <h2>Welcome to the Score Entry Page</h2>
            
            <asp:Label ID="lblUsername" runat="server" Text=""></asp:Label>
            
            <br />
            <asp:Label ID="lblDepartment" runat="server" Text=""></asp:Label>
            
            <br />
            <asp:Label ID="lblArea" runat="server" Text="Select Area:"></asp:Label>
            <asp:DropDownList ID="ddlAreas" runat="server" AutoPostBack="True">
            </asp:DropDownList>
            
            <br />
            <asp:Label ID="lblScore" runat="server" Text="Enter Score:"></asp:Label>
            <asp:TextBox ID="txtScore" runat="server"></asp:TextBox>
            
            <br />
            <asp:Button ID="btnSave" runat="server" Text="Save Score" OnClick="btnSave_Click" />
            
            <br />
            <asp:Label ID="lblMessage" runat="server" Text=""></asp:Label>
        </div>
    </form>
</body>
</html>
