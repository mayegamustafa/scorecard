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

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        if (Session["DepartmentID"] != null)
        {
            int departmentId = (int)Session["DepartmentID"];
            int schoolId = Convert.ToInt32(DropDownList1.SelectedValue);
            int areaId = Convert.ToInt32(DropDownList2.SelectedValue);
            int weekId = Convert.ToInt32(DropDownList3.SelectedValue);
            int score = Convert.ToInt32(TextBox1.Text);

            string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();
                string insertQuery = "INSERT INTO Scores (DepartmentID, SchoolID, AreaID, WeekID, Score) VALUES (@DepartmentID, @SchoolID, @AreaID, @WeekID, @Score)";
                using (SqlCommand cmd = new SqlCommand(insertQuery, conn))
                {
                    cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                    cmd.Parameters.AddWithValue("@SchoolID", schoolId);
                    cmd.Parameters.AddWithValue("@AreaID", areaId);
                    cmd.Parameters.AddWithValue("@WeekID", weekId);
                    cmd.Parameters.AddWithValue("@Score", score);

                    cmd.ExecuteNonQuery();
                }
            }

            // Refresh the GridView to show the new data
            GridView1.DataBind();
        }
        else
        {
            Response.Write("Session expired. Please log in again.");
        }

    }
}