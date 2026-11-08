export type IngredientFormState = {
    name: string;
    baseUnit: Unit;
    purchasePrice: string;
    purchaseQuantity: string;
    yieldPercentage: string;
};

export type IngredientRequest = {
    name: string;
    baseUnit: Unit;
    purchasePrice: number | null;
    purchaseQuantity: number | null;
    yieldPercentage: number;
};

export type Unit = 'Gram' | 'Millilitre' | 'Piece';

export type IngredientResponse = {
    id: number;
    name: string;
    baseUnit: Unit;
    purchasePrice: number | null;
    purchaseQuantity: number | null;
    yieldPercentage: number;
    costPerBaseUnit: number | null;
};
