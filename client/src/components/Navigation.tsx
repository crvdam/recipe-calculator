import './Navigation.css';
import { NavLink } from 'react-router';

export default function Navigation() {
    return (
        <nav aria-label="Main">
            <ul className="nav-list">
                <li>
                    <NavLink to="/home">Home</NavLink>
                </li>
                <li>
                    <NavLink to="/ingredients">Ingredients</NavLink>
                </li>
                <li>
                    <NavLink to="/recipes">Recipes</NavLink>
                </li>
            </ul>
        </nav>
    );
}
