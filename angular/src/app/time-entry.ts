// Field names match the API's JSON exactly, including its "StarTimeUtc" typo.
// Dates arrive as ISO strings, not Date objects.
export interface TimeEntry {
  EmployeeName: string | null;
  StarTimeUtc: string;
  EndTimeUtc: string;
  DeletedOn: string | null;
}
