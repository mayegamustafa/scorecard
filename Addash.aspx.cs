using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;

public partial class Afdash : System.Web.UI.Page
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
                // Display the most recent term
                DisplayRecentTermName(departmentId);

                DisplayRecentYearName(departmentId);


                // Fetch and display recent week's best and worst performers
                BindBestPerformingSchoolsRecentWeek(departmentId);
                BindWorstPerformingSchoolsRecentWeek(departmentId);
                BindBestPerformingAreasRecentWeek(departmentId);
                BindWorstPerformingAreasRecentWeek(departmentId);

                // Display the most recent week's name dynamically
                DisplayRecentWeekName(departmentId);

                // Fetch and display the total number of areas
                BindTotalAreas(departmentId);
                // Fetch and display the total number of areas
                BindTotalUsers(departmentId);

                // Fetch and display the total number of areas
                BindTotalSchools(departmentId);


            }
            else
            {
                // Handle the case where departmentId is not set in session or invalid
                // For example, you could display an error message or redirect the user
                // Response.Redirect("ErrorPage.aspx");
            }
        }
    }







    private void BindBestPerformingSchools(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingSchoolsByTerm", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestPerformers.DataSource = dt;
                GridViewBestPerformers.DataBind();
            }
        }
    }


    /*private void BindBestPerformingSchools(int departmentId)
     {
         string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
         string query = @"
             SELECT TOP 2 school_id, school_name, total_score
             FROM BestPerformingSchools
             WHERE department_id = @departmentId
             ORDER BY total_score DESC";

         using (SqlConnection conn = new SqlConnection(connectionString))
         {
             using (SqlCommand cmd = new SqlCommand(query, conn))
             {
                 cmd.Parameters.AddWithValue("@departmentId", departmentId);

                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 DataTable dt = new DataTable();
                 da.Fill(dt);

                 GridViewBestPerformers.DataSource = dt;
                 GridViewBestPerformers.DataBind();
             }
         }
     } */

    // New Methods for Recent Week Data
    private void BindWorstPerformingSchools(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("PoorPerformingSchoolsByTerm", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstPerformers.DataSource = dt;
                GridViewWorstPerformers.DataBind();
            }
        }
    }

    /* private void BindWorstPerformingSchools(int departmentId)
     {
         string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
         string query = @"
             SELECT TOP 2 school_id, school_name, total_score
             FROM WorstPerformingSchools
             WHERE department_id = @departmentId
             ORDER BY total_score ASC";

         using (SqlConnection conn = new SqlConnection(connectionString))
         {
             using (SqlCommand cmd = new SqlCommand(query, conn))
             {
                 cmd.Parameters.AddWithValue("@departmentId", departmentId);

                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 DataTable dt = new DataTable();
                 da.Fill(dt);

                 GridViewWorstPerformers.DataSource = dt;
                 GridViewWorstPerformers.DataBind();
             }
         }
     } */


    private void BindBestPerformingAreas(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingAreasByTerm", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestAreas.DataSource = dt;
                GridViewBestAreas.DataBind();
            }
        }
    }

    /*  private void BindBestPerformingAreas(int departmentId)
      {
          string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
          string query = @"
              SELECT TOP 2 a.area_id, a.area_name, SUM(sc.score) AS total_score
              FROM scores sc
              INNER JOIN areas a ON sc.area_id = a.area_id
              WHERE sc.department_id = @departmentId
              GROUP BY a.area_id, a.area_name
              ORDER BY total_score DESC";

          using (SqlConnection conn = new SqlConnection(connectionString))
          {
              using (SqlCommand cmd = new SqlCommand(query, conn))
              {
                  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                  SqlDataAdapter da = new SqlDataAdapter(cmd);
                  DataTable dt = new DataTable();
                  da.Fill(dt);

                  GridViewBestAreas.DataSource = dt;
                  GridViewBestAreas.DataBind();
              }
          }
      } */

    private void BindWorstPerformingAreas(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("PoorPerformingAreasByTerm", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstAreas.DataSource = dt;
                GridViewWorstAreas.DataBind();
            }
        }
    }
    /* private void BindWorstPerformingAreas(int departmentId)
     {
         string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
         string query = @"
             SELECT TOP 2 a.area_id, a.area_name, SUM(sc.score) AS total_score
             FROM scores sc
             INNER JOIN areas a ON sc.area_id = a.area_id
             WHERE sc.department_id = @departmentId
             GROUP BY a.area_id, a.area_name
             ORDER BY total_score ASC";

         using (SqlConnection conn = new SqlConnection(connectionString))
         {
             using (SqlCommand cmd = new SqlCommand(query, conn))
             {
                 cmd.Parameters.AddWithValue("@departmentId", departmentId);

                 SqlDataAdapter da = new SqlDataAdapter(cmd);
                 DataTable dt = new DataTable();
                 da.Fill(dt);

                 GridViewWorstAreas.DataSource = dt;
                 GridViewWorstAreas.DataBind();
             }
         }
     } */


    // New Methods for Recent Week Data
    private void BindBestPerformingSchoolsRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("BestPerformingSchools1", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewBestPerformersRecentWeek.DataSource = dt;
                GridViewBestPerformersRecentWeek.DataBind();
            }
        }
    }

    /*private void BindWorstPerformingSchoolsRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT TOP 2 school_id, school_name, total_score
        FROM fBestPerformingSchools
        WHERE department_id = @departmentId
        ORDER BY CAST(REPLACE(total_score, '%', '') AS DECIMAL(5,2)) ASC"; // Ascending order for worst performers

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstPerformersRecentWeek.DataSource = dt;
                GridViewWorstPerformersRecentWeek.DataBind();
            }
        }
    } */

    private void BindWorstPerformingSchoolsRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand("ffWorstPerformingSchools1", conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                // If you need to pass the departmentId, you can modify the stored procedure to accept a parameter
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstPerformersRecentWeek.DataSource = dt;
                GridViewWorstPerformersRecentWeek.DataBind();
            }
        }
    }

    /*  private void BindWorstPerformingSchoolsRecentWeek(int departmentId)
      {
          string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
          string query = @"
              SELECT TOP 2 school_id, school_name, total_score
              FROM fWorstPerformingSchools11
              WHERE department_id = @departmentId
              ORDER BY CAST(REPLACE(total_score, '%', '') AS DECIMAL(5,2)) ASC"; // Ascending order for worst performers"; 
        //  ORDER BY total_score ASC

          using (SqlConnection conn = new SqlConnection(connectionString))
          {
              using (SqlCommand cmd = new SqlCommand(query, conn))
              {
                  cmd.Parameters.AddWithValue("@departmentId", departmentId);

                  SqlDataAdapter da = new SqlDataAdapter(cmd);
                  DataTable dt = new DataTable();
                  da.Fill(dt);

                  GridViewWorstPerformersRecentWeek.DataSource = dt;
                  GridViewWorstPerformersRecentWeek.DataBind();
              }
          }
      } */

    private void BindBestPerformingAreasRecentWeek(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = "fBestPerformingAreas";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

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
        string query = "fWorstPerformingAreas";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@departmentId", departmentId);

                SqlDataAdapter da = new SqlDataAdapter(cmd);
                DataTable dt = new DataTable();
                da.Fill(dt);

                GridViewWorstAreasRecentWeek.DataSource = dt;
                GridViewWorstAreasRecentWeek.DataBind();
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


    private void DisplayRecentWeekName(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        // Query to fetch the most recent week name based on department data
        string query = @"
        SELECT TOP 1 w.week_name
        FROM weeks w
        INNER JOIN scores s ON w.week_id = s.week_id
        WHERE s.department_id = @departmentId
        ORDER BY s.week_id DESC";

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@departmentId", departmentId);
                conn.Open();
                object result = cmd.ExecuteScalar();  // Fetch the week name

                if (result != null)
                {
                    // Dynamically display the recent week name in the label
                    lblRecentWeek.Text = result.ToString();
                    lblRecentWeek2.Text = result.ToString();
                    lblRecentWeek3.Text = result.ToString();
                    lblRecentWeek4.Text = result.ToString();
                }
            }
        }
    }

    /* private void DisplayRecentWeekName()
     {
         string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
         string query = "SELECT TOP 1 week_name FROM weeks ORDER BY week_id DESC";

         using (SqlConnection conn = new SqlConnection(connectionString))
         {
             using (SqlCommand cmd = new SqlCommand(query, conn))
             {
                 conn.Open();
                 object result = cmd.ExecuteScalar();
                 if (result != null)
                 {
                     lblRecentWeek.Text = result.ToString();
                     lblRecentWeek2.Text = result.ToString();
                     lblRecentWeek3.Text = result.ToString();
                     lblRecentWeek4.Text = result.ToString();
                 }
             }
         }
     }  */

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

    private void BindTotalAreas(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalAreas
        FROM areas
        WHERE department_id = @departmentId";

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
                    lblTotalAreas.Text = result.ToString();
                }
                else
                {
                    lblTotalAreas.Text = "0";  // Default to 0 if no result is found
                }
            }
        }
    }



    private void BindTotalUsers(int departmentId)
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = @"
        SELECT COUNT(*) AS TotalUsers
        FROM Users
        WHERE DepartmentID = @departmentId";

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
                    lblTotalUser.Text = result.ToString();
                }
                else
                {
                    lblTotalUser.Text = "0";  // Default to 0 if no result is found
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



}








