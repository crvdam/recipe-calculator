import './IngredientForm.css';
import type { IngredientRequest, IngredientFormState } from './types';
import { useState } from 'react';

export default function IngredientForm() {
    const [form, setForm] = useState<IngredientFormState>({
        name: '',
        baseUnit: 'Gram',
        purchasePrice: '',
        purchaseQuantity: '',
        yieldPercentage: '100',
    });

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
