using ScottPlot;
using TimeEntries.Web.Models;

namespace TimeEntries.Web.Services;

public static class HoursPieChart
{
    private const int Width = 700;
    private const int Height = 450;

    /// <summary>
    /// Draws a pie chart of each employee's share of the total hours and returns it as PNG bytes.
    /// </summary>
    public static byte[] CreatePng(IReadOnlyList<EmployeeHours> employees)
    {
        var totalHours = employees.Sum(employee => employee.TotalHours);
        var palette = new ScottPlot.Palettes.Category10();

        var slices = employees
            .Select((employee, index) => new PieSlice
            {
                Value = employee.TotalHours,
                Label = $"{employee.TotalHours / totalHours * 100:0}%",
                LegendText = employee.Name,
                FillColor = palette.GetColor(index)
            })
            .ToList();

        var plot = new Plot();
        var pie = plot.Add.Pie(slices);
        pie.SliceLabelDistance = 0.7;

        plot.Axes.Frameless();
        plot.HideGrid();
        plot.ShowLegend(Edge.Right);

        return plot.GetImageBytes(Width, Height, ImageFormat.Png);
    }
}
