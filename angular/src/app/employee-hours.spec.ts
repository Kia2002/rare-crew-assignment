import { calculateEmployeeHours } from './employee-hours';
import { TimeEntry } from './time-entry';

const START = '2026-01-01T08:00:00';

function createEntry(
  name: string | null,
  hours: number,
  deletedOn: string | null = null,
): TimeEntry {
  const end = new Date(Date.parse(`${START}Z`) + hours * 60 * 60 * 1000);

  return {
    EmployeeName: name,
    StarTimeUtc: START,
    // The API format has no "Z" and no milliseconds.
    EndTimeUtc: end.toISOString().slice(0, 19),
    DeletedOn: deletedOn,
  };
}

describe('calculateEmployeeHours', () => {
  it('sums hours per employee', () => {
    const entries = [createEntry('Ana', 2), createEntry('Ana', 3), createEntry('Marko', 1)];

    const result = calculateEmployeeHours(entries);

    expect(result).toHaveLength(2);
    expect(result.find((employee) => employee.name === 'Ana')?.totalHours).toBe(5);
    expect(result.find((employee) => employee.name === 'Marko')?.totalHours).toBe(1);
  });

  it('orders employees by total hours, descending', () => {
    const entries = [createEntry('Ana', 1), createEntry('Marko', 3), createEntry('Jelena', 2)];

    const result = calculateEmployeeHours(entries);

    expect(result.map((employee) => employee.name)).toEqual(['Marko', 'Jelena', 'Ana']);
  });

  it('treats names with surrounding spaces as the same employee', () => {
    const entries = [createEntry('Ana', 2), createEntry(' Ana ', 3)];

    const result = calculateEmployeeHours(entries);

    expect(result).toEqual([{ name: 'Ana', totalHours: 5, hasLowHours: true }]);
  });

  it('skips deleted entries', () => {
    const entries = [createEntry('Ana', 2), createEntry('Ana', 5, START)];

    const result = calculateEmployeeHours(entries);

    expect(result).toEqual([{ name: 'Ana', totalHours: 2, hasLowHours: true }]);
  });

  it.each([null, '', '   '])('skips entries without an employee name (%j)', (name) => {
    const result = calculateEmployeeHours([createEntry(name, 2)]);

    expect(result).toEqual([]);
  });

  it('skips entries that end before they start', () => {
    const entries = [createEntry('Ana', 2), createEntry('Ana', -4)];

    const result = calculateEmployeeHours(entries);

    expect(result).toEqual([{ name: 'Ana', totalHours: 2, hasLowHours: true }]);
  });

  it.each([
    [1, true],
    [99.5, true],
    [100, false],
    [150, false],
  ])('marks %d hours as low: %s', (hours, expected) => {
    const [employee] = calculateEmployeeHours([createEntry('Ana', hours)]);

    expect(employee.hasLowHours).toBe(expected);
  });

  it('reads times as UTC, so daylight saving changes do not affect the duration', () => {
    // Clocks in most of Europe moved forward during this night.
    const entry: TimeEntry = {
      EmployeeName: 'Ana',
      StarTimeUtc: '2026-03-28T23:00:00',
      EndTimeUtc: '2026-03-29T03:00:00',
      DeletedOn: null,
    };

    const [employee] = calculateEmployeeHours([entry]);

    expect(employee.totalHours).toBe(4);
  });
});
