using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class fmanage : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session["DepartmentID"] == null)
        {
            lblError.Text = "You are not logged in. Please log in.";
            Response.Redirect("Login.aspx");
        }
        else
        {
            if (!IsPostBack)
            {
                // Load departments into dropdown
                LoadDepartments();

                // Set default selection
                ddlDepartment.SelectedValue = Session["DepartmentID"].ToString();
            }

            // Fetch and display the department name based on the logged-in department ID
            int departmentId;
            if (int.TryParse(Session["DepartmentID"].ToString(), out departmentId))
            {
                DisplayDepartmentName(departmentId);
            }
            else
            {
                lblError.Text = "Invalid Department ID.";
                lblError.ForeColor = System.Drawing.Color.Red;
            }

            lblDebug.Text = "Department ID: " + Session["DepartmentID"].ToString();
        }
    }


    private void DisplayDepartmentName(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT department_name 
        FROM departments 
        WHERE department_id = @departmentId";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the department name

                if (result != null)
                {
                    // Dynamically display the department name on the page (e.g., in a Label control)
                    lblDepartmentName.Text = result.ToString();
                }
                else
                {
                    lblDepartmentName.Text = "Unknown Department";  // Fallback message if department not found
                }
            }
        }
    }

    private void LoadDepartments()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            // Update the query to exclude department ID 6
            string query = "SELECT department_id, department_name FROM departments WHERE department_id <> 6";
            SqlCommand cmd = new SqlCommand(query, conn);

            try
            {
                conn.Open();
                SqlDataReader reader = cmd.ExecuteReader();
                ddlDepartment.DataSource = reader;
                ddlDepartment.DataTextField = "department_name";
                ddlDepartment.DataValueField = "department_id";
                ddlDepartment.DataBind();

                // Set default selection if the ID is not 6
                if (ddlDepartment.Items.FindByValue(Session["DepartmentID"].ToString()) != null)
                {
                    ddlDepartment.SelectedValue = Session["DepartmentID"].ToString();
                }
                else
                {
                    lblError.Text = "The selected department is not available.";
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
            catch (Exception ex)
            {
                lblError.Text = "An error occurred while loading departments: " + ex.Message;
                lblError.ForeColor = System.Drawing.Color.Red;
            }
        }
    }


    protected void btnAddArea_Click(object sender, EventArgs e)
    {
        string areaName = txtAreaName.Text;
        int departmentID = Convert.ToInt32(Session["DepartmentID"]);

        if (!string.IsNullOrEmpty(areaName) && departmentID > 0)
        {
            // Insert into the database
            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string insertQuery = "INSERT INTO areas (area_name, department_id) VALUES (@area_name, @department_id)";
                SqlCommand cmd = new SqlCommand(insertQuery, conn);
                cmd.Parameters.AddWithValue("@area_name", areaName);
                cmd.Parameters.AddWithValue("@department_id", departmentID);

                try
                {
                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        // Clear the TextBox
                        txtAreaName.Text = "";


                       // lblError.Text = "Area added successfully!";
                      //  lblError.ForeColor = System.Drawing.Color.Green;

                        lblNotification.Text = "Area added successfully!";
                        ScriptManager.RegisterStartupScript(this, GetType(), "showNotification", "showNotification();", true);

                  

                        // Optionally, refresh the GridView to show the new entry
                        GridView1.DataBind();
                    }
                    else
                    {
                        lblError.Text = "Error adding the area.";
                        lblError.ForeColor = System.Drawing.Color.Red;
                    }
                }
                catch (Exception ex)
                {
                    lblError.Text = "An error occurred: " + ex.Message;
                    lblError.ForeColor = System.Drawing.Color.Red;
                }
            }
        }
        else
        {
            lblError.Text = "Area Name cannot be empty.";
            lblError.ForeColor = System.Drawing.Color.Red;
        }
    }

    protected void ddlDepartment_SelectedIndexChanged(object sender, EventArgs e)
    {
        // Refresh the GridView
        GridView1.DataBind();
    }

}