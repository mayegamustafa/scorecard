<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMessage - Copy.aspx.cs" Inherits="SendMessage" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat</title>
    <style>
        .chat-container {
            width: 50%;
            margin: auto;
            border: 1px solid #ccc;
            padding: 10px;
            height: 400px;
            overflow-y: scroll;
        }
        .message {
            padding: 10px;
            margin: 5px;
            border-radius: 10px;
        }
        .sent {
            text-align: right;
            background-color: #dcf8c6;
        }
        .received {
            text-align: left;
            background-color: #fff;
        }
    </style>
</head>
<body>
    <h2>Chat with <asp:Label ID="lblChatWith" runat="server" /></h2>

    <div class="chat-container" id="chatContainer">
        <asp:Repeater ID="rptMessages" runat="server">
            <ItemTemplate>
                <div class="message <%# ((int)Eval("SenderUserID") == GetCurrentUserId()) ? "sent" : "received" %>">
                    <strong><%# ((int)Eval("SenderUserID") == GetCurrentUserId()) ? "You" : Eval("SenderName") %>:</strong>
                    <p><%# Eval("MessageText") %></p>
                    <span style="font-size:smaller;"><%# Eval("DateSent", "{0:g}") %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <form id="sendMessageForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        <label for="recipientUser">Recipient User:</label>
        <asp:DropDownList ID="ddlRecipientUser" runat="server"></asp:DropDownList>
        <br /><br />

        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="3" Columns="50"></asp:TextBox>
        <asp:Button ID="btnSendMessage" runat="server" Text="Send" OnClick="btnSendMessage_Click" />
        
        <asp:Timer ID="timerRefresh" runat="server" Interval="5000" OnTick="timerRefresh_Tick" />
    </form>
</body>
</html>
