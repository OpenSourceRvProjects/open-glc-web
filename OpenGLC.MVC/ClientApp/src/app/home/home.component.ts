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

      },
      error: (err) => {
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });

  }
}
