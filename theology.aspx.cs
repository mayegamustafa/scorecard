using System;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

public partial class finance : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lblUsername.Text = "Logged in as: " + Session["Username"];
        lblDepartment.Text = "Department: " + Session["department_name"];

        // Check if the session variables are set
        if (Session["DepartmentID"] == null || Session["UserID"] == null)
        {
            // Handle the null case, e.g., redirect to login page
            Response.Redirect("login.aspx");
            return;
        }

        if (!IsPostBack)
        {
            BindGrid();
        }
    }

    protected void Button1_Click(object sender, EventArgs e)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            con.Open();
            string query = "INSERT INTO scores (school_id, area_id, week_id, score, department_id, UserID) VALUES (@school_id, @area_id, @week_id, @score, @department_id, @UserID)";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@school_id", DropDownList1.SelectedValue);
                cmd.Parameters.AddWithValue("@area_id", DropDownList2.SelectedValue);
                cmd.Parameters.AddWithValue("@week_id", DropDownList3.SelectedValue);
                cmd.Parameters.AddWithValue("@score", TextBox1.Text);
                cmd.Parameters.AddWithValue("@department_id", Session["DepartmentID"]);
                cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                cmd.ExecuteNonQuery();

                // Clear the text box after saving
                TextBox1.Text = "";

                // Display a notification
                ClientScript.RegisterStartupScript(this.GetType(), "showNotification", "showNotification('Score saved successfully!');", true);

                // Refresh the GridView to show the updated data
                BindGrid();
            }
        }
    }

    // Overloaded BindGrid method to handle sorting and default parameters
    private void BindGrid()
    {
        BindGrid(null, "ASC");
    }

    private void BindGrid(string sortExpression)
    {
        BindGrid(sortExpression, "ASC");
    }

    private void BindGrid(string sortExpression, string sortDirection)
    {
        if (Session["DepartmentID"] != null && Session["UserID"] != null)
        {
            string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
            using (SqlConnection con = new SqlConnection(connectionString))
            {
                string query = "SELECT scores.score_id, schools.school_name, areas.area_name, weeks.week_name, scores.score " +
                               "FROM scores " +
                               "JOIN schools ON scores.school_id = schools.school_id " +
                               "JOIN areas ON scores.area_id = areas.area_id " +
                               "JOIN weeks ON scores.week_id = weeks.week_id " +
                               "WHERE scores.department_id = @department_id AND scores.UserID = @UserID " +
                               "ORDER BY " + (sortExpression ?? "scores.score_id") + " " + sortDirection;

                using (SqlCommand cmd = new SqlCommand(query, con))
                {
                    cmd.Parameters.AddWithValue("@department_id", Session["DepartmentID"]);
                    cmd.Parameters.AddWithValue("@UserID", Session["UserID"]);

                    con.Open();
                    SqlDataReader reader = cmd.ExecuteReader();
                    GridView1.DataSource = reader;
                    GridView1.DataBind();
                }
            }
        }
        else
        {
            Response.Redirect("login.aspx");
        }
    }

    // Sorting event
    protected void GridView1_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;
        string sortDirection = e.SortDirection == SortDirection.Ascending ? "ASC" : "DESC";

        BindGrid(sortExpression, sortDirection);
    }

    // Editing event
    protected void GridView1_RowEditing(object sender, GridViewEditEventArgs e)
    {
        GridView1.EditIndex = e.NewEditIndex;
        BindGrid();
    }

    // Updating event
    protected void GridView1_RowUpdating(object sender, GridViewUpdateEventArgs e)
    {
        int score_id = (int)GridView1.DataKeys[e.RowIndex].Value;
        string score = ((TextBox)GridView1.Rows[e.RowIndex].Cells[3].Controls[0]).Text;

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "UPDATE scores SET score = @score WHERE score_id = @score_id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@score_id", score_id);
                cmd.Parameters.AddWithValue("@score", score);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        GridView1.EditIndex = -1;
        BindGrid();
    }

    // Canceling edit
    protected void GridView1_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
    {
        GridView1.EditIndex = -1;
        BindGrid();
    }

    // Deleting event
    protected void GridView1_RowDeleting(object sender, GridViewDeleteEventArgs e)
    {
        int score_id = (int)GridView1.DataKeys[e.RowIndex].Value;

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection con = new SqlConnection(connectionString))
        {
            string query = "DELETE FROM scores WHERE score_id = @score_id";
            using (SqlCommand cmd = new SqlCommand(query, con))
            {
                cmd.Parameters.AddWithValue("@score_id", score_id);

                con.Open();
                cmd.ExecuteNonQuery();
            }
        }

        BindGrid();
    }
}
