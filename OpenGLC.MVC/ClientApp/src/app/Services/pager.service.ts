import { Injectable } from '@angular/core';
import PaginationModel from '../Models/PaginationModel';

@Injectable({
  providedIn: 'root'
})
export class PagerService {

  constructor() { }

  getItemPerPageOptions() {

    var options = new Array<PaginationModel.IItemsPerPage>();
    options.push(<PaginationModel.IItemsPerPage>{ text: "5 elementos por página", value: 5 })
    options.push(<PaginationModel.IItemsPerPage>{ text: "8 elementos por página", value: 8 })
    options.push(<PaginationModel.IItemsPerPage>{ text: "10 elementos por página", value: 10 })
    options.push(<PaginationModel.IItemsPerPage>{ text: "20 elementos por página", value: 20 })
    options.push(<PaginationModel.IItemsPerPage>{ text: "50 elementos por página", value: 50 })

    return options;

  }

  setPageLinks(currentPage: number, totalPages: number) {
    debugger;
    var viewPageLinks = [];
    var pagesForDisplay = 0;

    for (var cp = currentPage - 3; pagesForDisplay < 7; cp++) {
      if (cp >= 0) {
        viewPageLinks[pagesForDisplay] = cp;
        pagesForDisplay++;
      }

      if (cp == totalPages - 1)
        break;
    }
    return viewPageLinks;
  }
}
