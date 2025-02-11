export interface PaginationListEntityModel<T> {
  pagedList: T[];
  pageNumber: number;
  totalCount: number;
  totalPages: number;
}
