import { Component, ElementRef, afterRenderEffect, input, viewChild } from '@angular/core';
import Chart from 'chart.js/auto';
import ChartDataLabels from 'chartjs-plugin-datalabels';
import { EmployeeHours } from '../employee-hours';

// Chart.js has only 7 default colors, so with more employees they repeat.
// These are the same 10 colors as the C# chart (ScottPlot Category10).
const SLICE_COLORS = [
  '#1f77b4',
  '#ff7f0e',
  '#2ca02c',
  '#d62728',
  '#9467bd',
  '#8c564b',
  '#e377c2',
  '#7f7f7f',
  '#bcbd22',
  '#17becf',
];

@Component({
  selector: 'app-employee-chart',
  styleUrl: './employee-chart.css',
  templateUrl: './employee-chart.html',
})
export class EmployeeChart {
  readonly employees = input.required<EmployeeHours[]>();

  private readonly canvas = viewChild.required<ElementRef<HTMLCanvasElement>>('canvas');

  constructor() {
    afterRenderEffect((onCleanup) => {
      const chart = this.drawChart();
      onCleanup(() => chart.destroy());
    });
  }

  private drawChart(): Chart {
    const employees = this.employees();
    const totalHours = employees.reduce((sum, employee) => sum + employee.totalHours, 0);

    const names = employees.map((employee) => employee.name);
    const hours = employees.map((employee) => employee.totalHours);

    return new Chart(this.canvas().nativeElement, {
      type: 'pie',
      plugins: [ChartDataLabels],
      data: { labels: names, datasets: [{ data: hours, backgroundColor: SLICE_COLORS }] },
      options: {
        aspectRatio: 1.5,
        plugins: {
          legend: { position: 'right' },
          datalabels: {
            color: 'black',
            formatter: (value: number) => `${Math.round((value / totalHours) * 100)}%`,
          },
        },
      },
    });
  }
}
