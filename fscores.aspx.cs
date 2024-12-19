using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;

using System.Data;
using System.Data.SqlClient;
using System.Web.UI.DataVisualization.Charting;


public partial class fscores : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Retrieve the department name from session
            string departmentName = Session["DepartmentName"] as string;

            if (!string.IsNullOrEmpty(departmentName))
            {
                // Bind data if department name is set
                BindAllWeekComparisons(departmentName);
                PopulateWeekDropdowns();
            }
            else
            {
                // Handle the case where departmentName is not set
                lblErrorMessage.Text = "Department is not set. Please log in.";
                lblErrorMessage.Visible = true;
                // Alternatively, you can disable further actions or redirect to login
            }
        }
    }

    private void BindAllWeekComparisons(string departmentName)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
            SELECT * 
            FROM week_comparison 
            WHERE department_name = @departmentName
            AND Week1 IS NOT NULL AND Week2 IS NOT NULL";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentName", departmentName);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewAllWeeksComparison.DataSource = dt;
                GridViewAllWeeksComparison.DataBind();
            }
        }
    }

    private void PopulateWeekDropdowns()
    {
        // Assuming you have the code to populate the week dropdowns here
        ddlWeek1.Items.Insert(0, new ListItem("Select Week 1", "0"));
        ddlWeek2.Items.Insert(0, new ListItem("Select Week 2", "0"));
    }


    
        protected void btnCompareWeeks_Click(object sender, EventArgs e)
{
    // Get selected weeks from dropdowns
    string week1 = ddlWeek1.SelectedValue;
    string week2 = ddlWeek2.SelectedValue;

    if (week1 != "0" && week2 != "0")
    {
        CompareWeeks(week1, week2);
    }
    else
    {
        lblErrorMessage.Text = "Please select both weeks to compare.";
        lblErrorMessage.Visible = true;
    }
}

private void CompareWeeks(string week1, string week2)
{
    string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
    string departmentName = Session["DepartmentName"] as string;

    string query = @"
        SELECT * 
        FROM week_comparison 
        WHERE department_name = @departmentName
        AND (Week1 = @week1 OR Week2 = @week2)";

    using (SqlConnection conn = new SqlConnection(connectionString))
    {
        using (SqlCommand cmd = new SqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@departmentName", departmentName);
            cmd.Parameters.AddWithValue("@week1", week1);
            cmd.Parameters.AddWithValue("@week2", week2);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            GridViewWeekComparison.DataSource = dt;
            GridViewWeekComparison.DataBind();
        }
    }
}

    
}
