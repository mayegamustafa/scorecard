<%@ Page Language="C#" AutoEventWireup="true" CodeFile="SendMessage.aspx.cs" Inherits="SendMessage" %>

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
            display: flex;
            flex-direction: column;
        }

        .sent {
            align-self: flex-end;
            background-color: #dcf8c6; /* Green for sent messages */
            text-align: right;
        }

        .received {
            align-self: flex-start;
            background-color: #fff; /* White for received messages */
            text-align: left;
        }

        .message {
            margin: 5px;
            padding: 10px;
            border-radius: 10px;
            max-width: 70%;
            box-shadow: 0px 1px 3px rgba(0, 0, 0, 0.1);
        }

        .message strong {
            display: block;
        }

        .message span {
            font-size: smaller;
            color: #888;
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
                    <span><%# Eval("DateSent", "{0:g}") %></span>
                </div>
            </ItemTemplate>
        </asp:Repeater>
    </div>

    <form id="sendMessageForm" runat="server">
        <asp:ScriptManager ID="ScriptManager1" runat="server" />
        
        <asp:TextBox ID="txtMessage" runat="server" TextMode="MultiLine" Rows="3" Columns="50"></asp:TextBox>
        <asp:Button ID="btnSendMessage" runat="server" Text="Send" OnClick="btnSendMessage_Click" />
        
        <asp:Timer ID="timerRefresh" runat="server" Interval="5000" OnTick="timerRefresh_Tick" />
    </form>

    <script type="text/javascript">
        // Automatically scroll to the bottom of the chat container when new messages are loaded
        window.onload = function() {
            var chatContainer = document.getElementById("chatContainer");
            chatContainer.scrollTop = chatContainer.scrollHeight;
        };
    </script>
</body>
</html>
