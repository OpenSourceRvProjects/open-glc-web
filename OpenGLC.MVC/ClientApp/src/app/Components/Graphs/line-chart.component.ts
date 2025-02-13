import { Component, HostListener, Input, OnInit } from '@angular/core';
import { ChartType } from 'angular-google-charts'; // Import the ChartType enum
import { EventMealsService } from '../../Services/event-meals.service';

@Component({
  selector: 'glc-line-chart',
  templateUrl: './line-chart.component.html',
})
export class LineChartComponent implements OnInit {
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
  title = 'Niveles de azucar';
  public type: ChartType = ChartType.LineChart;
  columnNames = ['Medición', 'Glucosa'];
  options = {
    curveType: 'function', legend: { position: 'bottom' },
    //trendlines: { 0: {} }
    trendlines: {
      0: {
        type: 'linear',    // Trendline type (linear in this case)
        color: 'red',      // Color of the trendline
        lineWidth: 2,      // Optional: line width of the trendline
        labelInLegend: 'Tendencia lineal',
        showEquation: true, // Show the equation of the trendline on the chart
        visibleInLegend: true // Show R2 value in the legend
      },
    },
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
