using TimeEntries.Web.Models;

namespace TimeEntries.Web.Services;

public interface ITimeEntryApiClient
{
    Task<IReadOnlyList<TimeEntry>> GetTimeEntriesAsync();
}
