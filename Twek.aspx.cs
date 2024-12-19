using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI;

public partial class wek : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadYears();
            LoadTerms();
            LoadWeeks();
            LoadChartData();  // Load chart data on initial load
        }
    }

    private void LoadYears()
    {
        YearDropdown.Items.Clear();
        YearDropdown.Items.Add(new ListItem("2024", "2024"));
        YearDropdown.Items.Add(new ListItem("2025", "2025"));
        YearDropdown.Items.Add(new ListItem("2026", "2026"));
    }

    private void LoadTerms()
    {
        TermDropdown.Items.Clear();
        TermDropdown.Items.Add(new ListItem("Term 1", "1"));
        TermDropdown.Items.Add(new ListItem("Term 2", "2"));
        TermDropdown.Items.Add(new ListItem("Term 3", "3"));
    }

    private void LoadWeeks()
    {
        string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            string selectedYear = YearDropdown.SelectedValue;
            string selectedTerm = TermDropdown.SelectedValue;

            string query = "SELECT week_id, week_name FROM weeks WHERE year = @Year AND term = @Term ORDER BY week_id";
            SqlCommand cmd = new SqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@Year", selectedYear);
            cmd.Parameters.AddWithValue("@Term", selectedTerm);
            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);
            DropDownList2.DataSource = dt;
            DropDownList2.DataTextField = "week_name";
            DropDownList2.DataValueField = "week_id";
            DropDownList2.DataBind();
        }
    }

    protected void YearDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWeeks();
    }

    protected void TermDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadWeeks();
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridViewsAndChartData();
    }

    private void LoadGridViewsAndChartData()
    {
        string selectedWeek = DropDownList2.SelectedItem.Text;
        string[] parts = selectedWeek.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        int selectedWeekNumber = int.Parse(parts[1]);
        string selectedYear = YearDropdown.SelectedValue;
        string selectedTerm = "Term_" + TermDropdown.SelectedValue;

        int previousWeekNumber = selectedWeekNumber - 1;
        string userDepartment = GetLoggedInUserDepartment();
        string prefix = GetDepartmentPrefix(userDepartment);

        lblPivotedHeader.InnerText = "Week Scores - " + selectedWeek;
        lblVarianceHeader.InnerText = previousWeekNumber < 1 ? "Variance Data - Week 1" : "Variance Data - Week " + previousWeekNumber + " vs Week " + selectedWeekNumber;
        lblVarianceHeader1.InnerText = "Variance Data - " + selectedWeek;
        lblRankHeader.InnerText = "Rank Data - " + selectedWeek;
        lblRankHeader1.InnerText = "Percentage Chart - " + selectedWeek;

        string pivotedViewName = prefix + "week" + selectedWeekNumber + "_pivoted_view_" + selectedTerm + "_" + selectedYear;
        string varianceViewName = previousWeekNumber < 1 ? null : prefix + "variance_view_week" + selectedWeekNumber + "_vs_week" + previousWeekNumber + "_" + selectedTerm + "_" + selectedYear;
        string rankViewName = prefix + "summary_ranks_view_week" + selectedWeekNumber + "_" + selectedTerm + "_" + selectedYear;

        BindGridView(gvWeekData, pivotedViewName);
        BindGridView(gvVarianceData, varianceViewName);
        BindGridView(gvRankData, rankViewName);

        LoadChartData();  // Load chart data after updating views
    }

    private string GetLoggedInUserDepartment()
    {
        if (Session["UserDepartment"] != null)
        {
            return Session["UserDepartment"].ToString();
        }
        return string.Empty;
    }

    private string GetDepartmentPrefix(string department)
    {
        switch (department.ToLower())
        {
            case "finance": return "f";
            case "academic": return "A";
            case "theology": return "T";
            case "administration": return "Ad";
            case "quality assurance": return "Q";
            default: return "T";
        }
    }

    private void BindGridView(GridView gridView, string viewName)
    {
        if (string.IsNullOrEmpty(viewName))
        {
            gridView.DataSource = null;
            gridView.DataBind();
            return;
        }

        string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = "SELECT * FROM " + viewName;

        using (SqlConnection conn = new SqlConnection(connString))
        {
            try
            {
                conn.Open();
                string checkViewQuery = "SELECT COUNT(*) FROM INFORMATION_SCHEMA.VIEWS WHERE TABLE_NAME = @ViewName";
                SqlCommand checkCmd = new SqlCommand(checkViewQuery, conn);
                checkCmd.Parameters.AddWithValue("@ViewName", viewName);

                int viewExists = (int)checkCmd.ExecuteScalar();

                if (viewExists > 0)
                {
                    SqlDataAdapter da = new SqlDataAdapter(query, conn);
                    DataTable dt = new DataTable();
                    da.Fill(dt);

                    gridView.DataSource = dt;
                    gridView.DataBind();
                }
                else
                {
                    gridView.DataSource = null;
                    gridView.DataBind();
                }
            }
            catch (Exception ex)
            {
                LogError("Error in BindGridView method for view: " + viewName + "\nException: " + ex.ToString());
                lblErrorMessage.Text = "Error binding view: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }
    }

    private void LoadChartData()
    {
        if (DropDownList2.SelectedItem == null)
        {
            lblErrorMessage.Text = "Please select a week to view the chart data.";
            lblErrorMessage.Visible = true;
            return;
        }

        string selectedWeek = DropDownList2.SelectedItem.Text;
        string[] parts = selectedWeek.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

        if (parts.Length < 2)
        {
            lblErrorMessage.Text = "Selected week format is incorrect.";
            lblErrorMessage.Visible = true;
            return;
        }

        int selectedWeekNumber;
        if (!int.TryParse(parts[1], out selectedWeekNumber))
        {
            lblErrorMessage.Text = "Invalid week number format.";
            lblErrorMessage.Visible = true;
            return;
        }

        string selectedYear = YearDropdown.SelectedValue;
        string selectedTerm = "Term_" + TermDropdown.SelectedValue;
        string userDepartment = GetLoggedInUserDepartment();
        string prefix = GetDepartmentPrefix(userDepartment);

        string rankViewName = prefix + "summary_ranks_view_week" + selectedWeekNumber + "_" + selectedTerm + "_" + selectedYear;
        string query = "SELECT school_name, Average_Score FROM " + rankViewName;

        string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            try
            {
                SqlCommand cmd = new SqlCommand(query, conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Ensure data is valid
                if (dt.Rows.Count > 0)
                {
                    // Process data
                    foreach (DataRow row in dt.Rows)
                    {
                        if (row["Average_Score"] != DBNull.Value)
                        {
                            row["Average_Score"] = Convert.ToDouble(row["Average_Score"].ToString().Replace("%", ""));
                        }
                    }

                    dt = dt.AsEnumerable()
                        .Where(row => !row.IsNull("school_name") && !row.IsNull("Average_Score"))
                        .CopyToDataTable();

                    // Bind data to chart
                    SchoolScoresChart.Series["Series1"].XValueMember = "school_name";
                    SchoolScoresChart.Series["Series1"].YValueMembers = "Average_Score";
                    SchoolScoresChart.DataSource = dt;

                    // Customizations
                    SchoolScoresChart.Series["Series1"].IsValueShownAsLabel = true;
                    SchoolScoresChart.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                    SchoolScoresChart.ChartAreas[0].AxisX.Interval = 1;
                    SchoolScoresChart.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                    SchoolScoresChart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                    SchoolScoresChart.ChartAreas[0].AxisX.LabelStyle.Enabled = true;

                    // Assign different colors to each school
                    string[] colors = { "#FF5733", "#33FF57", "#3357FF", "#FF33A8", "#33FFA5", "#FFA533", "#5D33FF", "#33FFF7", "#FFC733", "#57FF33", "#FF3357", "#5733FF" };
                    int colorIndex = 0;

                    foreach (DataPoint point in SchoolScoresChart.Series["Series1"].Points)
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml(colors[colorIndex % colors.Length]);
                        colorIndex++;
                    }

                    SchoolScoresChart.DataBind();
                }
                else
                {
                    lblErrorMessage.Text = "No data available for the selected week.";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                LogError("Error loading chart data for week: " + selectedWeek + "\nException: " + ex.ToString());
                lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }
    }

    private void LogError(string message)
    {
        // Implement logging mechanism, e.g., log to a file or database
        System.Diagnostics.Debug.WriteLine(message);
    }
}
