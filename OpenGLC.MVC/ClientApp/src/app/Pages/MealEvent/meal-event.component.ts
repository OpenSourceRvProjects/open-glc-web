
import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { EventMealsService } from 'src/app/Services/event-meals.service';
import { ExistingMealItemPair, NewMealEventModel, NewMealItemModelDB, NewMealItemPair } from '../../Models/NewMealEventModel';

interface newMealQuantity {
  mealName: string;
  quantity: number;
}

interface MealType {
  type: number;
  name: string;
}

@Component({
  selector: 'app-meal-event',
  templateUrl: './meal-event.component.html',
  //styleUrls: ['./meal-event.component.css']
})
export class MealEventComponent implements OnInit {


  newMealslist: newMealQuantity[] = [];
  newMealItem: newMealQuantity = <newMealQuantity>{ mealName: "", quantity: 1 };


  preexistedMealList: NewMealItemModelDB[] = [];
  selectedMealItemToSave: NewMealItemModelDB = <NewMealItemModelDB>{};

  auxiliarPreexistedMealListView: NewMealItemModelDB[] = [];
  mealItemsListToSaveOnServer: ExistingMealItemPair[] = [];


  mealEventTypes: MealType[] = [];
  selectedEventType: MealType = <MealType>{};

  eventDate: Date;
  isPospandrial: boolean = true;
  glcLevel: number | undefined = undefined;
  newMealEvent: NewMealEventModel;

  errorMessage = "";
  processing: boolean = false;

  constructor(private eventMealService: EventMealsService, private router: Router) {
    debugger;
    var today = new Date();
    var month = today.getMonth();
    var year = today.getFullYear();
    var day = today.getDate();

    this.eventDate = new Date(year, month, day);
    this.newMealEvent = <NewMealEventModel>{}
    this.newMealEvent.eventDate = this.eventDate;
    this.newMealEvent.itemMeals = new Array();
    this.newMealEvent.newMeals = new Array();
    this.newMealEvent.glcLevel = 0;
  }

  ngOnInit(): void {
    this.eventMealService.getMealEventTypes().subscribe({
      next: (data: any) => {
        debugger;
        this.mealEventTypes = data;
        this.selectedEventType = this.mealEventTypes[0];

      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });

    this.eventMealService.getMealItems(0, 1000, "").subscribe({
      next: (data: any) => {
        debugger;
        this.preexistedMealList = data.pagedList;
        this.selectedMealItemToSave = this.preexistedMealList[0];

      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });
  }

  addToNewMealList() {

    debugger;
    if (this.newMealItem.mealName.trim() === "")
      return;

    var localNewMealItem = this.newMealItem;
    this.newMealslist.push(localNewMealItem);

    this.newMealItem = <newMealQuantity>{ mealName: "", quantity: 1 };

  }

  deleteNewMealItem(index: number) {
    this.newMealslist.splice(index, 1);
  }

  saveEvent() {
    this.processing = true;
    this.errorMessage = ""

    debugger;
    if (this.glcLevel == null || this.eventDate == null || !Date.parse(this.eventDate.toString())) {

      this.processing = false;
      this.errorMessage = "El nivel de glucosa y fecha del evento son obligatorios";
      return;
    }

    if (this.glcLevel < 0) {
      this.processing = false;
      this.errorMessage = "No se puede registrar una cantidad negativa";
      return;
    }

    this.newMealEvent.eventDate = this.eventDate;
    this.newMealEvent.glcLevel = this.glcLevel;
    this.newMealEvent.mealType = this.selectedEventType.type;
    this.newMealEvent.postprandial = this.isPospandrial;

    if (this.newMealslist.length > 0) {

      this.newMealslist.forEach(f => {
        var newMealQty = <NewMealItemPair>{};
        newMealQty.name = f.mealName;
        newMealQty.quantity = f.quantity;
        this.newMealEvent.newMeals.push(newMealQty);
      });

    }

    if (this.auxiliarPreexistedMealListView.length > 0) {
      this.auxiliarPreexistedMealListView.forEach(f => {
        debugger;
        var newPreexistedItemToSave = <ExistingMealItemPair>{};
        newPreexistedItemToSave.iD = f.id,
          newPreexistedItemToSave.quantity = f.quantity,
          this.mealItemsListToSaveOnServer.push(newPreexistedItemToSave);

      });

      this.newMealEvent.itemMeals = this.mealItemsListToSaveOnServer;

    }

    // alert("Datos a guardar: Glucosa " + this.newMealEvent.glcLevel + ", Fecha: " + this.newMealEvent.eventDate + ", TIpo comida: " + this.newMealEvent.mealType)
    this.eventMealService.addMealEvent(this.newMealEvent).subscribe({
      next: (data: any) => {
        debugger;
        this.processing = false;
        this.router.navigate(['/events']);
      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
        this.processing = false;
      },
    });
  }

  updateMealType($event: any) {
    debugger;
    this.selectedEventType = $event;

  }

  updateMealItemSelected($event: any) {
    debugger;
    this.selectedMealItemToSave = $event;

  }

  addToAuxiliarListView() {
    debugger;
    this.auxiliarPreexistedMealListView.push(this.selectedMealItemToSave);
    this.selectedMealItemToSave = <NewMealItemModelDB>{};
    this.selectedMealItemToSave = this.preexistedMealList[0];

  }

  deletePreexistedAuxiliarList(index: number) {
    this.auxiliarPreexistedMealListView.splice(index, 1);
  }

}
