using TimeEntries.Web.Models;

namespace TimeEntries.Web.Services;

public class TimeEntryApiClient : ITimeEntryApiClient
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public TimeEntryApiClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _apiKey = configuration["TimeEntryApi:Key"]
            ?? throw new InvalidOperationException(
                "API key is missing. Set it with: dotnet user-secrets set \"TimeEntryApi:Key\" \"<your-key>\"");
    }

    public async Task<IReadOnlyList<TimeEntry>> GetTimeEntriesAsync()
    {
        var url = $"api/gettimeentries?code={Uri.EscapeDataString(_apiKey)}";
        var entries = await _httpClient.GetFromJsonAsync<List<TimeEntry>>(url);

        return entries ?? [];
    }
}
