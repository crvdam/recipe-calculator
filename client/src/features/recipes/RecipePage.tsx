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
                <div className="detail-layout__left">
                    <Link to="new" className="btn btn-primary">
                        New recipe
                    </Link>
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

                <div className="detail-layout__middle">
                    <Outlet context={{ reload }} />
                </div>

                <div className="detail-layout__right"></div>
            </div>
        </>
    );
}
