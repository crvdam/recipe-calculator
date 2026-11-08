import { useNavigate, useOutletContext, useParams } from 'react-router';
import './IngredientForm.css';
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
    const isEdit = id !== undefined;

    useEffect(() => {
        if (id === undefined) return;

        let ignore = false;

        const load = async () => {
            const response = await fetch(`/api/ingredients/${id}`);

            if (!response.ok) {
                console.error(
                    `Failed to load ingredient ${id}: ${response.status}`,
                );
                return;
            }

            const ingredient: IngredientResponse = await response.json();

            if (!ignore) {
                setForm(toFormState(ingredient));
            }
        };

        load();

        return () => {
            ignore = true;
        };
    }, [id]);

    const handleChange = (
        event: React.ChangeEvent<HTMLInputElement | HTMLSelectElement>,
    ) => {
        setForm({ ...form, [event.target.name]: event.target.value });
    };

    const handleSubmit = async (event: React.SubmitEvent) => {
        event.preventDefault();

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
            console.error(await response.json());
            return;
        }

        const saved: IngredientResponse = await response.json();

        reload();

        navigate(`/ingredients/${saved.id}`);
    };

    return (
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

            <button type="submit">Save</button>
        </form>
    );
}

export default function IngredientFormRoute() {
    const { id } = useParams();
    return <IngredientForm key={id ?? 'new'} />;
}
