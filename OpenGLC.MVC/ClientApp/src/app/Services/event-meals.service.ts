import { Inject, Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { AccountService } from './AccountService';
import { NewMealEventModel } from '../Models/NewMealEventModel';


@Injectable({
  providedIn: 'root'
})
export class EventMealsService {

  constructor(private httpClient: HttpClient, private authService: AccountService, @Inject('BASE_URL') private baseUrl: string) { }

  // options : {withCredentials: true,

  // };
  getUserMetrics() {

    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealEvents/userEventMetrics", options);
  }

  getLastThreeMonths() {

    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealEvents/lastThreeMonthsLevels", options);
  }


  getMealEventTypes() {
    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealEvents/getEventMealTypes", options
    );
  }

  addMealEvent(newMealEvent: NewMealEventModel) {

    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.post(this.baseUrl + "api/MealEvents", newMealEvent, options);
  }

  getMealItems(page: number, itemsPerPage: number, searchTerm: string) {
    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealItems?page=" + page + "&itemsPerPage=" + 1000, options
    );
  }

  getMealEvents(page: number, itemsPerPage: number, searchTerm: string) {

    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealEvents?page=" + page + "&itemsPerPage=" + itemsPerPage + "&searchTerm=" + searchTerm, options
    );
  }

  getMealEventDetails(eventID: string) {

    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + this.authService.getUserData().token }) };

    return this.httpClient.get(this.baseUrl + "api/MealEvents/id?eventId=" + eventID, options);
  }

  deleteEvent(eventID: string, specialToken: string) {
    debugger;
    const options = { withCredentials: true, headers: new HttpHeaders({ 'Authorization': 'Bearer ' + specialToken }) };
    return this.httpClient.delete(this.baseUrl + "api/MealEvents/id?eventId=" + eventID, options);
  }
}
