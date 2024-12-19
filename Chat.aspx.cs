using System;
using System.Data.SqlClient;
using System.Web.UI;
using System.Configuration;

public partial class Chat : Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMessages();
            SetChatPartnerName();
        }
    }

    private int GetReceiverId()
    {
        if (Request.QueryString["userId"] != null)
        {
            return Convert.ToInt32(Request.QueryString["userId"]);
        }
        else
        {
            throw new Exception("Receiver ID not found in query string.");
        }
    }

    private void LoadMessages()
    {
        int userId = Convert.ToInt32(Session["UserID"]);
        int receiverId = GetReceiverId();

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT m.MessageText, m.SenderID, u.UserName, m.Timestamp FROM Messages m JOIN Users u ON m.SenderID = u.UserID WHERE (m.SenderID = @SenderID AND m.ReceiverID = @ReceiverID) OR (m.SenderID = @ReceiverID AND m.ReceiverID = @SenderID) ORDER BY m.Timestamp";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SenderID", userId);
            cmd.Parameters.AddWithValue("@ReceiverID", receiverId);

            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                string messageClass = (reader["SenderID"].ToString() == userId.ToString()) ? "sent" : "received";
                ChatMessagesContainer.InnerHtml += "<div class='" + messageClass + "'><b>" + reader["UserName"] + ":</b> " + "<br>" + reader["MessageText"] + " <span class='timestamp'>" + reader["Timestamp"] + "</span></div>";
            }
        }
    }

    private void SetChatPartnerName()
    {
        int receiverId = GetReceiverId();

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT UserName FROM Users WHERE UserID = @ReceiverID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@ReceiverID", receiverId);

            conn.Open();
            object result = cmd.ExecuteScalar();
            if (result != null)
            {
                lblChatWith.InnerText = result.ToString(); // Set the chat partner's name
            }
            else
            {
                lblChatWith.InnerText = "Unknown User"; // Fallback if user not found
            }
        }
    }

    protected void btnSend_Click(object sender, EventArgs e)
    {
        int userId = Convert.ToInt32(Session["UserID"]);
        int receiverId = GetReceiverId();
        string messageText = txtMessage.Value;

        // Validate message text
        if (string.IsNullOrWhiteSpace(messageText))
        {
            return;
        }

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Messages (SenderID, ReceiverID, MessageText, Timestamp) VALUES (@SenderID, @ReceiverID, @MessageText, @Timestamp)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SenderID", userId);
            cmd.Parameters.AddWithValue("@ReceiverID", receiverId);
            cmd.Parameters.AddWithValue("@MessageText", messageText);
            cmd.Parameters.AddWithValue("@Timestamp", DateTime.Now);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        txtMessage.Value = ""; // Clear the text area after sending
        Response.Redirect(Request.RawUrl); // Reload the page to show the new message
    }

    protected void btnClearChat_Click(object sender, EventArgs e)
    {
        int userId = Convert.ToInt32(Session["UserID"]);
        int receiverId = GetReceiverId();

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM Messages WHERE (SenderID = @SenderID AND ReceiverID = @ReceiverID) OR (SenderID = @ReceiverID AND ReceiverID = @SenderID)";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SenderID", userId);
            cmd.Parameters.AddWithValue("@ReceiverID", receiverId);

            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
                ChatMessagesContainer.InnerHtml = ""; // Clear the displayed messages immediately
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }
    }
}
