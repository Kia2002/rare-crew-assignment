namespace TimeEntries.Web.Models;

public class EmployeeHours
{
    public const double LowHoursThreshold = 100;

    public string Name { get; set; } = string.Empty;

    public double TotalHours { get; set; }

    public bool HasLowHours => TotalHours < LowHoursThreshold;
}
