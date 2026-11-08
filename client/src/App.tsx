import { Navigate, Route, Routes } from 'react-router';
import HomePage from './features/home/HomePage';
import Navigation from './components/Navigation';
import IngredientsPage from './features/ingredients/IngredientsPage';
import RecipesPage from './features/recipes/RecipesPage';

export default function App() {
    return (
        <>
            <Navigation />
            <main>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/ingredients" element={<IngredientsPage />} />
                    <Route path="/recipes" element={<RecipesPage />} />
                    <Route path="*" element={<p>Page not found</p>} />
                </Routes>
            </main>
        </>
    );
}
