import './IngredientPage.css';
import { NavLink, Outlet } from 'react-router';
import { useEffect, useState } from 'react';
import type { IngredientResponse } from './types';

export default function IngredientPage() {
    const [ingredients, setIngredients] = useState<IngredientResponse[]>([]);

    useEffect(() => {
        let ignore = false;

        const load = async () => {
            const response = await fetch('/api/ingredients');
            const data: IngredientResponse[] = await response.json();

            if (!ignore) {
                setIngredients(data);
            }
        };

        load();

        return () => {
            ignore = true;
        };
    }, []);

    return (
        <>
            <h1>Ingredients</h1>
            <div className="detail-layout">
                <ul className="item-list">
                    {ingredients.map((ingredient) => (
                        <li key={ingredient.id} className="item-list__item">
                            <NavLink to={`/ingredients/${ingredient.id}`}>
                                {ingredient.name}
                            </NavLink>
                        </li>
                    ))}
                </ul>

                <section>
                    <Outlet />
                </section>
            </div>
        </>
    );
}
