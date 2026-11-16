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
        var entries = await _apiClient.GetTimeEntriesAsync();
        var employees = EmployeeHoursCalculator.Calculate(entries);

        return View(employees);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
