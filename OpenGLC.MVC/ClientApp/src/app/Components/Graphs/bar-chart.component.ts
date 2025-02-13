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
  title = 'Niveles de azucar';
  public type: ChartType = ChartType.Bar;
  columnNames = ['Medición', 'Glucosa'];
  options = {
    legend: { position: 'bottom' },
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
