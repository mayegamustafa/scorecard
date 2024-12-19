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
            lblDebug.Text = "Department ID: " + Session["DepartmentID"].ToString();
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
}