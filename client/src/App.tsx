import { Route, Routes } from 'react-router';
import HomePage from './features/home/HomePage';
import Navigation from './components/Navigation';
import IngredientPage from './features/ingredients/IngredientPage';
import RecipePage from './features/recipes/RecipePage';
import IngredientForm from './features/ingredients/IngredientForm';

export default function App() {
    return (
        <>
            <Navigation />
            <main>
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/ingredients" element={<IngredientPage />}>
                        <Route
                            index
                            element={
                                <p>Select an ingredient or create a new one.</p>
                            }
                        />
                        <Route path="new" element={<IngredientForm />} />
                        <Route path=":id" element={<IngredientForm />} />
                    </Route>
                    <Route path="/recipes" element={<RecipePage />} />
                    <Route path="*" element={<p>Page not found</p>} />
                </Routes>
            </main>
        </>
    );
}
