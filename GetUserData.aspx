<%@ Page Language="C#" AutoEventWireup="true" CodeFile="GetUserData.aspx.cs" Inherits="GetUserData" %>
<%@ Import Namespace="System.Web.Script.Services" %>
<%@ Import Namespace="System.Web.Services" %>

<script runat="server">
    [WebMethod]
    [ScriptMethod(ResponseFormat = ResponseFormat.Json)]
    public static User GetUserData(int userID)
    {
        // Fetch user data from the database based on userID
        return new User
        {
            Username = "sampleUsername",
            Password = "samplePassword",
            DepartmentID = 1
        };
    }

    public class User
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public int DepartmentID { get; set; }
    }
</script>
