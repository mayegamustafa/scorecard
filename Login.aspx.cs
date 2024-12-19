using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

using System.Data.SqlClient;
using System.Web;


public partial class Login : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void btnLogin_Click(object sender, EventArgs e)
    {
        string username = txtUsername.Text;
        string password = txtPassword.Text;

        string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            conn.Open();
            string query = "SELECT UserID, DepartmentID FROM Users WHERE Username = @Username AND Password = @Password";
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);

                SqlDataReader reader = cmd.ExecuteReader();
                if (reader.HasRows)
                {
                    reader.Read();
                    int userId = reader.GetInt32(0);
                    int departmentId = reader.GetInt32(1);

                    // Store user information in the session
                    Session["UserID"] = userId;
                    Session["DepartmentID"] = departmentId;

                    // Redirect to different pages based on department
                    switch (departmentId)
                    {
                        case 1: // Finance Department
                            Response.Redirect("fdash.aspx");
                            break;
                        case 2: // Academic Department
                            Response.Redirect("Adash.aspx"); 
                            break;
                        case 3: // Theology Department
                            Response.Redirect("Tdash.aspx");
                            break;
                        case 4: // QA Department
                            Response.Redirect("Qdash.aspx");
                            break;
                        case 5: // QA Department
                            Response.Redirect("Addash.aspx");
                            break;
                        case 6: // QA Department
                            Response.Redirect("GMdash.aspx");
                            break;
                        // Add more cases as per your departments
                        default:
                            Response.Redirect("login.aspx"); // Default page if no specific department is matched
                            break;
                    }
                }
                else
                {
                    // Invalid login
                    lblError.Text = "Invalid username or password.";
                }
            }
        }

    }
}