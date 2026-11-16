using System.Text.Json.Serialization;

namespace TimeEntries.Web.Models;

public class TimeEntry
{
    public string? EmployeeName { get; set; }

    // The API spells this field "StarTimeUtc", so we map it to a correctly named property.
    [JsonPropertyName("StarTimeUtc")]
    public DateTime StartTimeUtc { get; set; }

    public DateTime EndTimeUtc { get; set; }

    /// <summary>
    /// Set when the entry was soft-deleted. Deleted entries should not be counted.
    /// </summary>
    public DateTime? DeletedOn { get; set; }
}
