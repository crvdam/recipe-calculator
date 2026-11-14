import type { Unit } from '../ingredients/types';

export type RecipeFormState = {
    name: string;
    portions: string;
    recipeLines: RecipeLineFormState[];
    outputQuantity: string;
    outputUnit: Unit | '';
};

export type RecipeLineFormState = {
    key: string;
    ingredientId: string;
    quantity: string;
};

export type RecipeRequest = {
    name: string;
    portions: number;
    recipeLines: RecipeLineRequest[];
    outputQuantity: number | null;
    outputUnit: Unit | null;
};

export type RecipeLineRequest = {
    ingredientId: number;
    quantity: number;
};

export type RecipeResponse = {
    id: number;
    name: string;
    portions: number;
    recipeLines: RecipeLineResponse[];
    totalCost: number | null;
    costPerPortion: number | null;
    outputQuantity: number | null;
    outputUnit: Unit | null;
    costPerOutputUnit: number | null;
};

export type RecipeLineResponse = {
    id: number;
    ingredientId: number;
    ingredientName: string;
    quantity: number;
    lineCost: number | null;
};
