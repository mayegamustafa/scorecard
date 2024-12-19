using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class ChatSelection : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadUsersAndDepartments();
        }
    }

    private void LoadUsersAndDepartments()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string userQuery = "SELECT UserID, UserName, DepartmentID FROM Users WHERE DepartmentID != @LoggedInDepartmentID";
            SqlCommand cmd = new SqlCommand(userQuery, conn);
            cmd.Parameters.AddWithValue("@LoggedInDepartmentID", Convert.ToInt32(Session["DepartmentID"]));
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }
    }

    private void LoadDepartments()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string departmentQuery = "SELECT department_id, department_name FROM departments";
            SqlCommand cmd = new SqlCommand(departmentQuery, conn);
            conn.Open();
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            gvUsers.DataSource = dt;
            gvUsers.DataBind();
        }
    }

    /* protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
     {
         if (e.CommandName == "Select")
         {
             // Get the selected row index
             int rowIndex = Convert.ToInt32(e.CommandArgument);
             GridViewRow row = gvUsers.Rows[rowIndex];

             // Find the label that contains the UserID
             Label lblUserID = (Label)row.FindControl("lblUserID");
             int userId = Convert.ToInt32(lblUserID.Text);

             // Redirect to the chat page with the selected user ID
             Response.Redirect("Chat.aspx?userId=" + userId);
         }
     }
     */
    protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            // Debugging line
            string commandArg = e.CommandArgument.ToString();

            // Declare the variable before using it
            int rowIndex;
            // Check if the argument is a valid number
            if (!int.TryParse(commandArg, out rowIndex))
            {
                // Log or handle the error
                Response.Write("<script>alert('Invalid selection.');</script>");
                return; // Exit the method
            }

            // Get the selected row
            GridViewRow row = gvUsers.Rows[rowIndex];
            Label lblUserID = (Label)row.FindControl("lblUserID");
            int userId = Convert.ToInt32(lblUserID.Text);
            Response.Redirect("Chat.aspx?userId=" + userId);
        }
    }

    /* protected void gvUsers_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "Select")
        {
            // Get the UserID from the CommandArgument
            int userId = Convert.ToInt32(e.CommandArgument);

            // Redirect to the chat page with the selected user ID
            Response.Redirect("Chat.aspx?userId=" + userId);
        }
    } */


}
