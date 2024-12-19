using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI.WebControls;


public partial class Qanaly : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            UpdateGridViews();
        }
    }

    protected void YearDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViews();
    }

    protected void TermDropdown_SelectedIndexChanged(object sender, EventArgs e)
    {
        UpdateGridViews();
    }

    private void UpdateGridViews()
    {
        string year = YearDropdown.SelectedValue;
        string term = TermDropdown.SelectedValue;

        string selectCommand = "SELECT * FROM [dbo].[Qsummary_progressive_scores_" + term + "_" + year + "]";
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;

        try
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                SqlDataAdapter adapter = new SqlDataAdapter(selectCommand, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                GridViewProgressiveScores.Columns.Clear();
                bool hasValidColumns = false;

                foreach (DataColumn col in dt.Columns)
                {
                    bool hasValidData = dt.AsEnumerable().Any(row => row[col] != DBNull.Value && !string.IsNullOrWhiteSpace(row[col].ToString()));

                    if (hasValidData)
                    {
                        BoundField field = new BoundField
                        {
                            DataField = col.ColumnName,
                            HeaderText = col.ColumnName
                        };
                        GridViewProgressiveScores.Columns.Add(field);
                        hasValidColumns = true;
                    }
                }

                if (!hasValidColumns)
                {
                    GridViewProgressiveScores.DataSource = null;
                    GridViewProgressiveScores.DataBind();
                }
                else
                {
                    GridViewProgressiveScores.DataSource = dt;
                    GridViewProgressiveScores.DataBind();
                }

                LoadChartData(dt);
            }
        }
        catch (Exception ex)
        {
            // Handle exception
            Console.WriteLine(ex.Message); // Log error
        }
    }

    private void LoadChartData(DataTable dt)
    {
        // Clear previous series and chart areas
        ScoresChart.Series.Clear();
        ScoresChart.ChartAreas.Clear();

        ChartArea chartArea = new ChartArea();
        chartArea.AxisX.Title = "Schools";
        chartArea.AxisY.Title = "Scores";
        chartArea.AxisY.LabelStyle.Format = "{0}%";
        chartArea.AxisX.LabelStyle.Angle = -45;
        chartArea.AxisX.Interval = 1;
        ScoresChart.ChartAreas.Add(chartArea);

        if (dt.Rows.Count > 0)
        {
            List<Color> weekColors = GenerateMixedColors(dt.Columns.Count / 2);
            int colorIndex = 0;

            for (int i = 1; i < dt.Columns.Count - 1; i += 2)
            {
                string weekName = dt.Columns[i].ColumnName;

                Series weekSeries = new Series(weekName)
                {
                    ChartType = SeriesChartType.Column,
                    Color = weekColors[colorIndex % weekColors.Count],
                    BorderWidth = 2,
                    IsValueShownAsLabel = true,
                    LabelBackColor = Color.White
                };
                ScoresChart.Series.Add(weekSeries);

                foreach (DataRow row in dt.Rows)
                {
                    string schoolName = row["school_name"].ToString();
                    string scoreString = row[i].ToString().Replace("%", "");

                    decimal weekScore;
                    if (decimal.TryParse(scoreString, out weekScore))
                    {
                        weekSeries.Points.AddXY(schoolName, weekScore);
                    }
                }

                colorIndex++;
            }

            Series movingAvgSeries = new Series("Moving Avg")
            {
                ChartType = SeriesChartType.Line,
                Color = Color.Red,
                BorderWidth = 3,
                MarkerStyle = MarkerStyle.Circle,
                IsValueShownAsLabel = true
            };
            ScoresChart.Series.Add(movingAvgSeries);

            foreach (DataRow row in dt.Rows)
            {
                string schoolName = row["school_name"].ToString();
                string movingAvgString = row["Moving_Avg"].ToString().Replace("%", "");

                decimal movingAvg;
                if (decimal.TryParse(movingAvgString, out movingAvg))
                {
                    movingAvgSeries.Points.AddXY(schoolName, movingAvg);
                }
            }

            ScoresChart.Legends.Add(new Legend("Legend")
            {
                Title = "Week Performance & Moving Avg",
                Docking = Docking.Bottom
            });

            ScoresChart.DataBind();
        }
        else
        {
            // Handle case where no data is returned
            Console.WriteLine("No data found.");
        }
    }

    private List<Color> GenerateMixedColors(int numberOfColors)
    {
        List<Color> colors = new List<Color>();
        double hueStep = 360.0 / numberOfColors;

        for (int i = 0; i < numberOfColors; i++)
        {
            int hue = (int)(i * hueStep);
            Color color = ColorFromHSV(hue, 0.7, 0.9);
            colors.Add(color);
        }

        return ShuffleColors(colors);
    }

    private List<Color> ShuffleColors(List<Color> colors)
    {
        Random random = new Random();
        for (int i = colors.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            Color temp = colors[i];
            colors[i] = colors[j];
            colors[j] = temp;
        }
        return colors;
    }

    private Color ColorFromHSV(double hue, double saturation, double value)
    {
        int hi = Convert.ToInt32(Math.Floor(hue / 60)) % 6;
        double f = hue / 60 - Math.Floor(hue / 60);

        value = value * 255;
        int v = Convert.ToInt32(value);
        int p = Convert.ToInt32(value * (1 - saturation));
        int q = Convert.ToInt32(value * (1 - f * saturation));
        int t = Convert.ToInt32(value * (1 - (1 - f) * saturation));

        if (hi == 0)
            return Color.FromArgb(255, v, t, p);
        else if (hi == 1)
            return Color.FromArgb(255, q, v, p);
        else if (hi == 2)
            return Color.FromArgb(255, p, v, t);
        else if (hi == 3)
            return Color.FromArgb(255, p, q, v);
        else if (hi == 4)
            return Color.FromArgb(255, t, p, v);
        else
            return Color.FromArgb(255, v, p, q);
    }
}
