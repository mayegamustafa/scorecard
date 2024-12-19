<%@ Page Language="C#" AutoEventWireup="true" CodeFile="Chat.aspx.cs" Inherits="Chat" %>
<%@ Register Src="~/Sidebar2.ascx" TagPrefix="uc" TagName="Sidebar" %>

<!DOCTYPE html>
<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title>Chat</title>
    <style>
        body {
            font-family: 'Helvetica Neue', Helvetica, Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }

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
        }

        .content {
            margin-top: 70px; /* Adjusted for navbar height */
            padding: 20px;
            display: flex;
            justify-content: center;
        }

        #chatContainer {
            width: 100%;
            max-width: 600px; /* Maximum width for larger screens */
            height: 80vh; /* Use viewport height for responsive design */
            border: 1px solid #ccc;
            border-radius: 8px;
            background-color: #fff;
            display: flex;
            flex-direction: column;
            box-shadow: 0 2px 10px rgba(0, 0, 0, 0.1);
            margin: auto; /* Centering the chat container */
        }

        #chatHeader {
            padding: 10px;
            background-color: #007bff; /* Header color */
            color: white;
            border-top-left-radius: 8px;
            border-top-right-radius: 8px;
            text-align: center;
        }

        #ChatMessagesContainer {
            flex: 1;
            overflow-y: auto;
            padding: 10px;
            border-bottom: 1px solid #ccc;
        }

        .sent, .received {
            padding: 10px;
            margin: 5px 0;
            border-radius: 10px;
            max-width: 70%; /* Limit the width of messages */
            word-wrap: break-word; /* Break long words */
        }

        .sent {
            align-self: flex-end;
            background-color: #dcf8c6;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
        }

        .received {
            align-self: flex-start;
            background-color: #fff;
            box-shadow: 0 1px 3px rgba(0, 0, 0, 0.2);
        }

        textarea {
            width: calc(100% - 22px); /* Full width minus padding and border */
            height: 60px;
            padding: 10px;
            border: 1px solid #ccc;
            border-radius: 5px;
            resize: none; /* Disable resize */
            margin-bottom: 10px; /* Space below the textarea */
        }

        button {
            background-color: #007bff; /* Button color */
            color: white;
            border: none;
            padding: 10px;
            border-radius: 5px;
            cursor: pointer;
            transition: background-color 0.3s;
            margin-right: 5px; /* Space between buttons */
        }

        button:hover {
            background-color: #0056b3; /* Darker shade on hover */
        }

        #btnClearChat {
            background-color: #dc3545; /* Danger color */
        }

        #btnClearChat:hover {
            background-color: #c82333; /* Darker shade on hover */
        }

        .timestamp {
            float: right; /* Aligns the timestamp to the right */
            font-size: 0.8em; /* Smaller font size for the timestamp */
            color: gray; /* Gray color for the timestamp */
            margin-top: 5px; /* Space above the timestamp */
        }

        /* Responsive styling */
        @media (max-width: 768px) {
            #chatContainer {
                width: 100%; /* Full width on smaller screens */
                height: 80vh; /* Adjust height for smaller screens */
            }
        }
    </style>
</head>
<body>
    <form id="form1" runat="server">
        

        <div class="content">
            <div class="sidebar">
                <uc:Sidebar ID="Sidebar" runat="server" />
            </div>
            <div class="navbar">
            <img src="assets/images/school-badge.jpg" alt="School Badge" style="height: 50px; vertical-align: middle; margin-right: 15px;" />
            <h1>SAK Scorecard</h1>
        </div>

            <div id="chatContainer" runat="server">
                <div id="chatHeader">
                    Chat with: <span id="lblChatWith" runat="server"></span> <!-- Placeholder for the chat partner's name -->
                </div>
                <div id="ChatMessagesContainer" runat="server"></div>
                <textarea id="txtMessage" runat="server"></textarea>
                <button id="btnSend" runat="server" onserverclick="btnSend_Click">Send</button>
                <button id="btnClearChat" runat="server" onserverclick="btnClearChat_Click">Clear Chat</button>
                <asp:Label ID="lblMessage" runat="server" ForeColor="Green" />
            </div>
        </div>
    </form>
</body>
</html>
