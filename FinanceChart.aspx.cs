using System;
using System.Web.UI;
using System.Web.UI.DataVisualization.Charting;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Drawing;
using System.Collections.Generic;

public partial class FinanceChart : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            LoadChartData();
        }
    }

    private void LoadChartData()
    {
        string connectionString = ConfigurationManager.ConnectionStrings["s_cardConnectionString"].ConnectionString;
        string query = "SELECT * FROM fsummary_progressive_scores_Term_2_2024";

        DataTable dt = new DataTable();

        using (SqlConnection conn = new SqlConnection(connectionString))
        {
            using (SqlCommand cmd = new SqlCommand(query, conn))
            {
                conn.Open();
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(dt);
            }
        }

        // Clear previous series and chart areas
        ScoresChart.Series.Clear();
        ScoresChart.ChartAreas.Clear();

        // Create and configure the chart area
        ChartArea chartArea = new ChartArea();
        chartArea.AxisX.Title = "Schools";
        chartArea.AxisY.Title = "Scores";
        chartArea.AxisY.LabelStyle.Format = "{0}%";
        chartArea.AxisX.LabelStyle.Angle = -45; // Angle labels for better readability
        chartArea.AxisX.Interval = 1; // Set interval for x-axis
        ScoresChart.ChartAreas.Add(chartArea);

        if (dt.Rows.Count > 0)
        {
            // Generate distinct colors for each week with mixed order for better visibility
            List<Color> weekColors = GenerateMixedColors(dt.Columns.Count / 2);

            int colorIndex = 0;

            // Loop through DataTable columns for weeks and add them as series to the chart
            for (int i = 1; i < dt.Columns.Count - 1; i += 2)
            {
                string weekName = dt.Columns[i].ColumnName;

                Series weekSeries = new Series(weekName)
                {
                    ChartType = SeriesChartType.Column,
                    Color = weekColors[colorIndex % weekColors.Count],
                    BorderWidth = 2,
                    IsValueShownAsLabel = true,
                    LabelBackColor = Color.White // Ensure labels are visible
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

            // Add Moving Average series
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

            // Add a legend to the chart
            ScoresChart.Legends.Add(new Legend("Legend")
            {
                Title = "Week Performance & Moving Avg",
                Docking = Docking.Bottom
            });

            // Bind data to the chart
            ScoresChart.DataBind();
        }
        else
        {
            // Handle case where no data is returned
            Console.WriteLine("No data found.");
        }
    }

    // Method to generate mixed colors for better chart visibility
    private List<Color> GenerateMixedColors(int numberOfColors)
    {
        List<Color> colors = new List<Color>();
        double hueStep = 360.0 / numberOfColors; // Angle step for hue values

        // Generate colors evenly distributed in HSV space
        for (int i = 0; i < numberOfColors; i++)
        {
            int hue = (int)(i * hueStep);
            Color color = ColorFromHSV(hue, 0.7, 0.9); // Adjust saturation and brightness for contrast
            colors.Add(color);
        }

        // Shuffle colors in a way to avoid similar colors being adjacent
        return ShuffleColors(colors);
    }

    // Helper method to shuffle colors strategically
    private List<Color> ShuffleColors(List<Color> colors)
    {
        Random random = new Random();
        for (int i = colors.Count - 1; i > 0; i--)
        {
            int j = random.Next(0, i + 1);
            // Swap colors[i] with colors[j]
            Color temp = colors[i];
            colors[i] = colors[j];
            colors[j] = temp;
        }
        return colors;
    }

    // Helper method to convert HSV values to Color
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
