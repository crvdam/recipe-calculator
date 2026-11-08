import { useNavigate, useOutletContext, useParams } from 'react-router';
import './RecipeForm.css';
import type { RecipeRequest, RecipeFormState, RecipeResponse } from './types';
import type { IngredientResponse } from '../ingredients/types';
import { useEffect, useState } from 'react';

function toFormState(recipe: RecipeResponse): RecipeFormState {
    return {
        name: recipe.name,
        portions: recipe.portions.toString(),
        recipeLines: recipe.recipeLines.map((line) => ({
            key: crypto.randomUUID(),
            ingredientId: line.ingredientId.toString(),
            quantity: line.quantity.toString(),
        })),
    };
}

function RecipeForm() {
    const { reload } = useOutletContext<{ reload: () => void }>();
    const navigate = useNavigate();
    const { id } = useParams();
    const [form, setForm] = useState<RecipeFormState>({
        name: '',
        portions: '1',
        recipeLines: [],
    });
    const [recipe, setRecipe] = useState<RecipeResponse | null>(null);
    const isEdit = id !== undefined;
    const [error, setError] = useState<string | null>(null);
    const [ingredients, setIngredients] = useState<IngredientResponse[]>([]);

    useEffect(() => {
        if (id === undefined) return;

        let ignore = false;

        const load = async () => {
            const response = await fetch(`/api/recipes/${id}`);

            if (!response.ok) {
                console.error(
                    `Failed to load recipe ${id}: ${response.status}`,
                );

                navigate('/recipes', { replace: true });

                return;
            }

            const recipe: RecipeResponse = await response.json();

            if (!ignore) {
                setForm(toFormState(recipe));
                setRecipe(recipe);
            }
        };

        load();

        return () => {
            ignore = true;
        };
    }, [id, navigate]);

    useEffect(() => {
        let ignore = false;
        fetch('/api/ingredients')
            .then((response) => response.json())
            .then((data: IngredientResponse[]) => {
                if (!ignore) setIngredients(data);
            });
        return () => {
            ignore = true;
        };
    }, []);

    const handleChange = (
        event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    ) => {
        setForm({ ...form, [event.target.name]: event.target.value });
    };

    const addLine = () => {
        setForm({
            ...form,
            recipeLines: [
                ...form.recipeLines,
                { key: crypto.randomUUID(), ingredientId: '', quantity: '' },
            ],
        });
    };

    const removeLine = (key: string) => {
        setForm({
            ...form,
            recipeLines: form.recipeLines.filter((line) => line.key !== key),
        });
    };

    const updateLine = (
        key: string,
        field: 'ingredientId' | 'quantity',
        value: string,
    ) => {
        setForm({
            ...form,
            recipeLines: form.recipeLines.map((line) =>
                line.key === key ? { ...line, [field]: value } : line,
            ),
        });
    };

    const handleSubmit = async (event: React.SubmitEvent) => {
        event.preventDefault();

        setError(null);

        const request: RecipeRequest = {
            name: form.name,
            portions: Number(form.portions),
            recipeLines: form.recipeLines.map((line) => ({
                ingredientId: Number(line.ingredientId),
                quantity: Number(line.quantity),
            })),
        };

        const response = await fetch(
            isEdit ? `/api/recipes/${id}` : '/api/recipes',
            {
                method: isEdit ? 'PUT' : 'POST',
                headers: {
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify(request),
            },
        );

        if (!response.ok) {
            const problem = await response.json();
            console.error(problem);
            setError(problem.title);

            return;
        }

        const saved: RecipeResponse = await response.json();

        setRecipe(saved);

        reload();

        navigate(`/recipes/${saved.id}`);
    };

    const handleDelete = async () => {
        setError(null);

        if (!window.confirm(`Delete ${form.name}?`)) return;

        const response = await fetch(`/api/recipes/${id}`, {
            method: 'DELETE',
        });

        if (!response.ok) {
            const problem = await response.json();
            console.error(problem);
            setError(problem.title);

            return;
        }

        reload();

        navigate('/recipes');
    };

    return (
        <>
            <form className="form" onSubmit={handleSubmit}>
                <label className="form__field">
                    Name
                    <input
                        name="name"
                        required
                        value={form.name}
                        onChange={handleChange}
                    />
                </label>

                <label className="form__field">
                    Portions
                    <input
                        name="portions"
                        type="number"
                        min="1"
                        step="1"
                        value={form.portions}
                        onChange={handleChange}
                    />
                </label>

                <fieldset>
                    <legend>Ingredients</legend>

                    {form.recipeLines.map((line) => (
                        <div key={line.key} className="form__line">
                            <label className="form__field">
                                Ingredient
                                <select
                                    required
                                    value={line.ingredientId}
                                    onChange={(event) =>
                                        updateLine(
                                            line.key,
                                            'ingredientId',
                                            event.target.value,
                                        )
                                    }
                                >
                                    <option value="">Choose…</option>
                                    {ingredients.map((ingredient) => (
                                        <option
                                            key={ingredient.id}
                                            value={ingredient.id}
                                        >
                                            {ingredient.name}
                                        </option>
                                    ))}
                                </select>
                            </label>

                            <label className="form__field">
                                Quantity
                                <input
                                    type="number"
                                    required
                                    min="0.01"
                                    step="0.01"
                                    value={line.quantity}
                                    onChange={(event) =>
                                        updateLine(
                                            line.key,
                                            'quantity',
                                            event.target.value,
                                        )
                                    }
                                />
                            </label>

                            <button
                                type="button"
                                onClick={() => removeLine(line.key)}
                            >
                                Remove
                            </button>
                        </div>
                    ))}

                    <button type="button" onClick={addLine}>
                        Add ingredient
                    </button>
                </fieldset>

                <button type="submit">Save</button>
            </form>

            {isEdit && (
                <button type="button" onClick={handleDelete}>
                    Delete
                </button>
            )}

            {error && <p role="alert">{error}</p>}

            {recipe && (
                <>
                    <p>Total cost {recipe.totalCost ?? '–'}</p>
                    <p>Cost per portion {recipe.costPerPortion ?? '–'}</p>
                </>
            )}
        </>
    );
}

export default function RecipeFormRoute() {
    const { id } = useParams();
    return <RecipeForm key={id ?? 'new'} />;
}
