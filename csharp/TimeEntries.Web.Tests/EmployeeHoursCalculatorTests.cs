using TimeEntries.Web.Models;
using TimeEntries.Web.Services;

namespace TimeEntries.Web.Tests;

public class EmployeeHoursCalculatorTests
{
    private static readonly DateTime Start = new(2026, 1, 1, 8, 0, 0, DateTimeKind.Utc);

    [Fact]
    public void Calculate_SumsHoursPerEmployee()
    {
        var entries = new[]
        {
            CreateEntry("Ana", hours: 2),
            CreateEntry("Ana", hours: 3),
            CreateEntry("Marko", hours: 1)
        };

        var result = EmployeeHoursCalculator.Calculate(entries);

        Assert.Equal(2, result.Count);
        Assert.Equal(5, result.Single(employee => employee.Name == "Ana").TotalHours);
        Assert.Equal(1, result.Single(employee => employee.Name == "Marko").TotalHours);
    }

    [Fact]
    public void Calculate_OrdersEmployeesByTotalHoursDescending()
    {
        var entries = new[]
        {
            CreateEntry("Ana", hours: 1),
            CreateEntry("Marko", hours: 3),
            CreateEntry("Jelena", hours: 2)
        };

        var result = EmployeeHoursCalculator.Calculate(entries);

        Assert.Equal(new[] { "Marko", "Jelena", "Ana" }, result.Select(employee => employee.Name));
    }

    [Fact]
    public void Calculate_TreatsNamesWithSurroundingSpacesAsSameEmployee()
    {
        var entries = new[]
        {
            CreateEntry("Ana", hours: 2),
            CreateEntry(" Ana ", hours: 3)
        };

        var result = EmployeeHoursCalculator.Calculate(entries);

        var employee = Assert.Single(result);
        Assert.Equal("Ana", employee.Name);
        Assert.Equal(5, employee.TotalHours);
    }

    [Fact]
    public void Calculate_SkipsDeletedEntries()
    {
        var entries = new[]
        {
            CreateEntry("Ana", hours: 2),
            CreateEntry("Ana", hours: 5, deletedOn: Start)
        };

        var result = EmployeeHoursCalculator.Calculate(entries);

        var employee = Assert.Single(result);
        Assert.Equal(2, employee.TotalHours);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void Calculate_SkipsEntriesWithoutEmployeeName(string? name)
    {
        var entries = new[] { CreateEntry(name, hours: 2) };

        var result = EmployeeHoursCalculator.Calculate(entries);

        Assert.Empty(result);
    }

    [Fact]
    public void Calculate_SkipsEntriesThatEndBeforeTheyStart()
    {
        var entries = new[]
        {
            CreateEntry("Ana", hours: 2),
            CreateEntry("Ana", hours: -4)
        };

        var result = EmployeeHoursCalculator.Calculate(entries);

        var employee = Assert.Single(result);
        Assert.Equal(2, employee.TotalHours);
    }

    private static TimeEntry CreateEntry(string? name, double hours, DateTime? deletedOn = null)
    {
        return new TimeEntry
        {
            EmployeeName = name,
            StartTimeUtc = Start,
            EndTimeUtc = Start.AddHours(hours),
            DeletedOn = deletedOn
        };
    }
}
