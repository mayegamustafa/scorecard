using System;
using System.Data;
using System.Configuration;
using System.Web.UI.WebControls;
using System.Web.UI;
using System.Data.SqlClient;

public partial class Gmanage : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            BindDepartments();
            BindUsers(); // Bind users data to GridView
        }
    }

    private void BindDepartments()
    {
        DataTable dt = GetDepartments();
        ddlDepartment.DataSource = dt;
        ddlDepartment.DataTextField = "department_name";
        ddlDepartment.DataValueField = "department_id";
        ddlDepartment.DataBind();

        // Bind departments to modal dropdown
        ddlModalDepartment.DataSource = dt;
        ddlModalDepartment.DataTextField = "department_name";
        ddlModalDepartment.DataValueField = "department_id";
        ddlModalDepartment.DataBind();
    }

    protected void btnAddUser_Click(object sender, EventArgs e)
    {
        try
        {
            AddUser(txtUsername.Text, txtPassword.Text, Convert.ToInt32(ddlDepartment.SelectedValue));
            ShowNotification("User added successfully!", false);

            // Re-bind GridView to show the new user
            BindUsers();
        }
        catch (Exception ex)
        {
            ShowNotification("Error: " + ex.Message, true);
        }
    }

    protected void GridView1_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName == "EditUser")
        {
            int userID;
            if (int.TryParse(e.CommandArgument.ToString(), out userID))
            {
                var user = GetUserById(userID);

                if (user != null)
                {
                    // Populate modal fields
                    txtModalUsername.Text = user.Username;
                    txtModalPassword.Text = user.Password;
                    ddlModalDepartment.SelectedValue = user.DepartmentID.ToString();

                    // Store the selected user ID in a hidden field
                    hfSelectedUserID.Value = userID.ToString();

                    // Show the modal
                    ScriptManager.RegisterStartupScript(this, this.GetType(), "OpenModal", "openModal();", true);
                }
                else
                {
                    ShowModalError("User not found.");
                }
            }
            else
            {
                ShowModalError("Invalid User ID.");
            }
        }
        else if (e.CommandName == "DeleteUser")
        {
            int userID;
            if (int.TryParse(e.CommandArgument.ToString(), out userID))
            {
                DeleteUser(userID);

                // Re-bind GridView to reflect changes
                BindUsers();
            }
            else
            {
                ShowNotification("Invalid User ID.", true);
            }
        }
    }

    protected void btnModalUpdateUser_Click(object sender, EventArgs e)
    {
        try
        {
            int userID = Convert.ToInt32(hfSelectedUserID.Value);
            UpdateUser(userID, txtModalUsername.Text, txtModalPassword.Text, Convert.ToInt32(ddlModalDepartment.SelectedValue));

            ShowNotification("User updated successfully!", false);

            // Re-bind GridView to show updated user
            BindUsers();
        }
        catch (Exception ex)
        {
            ShowModalError("Error: " + ex.Message);
        }
    }

    protected void btnCloseModal_Click(object sender, EventArgs e)
    {
        // Close modal using client-side script
    }

    protected void GridView1_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        GridView1.PageIndex = e.NewPageIndex;
        BindUsers();
    }

    private void BindUsers()
    {
        DataTable dt = GetUsers();
        GridView1.DataSource = dt;
        GridView1.DataBind();
    }

    private DataTable GetDepartments()
    {
        DataTable dt = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            string query = "SELECT department_id, department_name FROM Departments";
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }

    private DataTable GetUsers()
    {
        DataTable dt = new DataTable();
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            // Ensure column names match the original
            string query = @"
            SELECT u.UserID, u.Username, u.Password, d.department_name AS DepartmentName
            FROM Users u
            JOIN Departments d ON u.DepartmentID = d.department_id";
            using (var cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    dt.Load(reader);
                }
            }
        }
        return dt;
    }


    private dynamic GetUserById(int userId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            string query = "SELECT Username, Password, DepartmentID FROM Users WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new
                        {
                            Username = reader["Username"].ToString(),
                            Password = reader["Password"].ToString(),
                            DepartmentID = Convert.ToInt32(reader["DepartmentID"])
                        };
                    }
                }
            }
        }
        return null;
    }


    private void AddUser(string username, string password, int departmentId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            string query = "INSERT INTO Users (Username, Password, DepartmentID) VALUES (@Username, @Password, @DepartmentID)";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    private void UpdateUser(int userId, string username, string password, int departmentId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            string query = "UPDATE Users SET Username = @Username, Password = @Password, DepartmentID = @DepartmentID WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                cmd.Parameters.AddWithValue("@Username", username);
                cmd.Parameters.AddWithValue("@Password", password);
                cmd.Parameters.AddWithValue("@DepartmentID", departmentId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    private void DeleteUser(int userId)
    {
        string connStr = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        using (var conn = new SqlConnection(connStr))
        {
            string query = "DELETE FROM Users WHERE UserID = @UserID";
            using (var cmd = new SqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@UserID", userId);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }

    private void ShowNotification(string message, bool isError)
    {
        lblNotification.Text = message;
        lblNotification.ForeColor = isError ? System.Drawing.Color.Red : System.Drawing.Color.Green;
        lblNotification.Style["display"] = "block";
    }


    private void ShowModalError(string message)
    {
        lblModalError.Text = message;
        lblModalError.Style["display"] = "block";
    }
}
