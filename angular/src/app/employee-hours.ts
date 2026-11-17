import { TimeEntry } from './time-entry';

export const LOW_HOURS_THRESHOLD = 100;

export interface EmployeeHours {
  name: string;
  totalHours: number;
  hasLowHours: boolean;
}

/**
 * Sums the hours worked per employee, ordered from most to least.
 * Skips deleted entries, entries without an employee name and entries that end before they start.
 */
export function calculateEmployeeHours(entries: TimeEntry[]): EmployeeHours[] {
  const hoursByName = new Map<string, number>();

  for (const entry of entries) {
    const name = entry.EmployeeName?.trim();
    const hours = hoursBetween(entry.StarTimeUtc, entry.EndTimeUtc);

    if (entry.DeletedOn || !name || hours <= 0) {
      continue;
    }

    hoursByName.set(name, (hoursByName.get(name) ?? 0) + hours);
  }

  return Array.from(hoursByName, ([name, totalHours]) => ({
    name,
    totalHours,
    hasLowHours: totalHours < LOW_HOURS_THRESHOLD,
  })).sort((a, b) => b.totalHours - a.totalHours);
}

function hoursBetween(startUtc: string, endUtc: string): number {
  const millisecondsPerHour = 60 * 60 * 1000;
  return (parseUtc(endUtc) - parseUtc(startUtc)) / millisecondsPerHour;
}

// The API sends UTC times without a "Z" suffix. Without it, JavaScript would read them
// as local time, and durations around daylight saving changes would be off by an hour.
function parseUtc(value: string): number {
  return Date.parse(`${value}Z`);
}
