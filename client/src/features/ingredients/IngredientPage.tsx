import './IngredientPage.css';
import { NavLink, Outlet } from 'react-router';

export default function IngredientsPage() {
    return (
        <>
            <h1>Ingredients</h1>
            <div className="detail-layout">
                <ul className="item-list">
                    <li className="item-list__item">
                        <NavLink to="/ingredients/1">Butter</NavLink>
                    </li>
                    <li className="item-list__item">
                        <NavLink to="/ingredients/2">Cream</NavLink>
                    </li>
                    <li className="item-list__item">
                        <NavLink to="/ingredients/3">Milk</NavLink>
                    </li>
                </ul>

                <section>
                    <Outlet />
                </section>
            </div>
        </>
    );
}
