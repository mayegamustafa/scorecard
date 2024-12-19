using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;
using System.Drawing;

public partial class fSchoolTrend : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadDropdowns();
        }
    }

    // Load dropdowns for Year, Term, and School
    private void LoadDropdowns()
    {
        LoadYearDropdown();
        LoadTermDropdown();
        LoadSchoolDropdown();
    }

    private void LoadYearDropdown()
    {
        YearDropdown.Items.Clear();
        // Add available years
        YearDropdown.Items.Add(new ListItem("2024", "2024"));
        YearDropdown.Items.Add(new ListItem("2025", "2025"));
        YearDropdown.Items.Add(new ListItem("2026", "2026"));
        YearDropdown.SelectedIndex = 0;
    }

    private void LoadTermDropdown()
    {
        TermDropdown.Items.Clear();
        // Add available terms
        TermDropdown.Items.Add(new ListItem("Term 1", "Term 1"));
        TermDropdown.Items.Add(new ListItem("Term 2", "Term 2"));
        TermDropdown.Items.Add(new ListItem("Term 3", "Term 3"));
        TermDropdown.SelectedIndex = 0;
    }

    private void LoadSchoolDropdown()
    {
        SchoolDropdown.Items.Clear();
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            string query = "SELECT school_id, school_name FROM schools"; // Adjust table names if needed
            SqlCommand cmd = new SqlCommand(query, conn);
            conn.Open();
            SqlDataReader reader = cmd.ExecuteReader();

            SchoolDropdown.Items.Add(new ListItem("Select School", "0"));
            while (reader.Read())
            {
                SchoolDropdown.Items.Add(new ListItem(reader["school_name"].ToString(), reader["school_id"].ToString()));
            }

            reader.Close();
        }

        SchoolDropdown.SelectedIndex = 0;
    }

    // Event handler for dropdown changes
    protected void Dropdowns_SelectedIndexChanged(object sender, EventArgs e)
    {
        LoadSchoolTrendData();
    }

    // Load school trend data for selected school, term, and year
    private void LoadSchoolTrendData()
    {
        string yearString = YearDropdown.SelectedValue;
        string termString = TermDropdown.SelectedValue;
        string schoolId = SchoolDropdown.SelectedValue;

        if (schoolId == "0")
        {
            lblMessage.Text = "Please select a school.";
            return;
        }

        int year;
        int term;

        if (!int.TryParse(yearString, out year))
        {
            lblMessage.Text = "Invalid year selected.";
            return;
        }

        if (termString.StartsWith("Term "))
        {
            if (!int.TryParse(termString.Replace("Term ", ""), out term))
            {
                lblMessage.Text = "Invalid term selected.";
                return;
            }
        }
        else
        {
            lblMessage.Text = "Invalid term format.";
            return;
        }

        int departmentId = GetLoggedInUserDepartmentId();

        string selectCommand = @"
    SELECT w.week_name, 
           ROUND(AVG(sc.score), 1) AS average_score
    FROM scores sc
    JOIN weeks w ON sc.week_id = w.week_id
    JOIN schools s ON sc.school_id = s.school_id
    WHERE s.school_id = @SchoolId 
    AND w.year = @Year 
    AND w.term = @Term
    AND sc.department_id = @DepartmentId
    GROUP BY w.week_name, w.week_number
    ORDER BY w.week_number";

        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand, conn);
                adapter.SelectCommand.Parameters.AddWithValue("@SchoolId", schoolId);
                adapter.SelectCommand.Parameters.AddWithValue("@Year", year);
                adapter.SelectCommand.Parameters.AddWithValue("@Term", term);
                adapter.SelectCommand.Parameters.AddWithValue("@DepartmentId", departmentId);

                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    lblMessage.Text = "No data found for the selected school.";
                    return;
                }

                GridViewProgressiveScores1.Columns.Clear();
                bool hasValidColumns = false;

                foreach (DataColumn col in dt.Columns)
                {
                    BoundField field = new BoundField
                    {
                        DataField = col.ColumnName,
                        HeaderText = col.ColumnName
                    };

                    if (col.ColumnName == "average_score")
                    {
                        field.DataFormatString = "{0:0.00}%";
                    }

                    GridViewProgressiveScores1.Columns.Add(field);
                    hasValidColumns = true;
                }

                if (hasValidColumns)
                {
                    GridViewProgressiveScores1.DataSource = dt;
                    GridViewProgressiveScores1.DataBind();
                    lblMessage.Text = "Data loaded successfully.";
                }
                else
                {
                    GridViewProgressiveScores1.DataSource = null;
                    GridViewProgressiveScores1.DataBind();
                    lblMessage.Text = "No valid columns found.";
                }

                LoadSchoolTrendChartData(dt);
            }
        }
        catch (Exception ex)
        {
            lblMessage.Text = "Error: " + ex.Message;
        }
    }

    private void LoadSchoolTrendChartData(DataTable dt)
    {
        ScoresChart.Series.Clear();
        ScoresChart.ChartAreas.Clear();

        ChartArea chartArea = new ChartArea
        {
            AxisX = { Title = "Weeks", Interval = 1, LabelStyle = { Angle = -45 } },
            AxisY = { Title = "Average Scores", LabelStyle = { Format = "{0}%" } }
        };
        ScoresChart.ChartAreas.Add(chartArea);

        if (dt.Rows.Count > 0)
        {
            Series trendSeries = new Series("Average Performance Trend")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Blue,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                IsValueShownAsLabel = true // Show values on the chart
            };
            ScoresChart.Series.Add(trendSeries);

            foreach (DataRow row in dt.Rows)
            {
                string weekName = row["week_name"].ToString();
                string averageScoreString = row["average_score"].ToString().Replace("%", "");

                decimal averageScore;
                if (decimal.TryParse(averageScoreString, out averageScore))
                {
                    trendSeries.Points.AddXY(weekName, averageScore);

                    // Highlight weak points, e.g., average scores below 50%
                    if (averageScore < 50)
                    {
                        trendSeries.Points[trendSeries.Points.Count - 1].Color = Color.Red; // Highlight weak points in red
                    }
                }
                else
                {
                    lblMessage.Text = "Error parsing average score.";
                }
            }

            ScoresChart.DataBind();
        }
        else
        {
            lblMessage.Text = "No data found for the selected school.";
        }
    }


    // Dummy method to simulate fetching logged-in user's department ID
    private int GetLoggedInUserDepartmentId()
    {
        // Implement this method to return the department ID of the logged-in user
        // For example, from session state or user profile
        // Example:
        // return Convert.ToInt32(Session["DepartmentId"]);
        return 1; // Replace with actual logic
    }
}
