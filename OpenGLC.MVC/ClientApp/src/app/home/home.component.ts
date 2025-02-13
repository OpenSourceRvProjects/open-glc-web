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
  readyToRenderGraphComponent: boolean = false;
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
        this.readyToRenderGraphComponent = true;
      },
      error: (err) => {
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });
  }
}
