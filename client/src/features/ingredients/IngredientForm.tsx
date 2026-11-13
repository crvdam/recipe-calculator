import { useNavigate, useOutletContext, useParams } from 'react-router';
import type {
    IngredientRequest,
    IngredientFormState,
    IngredientResponse,
} from './types';
import { useEffect, useState } from 'react';

function toFormState(ingredient: IngredientResponse): IngredientFormState {
    return {
        name: ingredient.name,
        baseUnit: ingredient.baseUnit,
        purchasePrice: ingredient.purchasePrice?.toString() ?? '',
        purchaseQuantity: ingredient.purchaseQuantity?.toString() ?? '',
        yieldPercentage: ingredient.yieldPercentage.toString(),
    };
}

function IngredientForm() {
    const { reload } = useOutletContext<{ reload: () => void }>();
    const navigate = useNavigate();
    const { id } = useParams();
    const [form, setForm] = useState<IngredientFormState>({
        name: '',
        baseUnit: 'Gram',
        purchasePrice: '',
        purchaseQuantity: '',
        yieldPercentage: '100',
    });
    const [ingredient, setIngredient] = useState<IngredientResponse | null>(
        null,
    );
    const isEdit = id !== undefined;
    const [error, setError] = useState<string | null>(null);

    useEffect(() => {
        if (id === undefined) return;

        let ignore = false;

        const load = async () => {
            const response = await fetch(`/api/ingredients/${id}`);

            if (!response.ok) {
                console.error(
                    `Failed to load ingredient ${id}: ${response.status}`,
                );

                navigate('/ingredients', { replace: true });

                return;
            }

            const ingredient: IngredientResponse = await response.json();

            if (!ignore) {
                setForm(toFormState(ingredient));
                setIngredient(ingredient);
            }
        };

        load();

        return () => {
            ignore = true;
        };
    }, [id, navigate]);

    const handleChange = (
        event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    ) => {
        setForm({ ...form, [event.target.name]: event.target.value });
    };

    const handleSubmit = async (event: React.SubmitEvent) => {
        event.preventDefault();

        setError(null);

        const request: IngredientRequest = {
            name: form.name,
            baseUnit: form.baseUnit,
            purchasePrice:
                form.purchasePrice === '' ? null : Number(form.purchasePrice),
            purchaseQuantity:
                form.purchaseQuantity === ''
                    ? null
                    : Number(form.purchaseQuantity),
            yieldPercentage: Number(form.yieldPercentage),
        };

        const response = await fetch(
            isEdit ? `/api/ingredients/${id}` : '/api/ingredients',
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

        const saved: IngredientResponse = await response.json();

        setIngredient(saved);

        reload();

        navigate(`/ingredients/${saved.id}`);
    };

    const handleDelete = async () => {
        setError(null);

        if (!window.confirm(`Delete ${form.name}?`)) return;

        const response = await fetch(`/api/ingredients/${id}`, {
            method: 'DELETE',
        });

        if (!response.ok) {
            const problem = await response.json();
            console.error(problem);
            setError(problem.title);

            return;
        }

        reload();

        navigate('/ingredients');
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
                    Base unit
                    <select
                        name="baseUnit"
                        value={form.baseUnit}
                        onChange={handleChange}
                        required
                    >
                        <option value="Gram">Gram</option>
                        <option value="Millilitre">Millilitre</option>
                        <option value="Piece">Piece</option>
                    </select>
                </label>

                <label className="form__field">
                    Purchase price (€)
                    <input
                        name="purchasePrice"
                        type="number"
                        min="0.01"
                        step="0.01"
                        value={form.purchasePrice}
                        onChange={handleChange}
                    />
                </label>

                <label className="form__field">
                    Quantity ({form.baseUnit})
                    <input
                        name="purchaseQuantity"
                        type="number"
                        min="0.01"
                        step="0.01"
                        value={form.purchaseQuantity}
                        onChange={handleChange}
                    />
                </label>

                <label className="form__field">
                    Yield (%)
                    <input
                        name="yieldPercentage"
                        type="number"
                        min="1"
                        max="100"
                        step="1"
                        value={form.yieldPercentage}
                        onChange={handleChange}
                    />
                </label>

                <div className="button-container">
                    {isEdit && (
                        <button
                            type="button"
                            className="btn btn-delete"
                            onClick={handleDelete}
                        >
                            Delete
                        </button>
                    )}
                    <button type="submit" className="btn btn-secondary">
                        Save
                    </button>
                </div>
            </form>

            {error && <p role="alert">{error}</p>}

            {ingredient && (
                <p>
                    Cost per {ingredient.baseUnit.toLowerCase()}:{' '}
                    {ingredient.costPerBaseUnit ?? '–'}
                </p>
            )}
        </>
    );
}

export default function IngredientFormRoute() {
    const { id } = useParams();
    return <IngredientForm key={id ?? 'new'} />;
}
