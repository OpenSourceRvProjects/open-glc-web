import { Component, HostListener, Input, OnInit } from '@angular/core';
import { ChartType } from 'angular-google-charts'; // Import the ChartType enum
import { EventMealsService } from '../../Services/event-meals.service';


@Component({
  selector: 'glc-bar-chart',
  templateUrl: './bar-chart.component.html',
})
export class BarChartComponent implements OnInit {
  constructor(private eventService: EventMealsService) { }

  userData: any = {};

  @Input()
  serverData: number[] = [];

  showGraph: boolean = false;
  dataForGraph: [number, number][] = [];
  ngOnInit(): void {
    this.serverData.forEach((fe, index) => this.dataForGraph.push([index + 1, fe]));
    this.showGraph = true;
  }

  //npm install angular-google - charts@12
  public type: ChartType = ChartType.ColumnChart;
  columnNames = ['Medición', ''];
  title = 'Niveles de azucar';
  options = {
    curveType: 'function',
    legend: { position: 'bottom' },
    trendlines: {
      0: {
        type: 'linear',
        color: 'red',
        lineWidth: 2,
        labelInLegend: 'Tendencia lineal',
        showEquation: true,
        visibleInLegend: true
      },
    },
    animation: {
      startup: true,           // Enables animation when the chart starts to load
      duration: 2000,          // Duration of the animation in milliseconds
      easing: 'out',           // Easing function for the animation
    }
  };
  width: number = window.innerWidth * 0.9; // 80% of the window width
  height: number = 400; // You can also dynamically adjust this

  // Dynamically update the height based on window size
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.width = event.target.innerWidth * 0.9;  // 80% of the viewport width
    this.height = event.target.innerHeight * 0.6; // Adjust height based on screen
  }

}
