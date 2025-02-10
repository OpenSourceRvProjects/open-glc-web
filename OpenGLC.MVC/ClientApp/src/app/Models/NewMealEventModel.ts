export interface NewMealEventModel {
  eventDate: Date;
  glcLevel: number;
  newMeals: NewMealItemPair[];
  itemMeals: ExistingMealItemPair[];
  postprandial: boolean;
  mealType: number;
}

export interface NewMealItemPair {
  name: string;
  quantity: number;
}

export interface ExistingMealItemPair {
  iD: string;
  quantity: number;
}

export interface NewMealItemModelDB {
  id: string;
  name: string;
  quantity: number;
}
