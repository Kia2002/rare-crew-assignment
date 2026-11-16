using TimeEntries.Web.Models;

namespace TimeEntries.Web.Services;

public static class EmployeeHoursCalculator
{
    /// <summary>
    /// Sums the hours worked per employee, ordered from most to least.
    /// Skips deleted entries, entries without an employee name and entries that end before they start.
    /// </summary>
    public static IReadOnlyList<EmployeeHours> Calculate(IEnumerable<TimeEntry> entries)
    {
        return entries
            .Where(entry => entry.DeletedOn is null)
            .Where(entry => !string.IsNullOrWhiteSpace(entry.EmployeeName))
            .Where(entry => entry.EndTimeUtc > entry.StartTimeUtc)
            .GroupBy(entry => entry.EmployeeName!)
            .Select(group => new EmployeeHours
            {
                Name = group.Key,
                TotalHours = group.Sum(entry => (entry.EndTimeUtc - entry.StartTimeUtc).TotalHours)
            })
            .OrderByDescending(employee => employee.TotalHours)
            .ToList();
    }
}
