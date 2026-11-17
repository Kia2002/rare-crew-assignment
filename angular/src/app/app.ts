import { httpResource } from '@angular/common/http';
import { Component } from '@angular/core';
import { TimeEntry } from './time-entry';

@Component({
  imports: [],
  selector: 'app-root',
  styleUrl: './app.css',
  templateUrl: './app.html',
})
export class App {
  protected readonly timeEntries = httpResource<TimeEntry[]>(() => '/api/gettimeentries');
}
