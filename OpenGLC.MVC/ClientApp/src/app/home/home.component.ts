import { Component, HostListener, OnInit } from '@angular/core';
import { EventMealsService } from '../Services/event-meals.service';
import { ChartType } from 'angular-google-charts'; // Import the ChartType enum

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  constructor(private eventService: EventMealsService) { }

  userData: any = {};
  serverData: number[] = [];
  showGraph: boolean = false;
  dataForGraph: [number, number][] = [];
  ngOnInit(): void {

    this.eventService.getUserMetrics().subscribe({
      next: (data: any) => {
        this.userData = data;
        this.getLast3MonthsForGraph();

      },
      error: (err) => {
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });

  }

  getLast3MonthsForGraph() {
    this.eventService.getLastThreeMonths().subscribe({
      next: (data: any) => {
        this.serverData = data;
        //https://stackoverflow.com/questions/19642276/google-charts-trendline-not-showing?rq=1
        //cannot use the forEach, it seeks the indexOf a value, not the reference. so You need to specify second parameter for array
        //this.serverData.forEach(fe => this.dataForGraph.push([this.serverData.indexOf(fe), fe]));
        this.serverData.forEach((fe, index) => this.dataForGraph.push([index, fe]));
        debugger;
        this.showGraph = true;
      },
      error: (err) => {
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });
  }

  //npm install angular-google - charts@12

  title = 'Niveles de azucar';
  public type: ChartType = ChartType.LineChart;
  //data = [
  //  ["", 200],
  //  ["", 560],
  //  ["", 280],
  //  ["", 300],
  //  ["", 600]
  //];
  columnNames = ['Medición', 'Nivel de Glc'];
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
  //width = 600;
  //height = 400;  

}
