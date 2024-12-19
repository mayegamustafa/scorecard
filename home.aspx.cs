using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Configuration;

public partial class home : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] != null && Session["DepartmentID"] != null)
            {
                int departmentId = (int)Session["DepartmentID"];
                string username = GetUsername((int)Session["UserID"]);
                string departmentName = GetDepartmentName(departmentId);

                lblUsername.Text = "User: " + username;
                lblDepartment.Text = "Department: " + departmentName;


            }
            else
            {
                Response.Redirect("Login.aspx"); // Redirect to login if not logged in
            }
        }
    }



    private string GetUsername(int userId)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT Username FROM Users WHERE UserID = @UserID";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
        }
    }

    private string GetDepartmentName(int departmentId)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT department_name FROM departments WHERE department_id = @DepartmentID";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                conn.Open();

                return cmd.ExecuteScalar().ToString();
            }
        }
    }



    protected void Button1_Click(object sender, EventArgs e)
    {

        if (Session["UserID"] != null && Session["DepartmentID"] != null)
        {
            // Retrieve values from the form
            int userId = (int)Session["UserID"];
            int departmentId = (int)Session["DepartmentID"];

            int schoolId = int.Parse(DropDownList1.SelectedValue);
            int areaId = int.Parse(DropDownList2.SelectedValue);
            int weekId = int.Parse(DropDownList3.SelectedValue);
            decimal score;
            if (!decimal.TryParse(TextBox1.Text, out score))
            {
                // Handle invalid score input
                Response.Write("<script>alert('Invalid score value');</script>");
                return;
            }

            // Define connection string and SQL query
            string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
            string query = "INSERT INTO scores (UserID, department_id, week_id, school_id, area_id, score) VALUES (@UserID, @department_id, @week_id, @school_id, @area_id, @score)";

            // Insert data into the scores table
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@UserID", userId);
                //  cmd.Parameters.AddWithValue("@DepartmentID", departmentId);  1
                command.Parameters.AddWithValue("@department_id", departmentId); // Assuming department_id is always 1; change as needed
                command.Parameters.AddWithValue("@week_id", weekId);
                command.Parameters.AddWithValue("@school_id", schoolId);
                command.Parameters.AddWithValue("@area_id", areaId);
                command.Parameters.AddWithValue("@score", score);

                try
                {
                    connection.Open();
                    command.ExecuteNonQuery();
                    Response.Write("<script>alert('Score saved successfully');</script>");
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Error: " + ex.Message + "');</script>");
                }
            }
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }
}