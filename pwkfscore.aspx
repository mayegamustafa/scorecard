<%@ Page Language="C#" AutoEventWireup="true" CodeFile="pwkfscore.aspx.cs" Inherits="pwkfscore" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div>
    
        <asp:GridView ID="GridView1" runat="server" DataSourceID="fweekly" EnableModelValidation="True">
        </asp:GridView>
        <asp:SqlDataSource ID="fweekly" runat="server"></asp:SqlDataSource>
    
    </div>
    </form>
</body>
</html>
