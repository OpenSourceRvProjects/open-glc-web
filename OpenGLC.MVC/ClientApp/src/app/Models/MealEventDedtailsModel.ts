export interface MealEventDedtailsModel {
  id: string;
  eventDate: string;
  glcLevel: number;
  pospandrial: boolean;
  mealList: EventMealItemsModel[];
  mealTypeText: string;
}

export interface EventMealItemsModel {
  mealName: string;
  mealID: string;
  quantity: number;
}
