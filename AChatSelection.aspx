<%@ Page Language="C#" AutoEventWireup="true" CodeFile="AChatSelection.aspx.cs" Inherits="ChatSelection" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Select Chat</title>
</head>
<body>
    <form id="form1" runat="server">
        <h2>Select a Department or User to Chat</h2>
        <asp:GridView ID="gvUsers" runat="server" AutoGenerateColumns="False" OnRowCommand="gvUsers_RowCommand">
            <Columns>
                <asp:TemplateField>
                    <ItemTemplate>
                        <asp:Label ID="lblUserID" runat="server" Text='<%# Eval("UserID") %>' Visible="false"></asp:Label>
                        <%# Eval("UserName") %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:ButtonField ButtonType="Button" CommandName="Select" Text="Chat" />
            </Columns>
        </asp:GridView>
    </form>
</body>
</html>
