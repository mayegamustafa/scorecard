using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;

public partial class Gwek : System.Web.UI.Page
{
    private string departmentPrefix;

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadYears();
            LoadTerms();
            LoadDepartments();
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

    private void LoadDepartments()
    {
        DepartmentDropdown.Items.Clear();
        DepartmentDropdown.Items.Add(new ListItem("Finance", "finance"));
        DepartmentDropdown.Items.Add(new ListItem("Academic", "academic"));
        DepartmentDropdown.Items.Add(new ListItem("Theology", "theology"));
        DepartmentDropdown.Items.Add(new ListItem("Administration", "administration"));
        DepartmentDropdown.Items.Add(new ListItem("Quality Assurance", "quality assurance"));
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

            if (dt.Rows.Count == 0)
            {
                DropDownList2.Items.Add(new ListItem("No weeks available", ""));
            }
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

    protected void DepartmentDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        departmentPrefix = GetDepartmentPrefix(DepartmentDropdown.SelectedValue);
        LoadGridViewsAndChartData();
    }

    protected void DropDownList2_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadGridViewsAndChartData();
    }

    private void LoadGridViewsAndChartData()
    {
        if (DropDownList2.SelectedItem != null && !string.IsNullOrEmpty(DropDownList2.SelectedValue))
        {
            string selectedWeek = DropDownList2.SelectedItem.Text;
            string[] parts = selectedWeek.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length >= 2)
            {
                int selectedWeekNumber;
                bool isNumber = int.TryParse(parts[1], out selectedWeekNumber);

                if (isNumber)
                {
                    string selectedYear = YearDropdown.SelectedValue;
                    string selectedTerm = "Term_" + TermDropdown.SelectedValue;
                    int previousWeekNumber = selectedWeekNumber - 1;

                    lblPivotedHeader.Text = "Week Scores - " + selectedWeek;
                    lblVarianceHeader.Text = (previousWeekNumber < 1 ? "Variance Data - Week 1" : "Variance Data - Week " + previousWeekNumber + " vs Week " + selectedWeekNumber);
                    lblVarianceHeader1.Text = "Variance Data - Week " + previousWeekNumber + " vs Week " + selectedWeekNumber;
                    lblRankHeader.Text = "Ranking Data - Week " + selectedWeekNumber;
                    lblRankHeader1.Text = "Ranking Data - Week " + selectedWeekNumber;

                    LoadPivotedView(selectedYear, selectedTerm, selectedWeekNumber);
                    LoadVarianceView(selectedYear, selectedTerm, previousWeekNumber, selectedWeekNumber);
                    LoadRankView(selectedYear, selectedTerm, selectedWeekNumber);
                    LoadChartData(); // Refresh chart data after loading views
                }
                else
                {
                    lblErrorMessage.Text = "Selected week number is invalid.";
                    lblErrorMessage.Visible = true;
                }
            }
            else
            {
                lblErrorMessage.Text = "Selected week format is incorrect.";
                lblErrorMessage.Visible = true;
            }
        }
        else
        {
            ClearDataDisplays();
        }
    }

    private void LoadPivotedView(string year, string term, int weekNumber)
    {
        string viewName = departmentPrefix + "week" + weekNumber + "_pivoted_view_" + term + "_" + year;
        BindGridView(gvWeekData, viewName);
    }

    private void LoadVarianceView(string year, string term, int previousWeekNumber, int currentWeekNumber)
    {
        string viewName = departmentPrefix + "variance_view_week" + currentWeekNumber + "_vs_week" + previousWeekNumber + "_" + term + "_" + year;
        BindGridView(gvVarianceData, viewName);
    }

    private void LoadRankView(string year, string term, int weekNumber)
    {
        string viewName = departmentPrefix + "summary_ranks_view_week" + weekNumber + "_" + term + "_" + year;
        BindGridView(gvRankData, viewName);
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

                    if (dt.Rows.Count > 0)
                    {
                        gridView.DataSource = dt;
                        gridView.DataBind();
                    }
                    else
                    {
                        lblErrorMessage.Text = "No data available for the view '" + viewName + "'.";
                        lblErrorMessage.Visible = true;
                    }
                }
                else
                {
                    lblErrorMessage.Text = "The view '" + viewName + "' does not exist.";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                lblErrorMessage.Text = "Error binding view: " + ex.Message;
                lblErrorMessage.Visible = true;
            }
        }
    }

    /*  private void LoadChartData()
      {
          if (DropDownList2.SelectedItem == null || string.IsNullOrEmpty(DropDownList2.SelectedValue))
          {
              lblErrorMessage.Text = "Please select a week to view the chart data.";
              lblErrorMessage.Visible = true;
              return;
          }

          string selectedWeek = DropDownList2.SelectedItem.Text;
          string[] parts = selectedWeek.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

          if (parts.Length >= 2)
          {
              int selectedWeekNumber;
              bool isNumber = int.TryParse(parts[1], out selectedWeekNumber);

              if (isNumber)
              {
                  string selectedYear = YearDropdown.SelectedValue;
                  string selectedTerm = "Term_" + TermDropdown.SelectedValue;

                  string rankViewName = departmentPrefix + "summary_ranks_view_week" + selectedWeekNumber + "_" + selectedTerm + "_" + selectedYear;
                  string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

                  using (SqlConnection conn = new SqlConnection(connString))
                  {
                      try
                      {
                          conn.Open();
                          string query = "SELECT school, score FROM " + rankViewName;

                          SqlCommand cmd = new SqlCommand(query, conn);
                          SqlDataAdapter da = new SqlDataAdapter(cmd);
                          DataTable dt = new DataTable();
                          da.Fill(dt);

                          if (dt.Rows.Count > 0)
                          {
                              SchoolChart.Series[0].Points.Clear();
                              foreach (DataRow row in dt.Rows)
                              {
                                  string schoolName = row["school_name"].ToString();
                                  double score = Convert.ToDouble(row["Average_Score"]);
                                  SchoolChart.Series[0].Points.AddXY(schoolName, score);
                              }

                              SchoolChart.Visible = true;
                          }
                          else
                          {
                              lblErrorMessage.Text = "No data available to display on the chart.";
                              lblErrorMessage.Visible = true;
                          }
                      }
                      catch (Exception ex)
                      {
                          lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                          lblErrorMessage.Visible = true;
                      }
                  }
              }
              else
              {
                  lblErrorMessage.Text = "Selected week number is invalid.";
                  lblErrorMessage.Visible = true;
              }
          }
          else
          {
              lblErrorMessage.Text = "Selected week format is incorrect.";
              lblErrorMessage.Visible = true;
          }
      }

      */

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
        string userDepartment = DepartmentDropdown.SelectedValue; // Get department from dropdown
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
                    SchoolChart.Series["Series1"].XValueMember = "school_name";
                    SchoolChart.Series["Series1"].YValueMembers = "Average_Score";
                    SchoolChart.DataSource = dt;

                    // Customizations
                    SchoolChart.Series["Series1"].IsValueShownAsLabel = true;
                    SchoolChart.ChartAreas[0].AxisX.LabelStyle.Angle = -45;
                    SchoolChart.ChartAreas[0].AxisX.Interval = 1;
                    SchoolChart.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                    SchoolChart.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                    SchoolChart.ChartAreas[0].AxisX.LabelStyle.Enabled = true;

                    // Assign different colors to each school
                    string[] colors = { "#FF5733", "#33FF57", "#3357FF", "#FF33A8", "#33FFA5", "#FFA533", "#5D33FF", "#33FFF7", "#FFC733", "#57FF33", "#FF3357", "#5733FF" };
                    int colorIndex = 0;

                    foreach (DataPoint point in SchoolChart.Series["Series1"].Points)
                    {
                        point.Color = System.Drawing.ColorTranslator.FromHtml(colors[colorIndex % colors.Length]);
                        colorIndex++;
                    }

                    SchoolChart.DataBind();
                }
                else
                {
                    lblErrorMessage.Text = "No data available for the selected week.";
                    lblErrorMessage.Visible = true;
                }
            }
            catch (Exception ex)
            {
                // Remove the LogError call
                lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                lblErrorMessage.Visible = true;
            }

        }
    }


    private void ClearDataDisplays()
    {
        gvWeekData.DataSource = null;
        gvWeekData.DataBind();
        gvVarianceData.DataSource = null;
        gvVarianceData.DataBind();
        gvRankData.DataSource = null;
        gvRankData.DataBind();
        SchoolChart.Visible = false;
        lblErrorMessage.Visible = false;
    }

    private string GetDepartmentPrefix(string departmentName)
    {
        switch (departmentName.ToLower())
        {
            case "finance":
                return "f";
            case "academic":
                return "a";
            case "theology":
                return "t";
            case "administration":
                return "adm";
            case "quality assurance":
                return "qa";
            default:
                return string.Empty;
        }
    }
}
