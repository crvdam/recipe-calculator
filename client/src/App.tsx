import { Route, Routes } from 'react-router';
import './App.css';
import HomePage from './features/home/HomePage';
import Navigation from './components/Navigation';
import IngredientPage from './features/ingredients/IngredientPage';
import RecipePage from './features/recipes/RecipePage';
import IngredientForm from './features/ingredients/IngredientForm';
import RecipeForm from './features/recipes/RecipeForm';

export default function App() {
    return (
        <div className="site">
            <Navigation />
            <main className="main">
                <Routes>
                    <Route path="/" element={<HomePage />} />
                    <Route path="/ingredients" element={<IngredientPage />}>
                        <Route
                            index
                            element={
                                <p className="detail-layout__middle">
                                    Select an ingredient or create a new one.
                                </p>
                            }
                        />
                        <Route path="new" element={<IngredientForm />} />
                        <Route path=":id" element={<IngredientForm />} />
                    </Route>
                    <Route path="/recipes" element={<RecipePage />}>
                        <Route
                            index
                            element={
                                <p className="detail-layout__middle">
                                    Select a recipe or create a new one.
                                </p>
                            }
                        />
                        <Route path="new" element={<RecipeForm />} />
                        <Route path=":id" element={<RecipeForm />} />
                    </Route>
                    <Route path="*" element={<p>Page not found</p>} />
                </Routes>
            </main>
        </div>
    );
}
