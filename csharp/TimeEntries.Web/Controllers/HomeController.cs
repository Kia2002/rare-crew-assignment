using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using TimeEntries.Web.Models;
using TimeEntries.Web.Services;

namespace TimeEntries.Web.Controllers;

public class HomeController : Controller
{
    private readonly ITimeEntryApiClient _apiClient;

    public HomeController(ITimeEntryApiClient apiClient)
    {
        _apiClient = apiClient;
    }

    public async Task<IActionResult> Index()
    {
        var employees = await GetEmployeeHoursAsync();

        return View(employees);
    }

    public async Task<IActionResult> Chart(bool download = false)
    {
        var employees = await GetEmployeeHoursAsync();
        var png = HoursPieChart.CreatePng(employees);

        return download
            ? File(png, "image/png", "employee-hours.png")
            : File(png, "image/png");
    }

    private async Task<IReadOnlyList<EmployeeHours>> GetEmployeeHoursAsync()
    {
        var entries = await _apiClient.GetTimeEntriesAsync();

        return EmployeeHoursCalculator.Calculate(entries);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
