import './RecipePage.css';
import { NavLink, Outlet, Link } from 'react-router';
import { useEffect, useState } from 'react';
import type { RecipeResponse } from './types';

async function fetchRecipes(): Promise<RecipeResponse[]> {
    const response = await fetch('/api/recipes');
    return response.json();
}

export default function RecipePage() {
    const [recipes, setRecipes] = useState<RecipeResponse[]>([]);

    useEffect(() => {
        let ignore = false;
        fetchRecipes().then((data) => {
            if (!ignore) setRecipes(data);
        });
        return () => {
            ignore = true;
        };
    }, []);

    const reload = async () => {
        setRecipes(await fetchRecipes());
    };

    return (
        <>
            <h1>Recipes</h1>
            <div className="detail-layout">
                <div>
                    <Link to="new">New Recipe</Link>
                    <ul className="item-list">
                        {recipes.map((recipe) => (
                            <li key={recipe.id} className="item-list__item">
                                <NavLink to={`/recipes/${recipe.id}`}>
                                    {recipe.name}
                                </NavLink>
                            </li>
                        ))}
                    </ul>
                </div>

                <section>
                    <Outlet context={{ reload }} />
                </section>
            </div>
        </>
    );
}
