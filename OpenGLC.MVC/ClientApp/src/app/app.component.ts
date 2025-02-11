
import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { AccountService } from './Services/AccountService';
import { HomeComponent } from './home/home.component';
import { AuthguardGuard } from './Security/AuthguardGuard';
import { MealEventComponent } from './Pages/MealEvent/meal-event.component';
import { EventsComponent } from './Pages/EventsList/events.component';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
})
export class AppComponent {
  title = 'OpenGLC App';
  isExpanded = false;
  environmentMessage = "";


  links: Array<{ text: string, path: string }> = [];

  constructor(private router: Router, public authService: AccountService) { }

  ngOnInit() {

    this.environmentMessage = ""
    this.router.config.push(
      { path: "", component: HomeComponent, canActivate: [AuthguardGuard] },
      { path: "events", component: EventsComponent, canActivate: [AuthguardGuard] },
      { path: "mealEvent", component: MealEventComponent, canActivate: [AuthguardGuard] },
    )

    this.links.push(
      { text: "Principal", path: "" },
      { text: "Bitácora", path: "events" },
      { text: "Agregar evento", path: "mealEvent" },
    );


  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  logOut() {
    this.toggle();
    localStorage.removeItem("userData");
    this.router.navigate(['/login']);
  }
}
