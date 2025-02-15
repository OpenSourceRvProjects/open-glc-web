import { Component, HostListener, Input, OnInit } from '@angular/core';
import { ChartType } from 'angular-google-charts'; // Import the ChartType enum
import { EventMealsService } from '../../Services/event-meals.service';

@Component({
  selector: 'glc-chart',
  templateUrl: './chart.component.html',
})
export class ChartComponent implements OnInit {
  constructor(private eventService: EventMealsService) { }

  userData: any = {};
  @Input()
  serverData: number[] = [];

  @Input()
  chartType?: ChartType;

  showGraph: boolean = false;
  type: ChartType = ChartType.Calendar;

  columnNames: string[] = []
  options: any = {};
  width: number = window.innerWidth * 0.9; // 80% of the window width
  height: number = 400; // You can also dynamically adjust this


  title = 'Niveles de azucar';
  dataForGraph: [number, number][] = [];
  ngOnInit(): void {
    debugger;
    this.serverData.forEach((fe, index) => this.dataForGraph.push([index + 1, fe]));

    //npm install angular-google - charts@12

    //https://stackoverflow.com/questions/54496398/typescript-type-string-undefined-is-not-assignable-to-type-string
    this.type = this.chartType!;
    this.columnNames = ['Medición', 'Glucosa'];
    this.options = {
      curveType: 'function',
      legend: { position: 'bottom' },
      trendlines: {
        0: {
          //just the same as linear
          type: 'polynomial',
          degree: 1,
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
      },
      responsive: true,
      //width: 900,  // 600 pixels wide
      //height: 400, // 400 pixels tall
    };


    this.showGraph = true;
  }

  // Dynamically update the height based on window size
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.width = event.target.innerWidth * 0.9;  // 80% of the viewport width
    this.height = event.target.innerHeight * 0.6; // Adjust height based on screen
  }



}
