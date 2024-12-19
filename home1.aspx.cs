using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data.SqlClient;

public partial class home1 : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            if (Session["UserID"] != null && Session["department_id"] != null)
            {
                int departmentId = (int)Session["department_id"];
                string username = GetUsername((int)Session["UserID"]);
                string departmentName = GetDepartmentName(departmentId);

                lblUsername.Text = "User: " + username;
                lblDepartment.Text = "Department: " + departmentName;

                BindAreaDropdown(departmentId);
            }
            else
            {
                Response.Redirect("Login.aspx"); // Redirect to login if not logged in
            }
        }
    }


    private void BindAreaDropdown(int departmentId)
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT area_id, area_name FROM areas WHERE department_id = @department_id";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@department_id", departmentId);
                conn.Open();

                SqlDataReader reader = cmd.ExecuteReader();
                ddlAreas.DataSource = reader;
                ddlAreas.DataTextField = "area_name";
                ddlAreas.DataValueField = "area_id";
                ddlAreas.DataBind();
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


    protected void btnSave_Click(object sender, EventArgs e)
    {
         if (Session["UserID"] != null && Session["DepartmentID"] != null)
        {
            int userId = (int)Session["UserID"];
            int departmentId = (int)Session["DepartmentID"];
            int areaId = int.Parse(ddlAreas.SelectedValue);
            double score = double.Parse(txtScore.Text);

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "INSERT INTO scores (UserID, department_id, area_id, score) VALUES (@UserID, @department_id, @area_id, @score)";
                using (SqlCommand cmd = new SqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@UserID", userId);
                    cmd.Parameters.AddWithValue("@department_id", departmentId);
                    cmd.Parameters.AddWithValue("@area_id", areaId);
                    cmd.Parameters.AddWithValue("@score", score);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }
            }

            lblMessage.Text = "Score saved successfully!";
        }
        else
        {
            Response.Redirect("Login.aspx");
        }
    }

    }
