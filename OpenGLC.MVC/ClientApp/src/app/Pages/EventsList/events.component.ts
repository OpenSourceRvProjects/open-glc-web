
import { Component, OnInit } from '@angular/core';
import { EventMealsService } from 'src/app/Services/event-meals.service';
import { PagerService } from 'src/app/Services/pager.service';
import { AccountService } from '../../Services/AccountService';
import PaginationModel from '../../Models/PaginationModel';
import { MealEventModel } from '../../Models/MealEventModel';
import { MealEventDedtailsModel } from '../../Models/MealEventDedtailsModel';
import { PaginationListEntityModel } from '../../Models/PaginationListEntityModel';

@Component({
  selector: 'app-events',
  templateUrl: './events.component.html',
})
export class EventsComponent implements OnInit {


  pagedData: PaginationListEntityModel<MealEventModel> = <PaginationListEntityModel<MealEventModel>>{}
  selectedMealEvent: MealEventModel = <MealEventModel>{};

  currentEventShow: MealEventDedtailsModel = <MealEventDedtailsModel>{}
  selectedIndexEventType = 0;
  deleteMode: boolean = false;

  itemsPerPage: number = 5;
  currentPage: number = 0;
  totalPages: number = 0;
  itemsPerPageOptionValue: Array<PaginationModel.IItemsPerPage>;
  selectedItemPerPageOption: PaginationModel.IItemsPerPage;
  viewPageLinks: number[] = [];
  userPassword: string = "";
  processingGetToken: boolean = false;
  deleteToken: string = "";
  public isCollapsed = true;
  searchTerm = "";

  constructor(private eventMealService: EventMealsService, private pagerService: PagerService
    , private accountService: AccountService) {
    this.itemsPerPageOptionValue = this.pagerService.getItemPerPageOptions();
    this.selectedItemPerPageOption = this.itemsPerPageOptionValue[0];
  }

  // #region server call definition methods
  getEvents() {
    debugger;
    this.eventMealService.getMealEvents(this.currentPage, this.itemsPerPage, this.searchTerm).subscribe({
      next: (data: any) => {
        debugger;
        this.pagedData = data;
        this.totalPages = this.pagedData.totalPages;
        this.selectedMealEvent = this.pagedData.pagedList[0];
        this.viewPageLinks = this.pagerService.setPageLinks(this.currentPage, this.totalPages);
        this.getDataFromSelectedEventID();
      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });
  }

  getDeleteToken() {
    this.processingGetToken = true;
    this.accountService.login(this.accountService.getUserData().userName, this.userPassword, true)
      .subscribe({
        next: (data: any) => {
          debugger;
          this.deleteToken = data.token;
          this.deleteEventById();

        },
        error: (err) => {
          debugger;
          alert("Contraseña incorrecta o servicio no disponible! -->" + err.error.errorMessages[0]);
          this.processingGetToken = false;
        },
      });

  }

  deleteEventById() {
    var selectedEventID = this.currentEventShow.id;
    this.eventMealService.deleteEvent(selectedEventID, this.deleteToken)
      .subscribe({
        next: (data: any) => {
          debugger;
          this.deleteToken = "";
          window.location.reload();

        },
        error: (err) => {
          debugger;
          alert("Contraseña incorrecta o servicio no disponible! -->" + err.error.errorMessages[0]);
          this.processingGetToken = false;
        },
      });

  }

  getDataFromSelectedEventID() {
    // alert(this.selectedMealEvent.id);
    this.eventMealService.getMealEventDetails(this.selectedMealEvent.id).subscribe({
      next: (data: any) => {
        debugger;
        this.currentEventShow = data;
      },
      error: (err) => {
        debugger;
        alert("Ha ocurrido un error -->" + err.error.errorMessages[0]);
      },
    });

  }

  //#endregion

  // #region component methodos

  changeSelectedIndex(i: number) {
    debugger;
    this.deleteMode = false;
    this.selectedIndexEventType = i;
    this.selectedMealEvent = this.pagedData.pagedList[this.selectedIndexEventType];
    this.getDataFromSelectedEventID();
  }

  enableDeleteEvent(i: number) {
    debugger;
    this.deleteMode = true;
  }

  closeDeleteMode() {
    this.deleteMode = false;
  }

  //#endregion


  //#region pager logic methods

  manageItemsPerPage(event: any) {
    debugger;
    this.itemsPerPage = this.selectedItemPerPageOption.value;
    this.currentPage = 0;
    this.getEvents();
  }


  firstPage() {
    this.currentPage = 0;
    this.getEvents();
  }

  lastPage() {
    this.currentPage = this.totalPages - 1;
    this.getEvents();
  }

  nextPage() {
    this.currentPage++;
    this.getEvents();
  }

  previousPage() {
    this.currentPage--;
    this.getEvents();
  }

  goToPage(pageNumber: number) {
    this.currentPage = pageNumber;
    this.getEvents();
  }



  ngOnInit(): void {
    this.getEvents();

  }


}
