using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Text;
using System.IO; // Add this line
using System.Drawing; // Add this using directive
using System.Web.UI.WebControls;
using System.Web.UI.DataVisualization.Charting;
using System.Web.UI;
using System.Linq;



public partial class GMdash : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Retrieve the department ID from session
            int departmentId;
            if (Session["DepartmentId"] != null && int.TryParse(Session["DepartmentId"].ToString(), out departmentId))
            {
                // Fetch and display the department name based on the logged-in department ID
                DisplayDepartmentName(departmentId);

                // Fetch and display the best two and worst two performers
                BindBestPerformingSchools(departmentId);
                BindWorstPerformingSchools(departmentId);
                BindBestPerformingAreas(departmentId);
                BindWorstPerformingAreas(departmentId);

                // Fetch and display recent week's best and worst performers
                /*   BindBestPerformingSchoolsRecentWeek(departmentId);
                   BindWorstPerformingSchoolsRecentWeek(departmentId);
                   BindBestPerformingAreasRecentWeek(departmentId);
                   BindWorstPerformingAreasRecentWeek(departmentId);

                   // Display the most recent week's name dynamically
                   DisplayRecentWeekName(); */


                // Fetch and display recent week's best and worst performers
                BindBestPerformingSchoolsRecentWeek(departmentId);
                BindWorstPerformingSchoolsRecentWeek(departmentId);
                BindBestPerformingAreasRecentWeek(departmentId);
                BindWorstPerformingAreasRecentWeek(departmentId);

                // Display the most recent week's name dynamically
                DisplayRecentWeekName(departmentId);

                // Display the most recent term
                DisplayRecentTermName(departmentId);

                DisplayRecentYearName(departmentId);

                // Bind improvement and decline data
                BindImprovementDeclineData(departmentId);

                 // Fetch and display the total number of areas
                BindTotalAreasf(departmentId);
                BindTotalAreasA(departmentId);
                BindTotalAreasT(departmentId);
                BindTotalAreasQ(departmentId);
                BindTotalAreasAd(departmentId);

                BindTotalusers(departmentId);

                // BindSchoolDepartmentScoresTrend(); // NEW METHOD CALL HERE

                BindChart();

                // Fetch and display the total number of areas
                BindTotalSchools(departmentId);

                LoadChartData();  // Load chart data after updating views
                LoadDptChartData();
            }
            else
            {
                // Handle the case where departmentId is not set in session or invalid
                // For example, you could display an error message or redirect the user
                // Response.Redirect("ErrorPage.aspx");
            }
        }
    }

    private void BindChart()
    {
        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("SchoolTrendChart31", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                // Clear previous series
                SchoolTrendChart.Series.Clear();

                // Get distinct department names
                var departments = dt.AsEnumerable()
                    .Select(row => row.Field<string>("department_name"))
                    .Distinct()
                    .ToList();

                // Create a series for each department
                foreach (var department in departments)
                {
                    var series = new Series(department)
                    {
                        // Change ChartType to Column for vertical bars
                        ChartType = SeriesChartType.Column,
                        YValueType = ChartValueType.Double // Average scores are numeric
                    };

                    // Filter data for the current department
                    var departmentData = dt.AsEnumerable()
                        .Where(row => row.Field<string>("department_name") == department)
                        .ToList();

                    // Add points to the series for each school
                    foreach (var row in departmentData)
                    {
                        string schoolName = row.Field<string>("school_name");

                        // Use safe type conversion for avg_score
                        var avgScoreObj = row["avg_score"];
                        double avgScore;

                        if (avgScoreObj != DBNull.Value && double.TryParse(avgScoreObj.ToString(), out avgScore))
                        {
                            series.Points.AddXY(schoolName, avgScore);
                        }
                        else
                        {
                            series.Points.AddXY(schoolName, 0); // Default to 0 if the conversion fails
                        }
                    }

                    // Assign a unique color to each department
                    series.Color = GetColorForDepartment(department); // Custom method to get a color for each department
                    SchoolTrendChart.Series.Add(series);
                }

                // Configure chart area
                SchoolTrendChart.ChartAreas[0].AxisX.Title = "School Name";
                SchoolTrendChart.ChartAreas[0].AxisY.Title = "Average Score";
                SchoolTrendChart.ChartAreas[0].AxisX.Interval = 1;
                SchoolTrendChart.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate X-axis labels if needed

                // Add chart title
                SchoolTrendChart.Titles.Clear();
                SchoolTrendChart.Titles.Add("Average Scores by School per Department");
                SchoolTrendChart.Titles[0].Font = new System.Drawing.Font("Arial", 14, System.Drawing.FontStyle.Bold);


                // Add data labels
                foreach (var series in SchoolTrendChart.Series)
                {
                    series.IsValueShownAsLabel = true;
                    series.LabelForeColor = Color.Black;
                }
            }
        }
    }


    // This method returns a unique color for each department
    private Color GetColorForDepartment(string department)
    {
        switch (department)
        {
            case "Finance":
                return Color.Blue;
            case "Quality Assurance":
                return Color.Orange;
            case "Theology":
                return Color.Purple;
            case "Academic":
                return Color.Red;
            default:
                return Color.Gray; // Default color if no match
        }
    }



    // Custom method to get a color for each department
    /*  private Color GetColorForDepartment(string department)
      {
          switch (department)
          {
              case "Finance":
                  return Color.Blue;
              case "Academic":
                  return Color.Green;
              case "Theology":
                  return Color.Red;
              case "Quality Assurance":
                  return Color.Orange;
              default:
                  return Color.Gray;
          }
      } */


    /* private void BindSchoolDepartmentScoresTrend()
     {
         string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

         using (SqlConnection conn = new SqlConnection(connectionString))
         {
             using (SqlCommand cmd = new SqlCommand("SchoolTrendChartPivot", conn))
             {
                 cmd.CommandType = CommandType.StoredProcedure;
                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 DataTable dt = new DataTable();
                 da.Fill(dt);

                 // Debug: Print column names and data
                 foreach (DataColumn column in dt.Columns)
                 {
                     Response.Write(column.ColumnName + "<br>");
                 }

                 foreach (DataRow row in dt.Rows)
                 {
                     foreach (var item in row.ItemArray)
                     {
                         Response.Write(item.ToString() + " ");
                     }
                     Response.Write("<br>");
                 }
             }

             }
         } */




    private void BindBestPerformingSchools(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingSchoolsByTermGM", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
               // cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestPerformers.DataSource = dt;
                GridViewBestPerformers.DataBind();
            }
        }
    }


 

    // New Methods for Recent Week Data
    private void BindWorstPerformingSchools(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("PoorPerformingSchoolsByTermGM", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
              //  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstPerformers.DataSource = dt;
                GridViewWorstPerformers.DataBind();
            }
        }
    }

    

    private void BindBestPerformingAreas(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingAreasByTermGM", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                //cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestAreas.DataSource = dt;
                GridViewBestAreas.DataBind();
            }
        }
    }

   

    private void BindWorstPerformingAreas(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("PoorPerformingAreasByTermGM", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                //  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstAreas.DataSource = dt;
                GridViewWorstAreas.DataBind();
            }
        }
    }

    // New Methods for Recent Week Data
    private void BindBestPerformingSchoolsRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingSchools2", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
              //  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestPerformersRecentWeek.DataSource = dt;
                GridViewBestPerformersRecentWeek.DataBind();
               
            }
        }
    }

   
    private void BindWorstPerformingSchoolsRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("ffWorstPerformingSchools2", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
              //  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstPerformersRecentWeek.DataSource = dt;
                GridViewWorstPerformersRecentWeek.DataBind();
            }
        }
    }

    

    private void BindBestPerformingAreasRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = "fBestPerformingAreas1";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
               // cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestAreasRecentWeek.DataSource = dt;
                GridViewBestAreasRecentWeek.DataBind();

               
            }
        }
    }


    private void BindWorstPerformingAreasRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = "fWorstPerformingAreas1";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
              //  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstAreasRecentWeek.DataSource = dt;
                GridViewWorstAreasRecentWeek.DataBind();
            }
        }
    }


    private void DisplayRecentWeekName(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        // Query to fetch the most recent week name based on department data
        string query = @"
        SELECT TOP 1 w.week_name
        FROM weeks w
        INNER JOIN scores s ON w.week_id = s.week_id
      
        ORDER BY s.week_id DESC";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
              //  cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the week name

                if (result != null)
                {
                    // Dynamically display the recent week name in the label
                    lblRecentWeek.Text = result.ToString();
                    lblRecentWeek2.Text = result.ToString();
                    lblRecentWeek3.Text = result.ToString();
                    lblRecentWeek4.Text = result.ToString();
                    lblRecentWeek5.Text = result.ToString();
                    lblRecentWeek6.Text = result.ToString();
                    lblRecentWeek7.Text = result.ToString();
                }
            }
        }
    }

    private void DisplayRecentTermName(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        // Query to fetch the most recent week name based on department data
      /*  string query = @"
        SELECT TOP 1 w.week_name
        FROM weeks w
        INNER JOIN scores s ON w.week_id = s.week_id
      
        ORDER BY s.week_id DESC";  */

        string query = @"
        SELECT TOP 1
         w.term
    FROM scores s
    INNER JOIN weeks w ON s.week_id = w.week_id
   
    ORDER BY  w.term DESC";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                //  cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the week name

                if (result != null)
                {
                    // Dynamically display the recent week name in the label
                    lblRecentWeek11.Text = result.ToString();
                    lblRecentWeek12.Text = result.ToString();
                    lblRecentWeek13.Text = result.ToString();
                    lblRecentWeek14.Text = result.ToString();

                }
            }
        }
    }

    private void DisplayRecentYearName(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        // Query to fetch the most recent week name based on department data
        /*  string query = @"
          SELECT TOP 1 w.week_name
          FROM weeks w
          INNER JOIN scores s ON w.week_id = s.week_id

          ORDER BY s.week_id DESC";  */

        string query = @"
        SELECT TOP 1
         w.year
    FROM scores s
    INNER JOIN weeks w ON s.week_id = w.week_id
   
    ORDER BY  w.year DESC";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                //  cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the week name

                if (result != null)
                {
                    // Dynamically display the recent week name in the label
                    lblRecentWeek111.Text = result.ToString();
                    lblRecentWeek112.Text = result.ToString();
                    lblRecentWeek113.Text = result.ToString();
                    lblRecentWeek114.Text = result.ToString();

                }
            }
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

    private void BindImprovementDeclineData(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            SqlCommand cmd = new SqlCommand("GetImprovementDecline1", conn);
            cmd.CommandType = CommandType.StoredProcedure;

            // If your stored procedure does not require parameters, remove or comment out this line
            // cmd.Parameters.AddWithValue("@DepartmentId", departmentId);

            SqlDataAdapter da = new SqlDataAdapter(cmd);
            DataTable dt = new DataTable();
            da.Fill(dt);

            // Log the actual columns in the DataTable
            StringBuilder columnNames = new StringBuilder();
            foreach (DataColumn column in dt.Columns)
            {
                if (columnNames.Length > 0)
                    columnNames.Append(", ");
                columnNames.Append(column.ColumnName);
            }

           

            GridViewImprovementDecline.DataSource = dt;
            GridViewImprovementDecline.DataBind();
        }
    }

    


    private void BindTotalAreasf(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = 1";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalAreasf.Text = result.ToString();
                }
                else
                {
                    lblTotalAreasf.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void BindTotalAreasA(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = 2";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalAreasA.Text = result.ToString();
                }
                else
                {
                    lblTotalAreasA.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void BindTotalAreasT(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = 3";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalAreasT.Text = result.ToString();
                }
                else
                {
                    lblTotalAreasT.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void BindTotalAreasQ(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = 4";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalAreasQ.Text = result.ToString();
                }
                else
                {
                    lblTotalAreasQ.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void BindTotalAreasAd(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = 5";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalAreasAd.Text = result.ToString();
                }
                else
                {
                    lblTotalAreasAd.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void BindTotalSchools(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalSchools
        FROM schools
      ";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalschools.Text = result.ToString();
                }
                else
                {
                    lblTotalschools.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }
    

         private void BindTotalusers(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalUsers
        FROM Users
      ";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                // cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the total number of areas

                if (result != null)
                {
                    // Display the total number of areas in a label or other control
                    lblTotalUsers.Text = result.ToString();
                }
                else
                {
                    lblTotalUsers.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }

    private void LoadChartData()
    {
        

       // string rankViewName = "summary_ranks_view_week6_term_2_2024";
      //  string query = "SELECT school_name, Average_Score FROM " + rankViewName;

        string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingSchools3", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                // SqlCommand cmd = new SqlCommand(query, conn);
                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    DataTable dt = new DataTable();
                    da.Fill(dt);
                try
                {

                   

                    // Ensure data is valid
                    if (dt.Rows.Count > 0)
                    {
                        // Process data
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["total_score"] != DBNull.Value)
                            {
                                row["total_score"] = Convert.ToDouble(row["total_score"].ToString().Replace("%", ""));
                            }
                        }

                        dt = dt.AsEnumerable()
                            .Where(row => !row.IsNull("school_name") && !row.IsNull("total_score"))
                            .CopyToDataTable();

                        // Bind data to chart
                        SchoolScoresChart.Series["Series1"].XValueMember = "school_name";
                        SchoolScoresChart.Series["Series1"].YValueMembers = "total_score";
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
                    LogError("Error loading chart data for week: " + "\nException: " + ex.ToString());
                    lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                    lblErrorMessage.Visible = true;
                }


            }
        }
    }

    private void LoadDptChartData()
    {
        string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingDpts", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                try
                {
                    // Ensure data is valid
                    if (dt.Rows.Count > 0)
                    {
                        // Process data
                        foreach (DataRow row in dt.Rows)
                        {
                            if (row["total_score"] != DBNull.Value)
                            {
                                row["total_score"] = Convert.ToDouble(row["total_score"].ToString().Replace("%", ""));
                            }
                        }

                        dt = dt.AsEnumerable()
                            .Where(row => !row.IsNull("department_name") && !row.IsNull("total_score"))
                            .CopyToDataTable();

                        // Clear previous series
                        SchoolScoresChart1.Series.Clear();

                        // Create a single series for all data
                        var series = new Series("Department Scores")
                        {
                            ChartType = SeriesChartType.Bar, // Horizontal bars
                            YValueType = ChartValueType.Double // Total scores are numeric
                        };

                        // Assign points to the series and set colors for each department
                        foreach (DataRow row in dt.Rows)
                        {
                            string departmentName = row.Field<string>("department_name");

                            // Use safe type conversion for total_score
                            var totalScoreObj = row["total_score"];
                            double totalScore;

                            if (totalScoreObj != DBNull.Value && double.TryParse(totalScoreObj.ToString(), out totalScore))
                            {
                                // Add a point for each department
                                series.Points.AddXY(departmentName, totalScore);

                                // Assign the unique color for the department
                                series.Points[series.Points.Count - 1].Color = GetColorForDepartment(departmentName);
                            }
                        }

                        // Add series to the chart
                        SchoolScoresChart1.Series.Add(series);

                        // Configure chart area
                        SchoolScoresChart1.ChartAreas[0].AxisX.Title = "Department Name";
                        SchoolScoresChart1.ChartAreas[0].AxisY.Title = "Total Score";

                        // Adjust X-axis labels
                        SchoolScoresChart1.ChartAreas[0].AxisX.Interval = 1; // Ensure labels for each department
                        SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate labels to avoid overlap
                        SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.IsEndLabelVisible = true; // Show end labels if they are cut off
                        SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                        SchoolScoresChart1.ChartAreas[0].AxisX.IsLabelAutoFit = true; // Adjust label fitting

                        // Ensure the X-axis is not too cramped
                        SchoolScoresChart1.ChartAreas[0].Position = new ElementPosition(0, 0, 100, 75); // Adjust chart area position to make room for labels

                        // Add chart title
                        SchoolScoresChart1.Titles.Clear();
                        SchoolScoresChart1.Titles.Add("Department Scores");

                        // Add data labels to show the values
                        series.IsValueShownAsLabel = true;
                        series.LabelForeColor = Color.Black;
                        series["PointWidth"] = "0.8"; // Adjust width of bars

                        SchoolScoresChart1.DataBind();
                    }
                    else
                    {
                        lblErrorMessage.Text = "No data available for the selected week.";
                        lblErrorMessage.Visible = true;
                    }
                }
                catch (Exception ex)
                {
                    LogError("Error loading chart data: " + "\nException: " + ex.ToString());
                    lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                    lblErrorMessage.Visible = true;
                }
            }
        }
    }




    /* private void LoadDptChartData()
     {
         string connString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
         using (SqlConnection conn = new SqlConnection(connString))
         {
             using (SqlCommand cmd = new SqlCommand("BestPerformingDpts", conn))
             {
                 cmd.CommandType = CommandType.StoredProcedure;

                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 DataTable dt = new DataTable();
                 da.Fill(dt);

                 try
                 {
                     // Ensure data is valid
                     if (dt.Rows.Count > 0)
                     {
                         // Process data
                         foreach (DataRow row in dt.Rows)
                         {
                             if (row["total_score"] != DBNull.Value)
                             {
                                 row["total_score"] = Convert.ToDouble(row["total_score"].ToString().Replace("%", ""));
                             }
                         }

                         dt = dt.AsEnumerable()
                             .Where(row => !row.IsNull("department_name") && !row.IsNull("total_score"))
                             .CopyToDataTable();

                         // Clear previous series
                         SchoolScoresChart1.Series.Clear();

                         // Create a single series for all data
                         var series = new Series("Department Scores")
                         {
                             ChartType = SeriesChartType.Bar, // Horizontal bars
                             XValueMember = "department_name",
                             YValueMembers = "total_score",
                             IsValueShownAsLabel = true
                         };

                         // Assign different colors to each department
                         string[] colors = { "#FF5733", "#33FF57", "#3357FF", "#FF33A8", "#33FFA5", "#FFA533", "#5D33FF", "#33FFF7", "#FFC733", "#57FF33", "#FF3357", "#5733FF" };
                         int colorIndex = 0;

                         // Bind data to chart
                         SchoolScoresChart1.Series.Add(series);
                         SchoolScoresChart1.DataSource = dt;

                         // Customize chart
                         SchoolScoresChart1.Series["Department Scores"].ChartType = SeriesChartType.Bar;
                         SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.Angle = -45; // Rotate X-axis labels if needed
                         SchoolScoresChart1.ChartAreas[0].AxisX.Interval = 1; // Ensure labels for each department
                         SchoolScoresChart1.ChartAreas[0].AxisX.IsLabelAutoFit = true;
                         SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.Font = new System.Drawing.Font("Arial", 8, System.Drawing.FontStyle.Regular);
                         SchoolScoresChart1.ChartAreas[0].AxisX.LabelStyle.Enabled = true;
                         SchoolScoresChart1.ChartAreas[0].AxisY.Title = "Total Score";
                         SchoolScoresChart1.ChartAreas[0].AxisX.Title = "Department Name";

                         foreach (DataPoint point in SchoolScoresChart1.Series["Department Scores"].Points)
                         {
                             point.Color = System.Drawing.ColorTranslator.FromHtml(colors[colorIndex % colors.Length]);
                             colorIndex++;
                         }

                         SchoolScoresChart1.DataBind();
                     }
                     else
                     {
                         lblErrorMessage.Text = "No data available for the selected week.";
                         lblErrorMessage.Visible = true;
                     }
                 }
                 catch (Exception ex)
                 {
                     LogError("Error loading chart data: " + "\nException: " + ex.ToString());
                     lblErrorMessage.Text = "Error loading chart data: " + ex.Message;
                     lblErrorMessage.Visible = true;
                 }
             }
         }
     }
 */


    private void LogError(string message)
    {
        // Implement logging mechanism, e.g., log to a file or database
        System.Diagnostics.Debug.WriteLine(message);
    }

}








