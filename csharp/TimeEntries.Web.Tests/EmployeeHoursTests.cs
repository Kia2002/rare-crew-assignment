using TimeEntries.Web.Models;

namespace TimeEntries.Web.Tests;

public class EmployeeHoursTests
{
    [Theory]
    [InlineData(0, true)]
    [InlineData(99.9, true)]
    [InlineData(100, false)]
    [InlineData(150, false)]
    public void HasLowHours_IsTrueOnlyBelowThreshold(double totalHours, bool expected)
    {
        var employee = new EmployeeHours { Name = "Ana", TotalHours = totalHours };

        Assert.Equal(expected, employee.HasLowHours);
    }
}
