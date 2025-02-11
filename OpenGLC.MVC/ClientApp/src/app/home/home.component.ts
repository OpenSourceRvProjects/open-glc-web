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
  ngOnInit(): void {

    this.eventService.getUserMetrics().subscribe({
      next: (data: any) => {
        debugger;
        this.userData = data;

      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });

  }




  title = 'Company Hiring Report';
  public type: ChartType = ChartType.LineChart;
  data = [
    ["", 200],
    ["", 560],
    ["", 280],
    ["", 300],
    ["", 600]
  ];
  columnNames = ['Year', 'Nivel de Glc'];
  options = {
    curveType: 'function', legend: { position: 'bottom' }
  };

  width: number = window.innerWidth * 0.8; // 80% of the window width
  height: number = 400; // You can also dynamically adjust this

  // Dynamically update the height based on window size
  @HostListener('window:resize', ['$event'])
  onResize(event: any) {
    this.width = event.target.innerWidth * 0.8;  // 80% of the viewport width
    this.height = event.target.innerHeight * 0.6; // Adjust height based on screen
  }
  //width = 600;
  //height = 400;  

}
