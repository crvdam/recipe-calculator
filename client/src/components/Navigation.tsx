import './Navigation.css';
import { NavLink } from 'react-router';

export default function Navigation() {
    return (
        <header className="header">
            <nav className="navigation" aria-label="Main">
                <p className="navigation__logo">Foodcost Calculator</p>
                <ul className="navigation__list">
                    <li>
                        <NavLink to="/">Home</NavLink>
                    </li>
                    <li>
                        <NavLink to="/ingredients">Ingredients</NavLink>
                    </li>
                    <li>
                        <NavLink to="/recipes">Recipes</NavLink>
                    </li>
                </ul>
            </nav>
        </header>
    );
}
