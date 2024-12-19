using System;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web;

public partial class Awek : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            // Load years and terms into dropdowns
            LoadYears();
            LoadTerms();
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

        string pivotedViewName = prefix + "week" + selectedWeekNumber + "_pivoted_view_" + selectedTerm + "_" + selectedYear;
        string varianceViewName = previousWeekNumber < 1 ? null : prefix + "variance_view_week" + selectedWeekNumber + "_vs_week" + previousWeekNumber + "_" + selectedTerm + "_" + selectedYear;
        string rankViewName = prefix + "summary_ranks_view_week" + selectedWeekNumber + "_" + selectedTerm + "_" + selectedYear;

        BindGridView(gvWeekData, pivotedViewName);
        BindGridView(gvVarianceData, varianceViewName);
        BindGridView(gvRankData, rankViewName);
    }

    private string GetLoggedInUserDepartment()
    {
        // Replace with the actual logic to retrieve the department for the logged-in user
        // This could be from session, database, or any authentication context
        object department = HttpContext.Current.Session["UserDepartment"];

        if (department != null)
        {
            return department.ToString();
        }
        else
        {
            return ""; // Return an empty string if the department is not found
        }
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

         //   default:
              //  return "";
            default: return "A"; // Default case if no matching department is found
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
                    string query = "SELECT * FROM " + viewName;
                    try
                    {
                        SqlDataAdapter da = new SqlDataAdapter(query, conn);
                        DataTable dt = new DataTable();
                        da.Fill(dt);

                        gridView.DataSource = dt;
                        gridView.DataBind();
                    }
                    catch (Exception ex)
                    {
                        LogError("Error executing query: " + query + "\nException: " + ex.ToString());
                        lblErrorMessage.Text = "Error fetching data: " + ex.Message;
                        lblErrorMessage.Visible = true;
                    }
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

    private void LogError(string message)
    {
        string logPath = Server.MapPath("~/App_Data/ErrorLog.txt");
        string appDataDirectory = Server.MapPath("~/App_Data");
        if (!System.IO.Directory.Exists(appDataDirectory))
        {
            System.IO.Directory.CreateDirectory(appDataDirectory);
        }
        System.IO.File.AppendAllText(logPath, DateTime.Now.ToString() + ": " + message + Environment.NewLine);
    }
}
