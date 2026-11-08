import './IngredientPage.css';
import { NavLink, Outlet } from 'react-router';
import { useEffect, useState } from 'react';
import type { IngredientResponse } from './types';

async function fetchIngredients(): Promise<IngredientResponse[]> {
    const response = await fetch('/api/ingredients');
    return response.json();
}

export default function IngredientPage() {
    const [ingredients, setIngredients] = useState<IngredientResponse[]>([]);

    useEffect(() => {
        let ignore = false;
        fetchIngredients().then((data) => {
            if (!ignore) setIngredients(data);
        });
        return () => {
            ignore = true;
        };
    }, []);

    const reload = async () => {
        setIngredients(await fetchIngredients());
    };

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
                    <Outlet context={{ reload }} />
                </section>
            </div>
        </>
    );
}
