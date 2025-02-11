import { Component, OnInit } from '@angular/core';
import { EventMealsService } from '../Services/event-meals.service';

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

}
