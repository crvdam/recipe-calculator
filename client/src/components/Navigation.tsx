import './Navigation.css';
import { NavLink } from 'react-router';

export default function Navigation() {
    return (
        <header className="header">
            <nav className="navigation" aria-label="Main">
                <p className="navigation__logo">Foodcost Calculator</p>
                <ul className="navigation__list">
                    <li>
                        <NavLink to="/" data-text="Home">
                            Home
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/ingredients" data-text="Ingredients">
                            Ingredients
                        </NavLink>
                    </li>
                    <li>
                        <NavLink to="/recipes" data-text="Recipes">
                            Recipes
                        </NavLink>
                    </li>
                </ul>
            </nav>
        </header>
    );
}
