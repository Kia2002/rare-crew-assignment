import { httpResource } from '@angular/common/http';
import { Component, computed } from '@angular/core';
import { calculateEmployeeHours } from './employee-hours';
import { EmployeeTable } from './employee-table/employee-table';
import { TimeEntry } from './time-entry';

@Component({
  imports: [EmployeeTable],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly timeEntries = httpResource<TimeEntry[]>(() => '/api/gettimeentries');

  protected readonly employees = computed(() =>
    calculateEmployeeHours(this.timeEntries.value() ?? []),
  );
}
