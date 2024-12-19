using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI;

public partial class SendMessage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadMessages();
            SetChatWithLabel(); // Set the label text
        }
    }

    

    private string GetUsernameById(int userId)
    {
        string username = string.Empty;
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Username FROM Users WHERE UserID = @UserID";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@UserID", userId);

            conn.Open();
            username = cmd.ExecuteScalar() as string;
        }

        return username;
    }

    protected int GetCurrentUserId()
    {
        if (Session["UserID"] != null)
        {
            return (int)Session["UserID"];
        }
        else
        {
            throw new Exception("User is not logged in. Please log in.");
        }
    }

    private void LoadMessages()
    {
        int senderUserId = GetCurrentUserId();
        int recipientUserId = Convert.ToInt32(Request.QueryString["userId"]);

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT M.MessageText, M.DateSent, M.SenderUserID, U.Username AS SenderName " +
                           "FROM Messages M " +
                           "INNER JOIN Users U ON M.SenderUserID = U.UserID " +
                           "WHERE (M.SenderUserID = @SenderUserID AND M.RecipientUserID = @RecipientUserID) " +
                           "OR (M.SenderUserID = @RecipientUserID AND M.RecipientUserID = @SenderUserID) " +
                           "ORDER BY M.DateSent";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SenderUserID", senderUserId);
            cmd.Parameters.AddWithValue("@RecipientUserID", recipientUserId);

            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            rptMessages.DataSource = dt;
            rptMessages.DataBind();

            // Automatically scroll to the bottom after loading messages
            ScriptManager.RegisterStartupScript(this, GetType(), "ScrollToBottom", "setTimeout(() => { const chatContainer = document.getElementById('chatContainer'); chatContainer.scrollTop = chatContainer.scrollHeight; }, 100);", true);
        }
    }


    protected void btnSendMessage_Click(object sender, EventArgs e)
    {
        string messageText = txtMessage.Text.Trim();
        if (string.IsNullOrEmpty(messageText))
        {
            return; // Don't send empty messages
        }

        int senderUserId = GetCurrentUserId();
        int recipientUserId = Convert.ToInt32(Request.QueryString["userId"]);

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "INSERT INTO Messages (SenderUserID, RecipientUserID, MessageText, IsRead) " +
                           "VALUES (@SenderUserID, @RecipientUserID, @MessageText, 0)";

            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@SenderUserID", senderUserId);
            cmd.Parameters.AddWithValue("@RecipientUserID", recipientUserId);
            cmd.Parameters.AddWithValue("@MessageText", messageText);

            conn.Open();
            cmd.ExecuteNonQuery();
        }

        // Clear the input box
        txtMessage.Text = string.Empty;

        // Reload messages to show the new message
        LoadMessages();
    }

    protected void timerRefresh_Tick(object sender, EventArgs e)
    {
        LoadMessages();
    }

    private void SetChatWithLabel()
    {
        if (Request.QueryString["userId"] != null)
        {
            int recipientUserId = Convert.ToInt32(Request.QueryString["userId"]);
            lblChatWith.Text = GetUsernameById(recipientUserId); // This gets the username of the recipient
        }
    }

}
