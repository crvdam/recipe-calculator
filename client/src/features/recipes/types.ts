export type RecipeFormState = {
    name: string;
    portions: string;
    recipeLines: RecipeLineFormState[];
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
};

export type RecipeLineResponse = {
    id: number;
    ingredientId: number;
    ingredientName: string;
    quantity: number;
    lineCost: number | null;
};
