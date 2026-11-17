import { DecimalPipe } from '@angular/common';
import { Component, input } from '@angular/core';
import { EmployeeHours } from '../employee-hours';

@Component({
  imports: [DecimalPipe],
  selector: 'app-employee-table',
  styleUrl: './employee-table.css',
  templateUrl: './employee-table.html',
})
export class EmployeeTable {
  readonly employees = input.required<EmployeeHours[]>();
}
