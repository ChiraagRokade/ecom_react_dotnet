import { BrowserRouter as Router, Routes, Route, Navigate } from 'react-router-dom';
import { useEffect, useState } from 'react';
import './App.css';
import LoginPage from './pages/LoginPage';
import WeatherForecast from './components/WeatherForecast';

function App() {
    return (
        <Router>
            <Routes>
                {/* Redirect root to login */}
                <Route path="/" element={<Navigate to="/login" replace />} />
                
                {/* Login route */}
                <Route path="/login" element={<LoginPage />} />
                
                {/* Weather forecast route (protected route example) */}
                <Route path="/weather" element={<WeatherForecast />} />
            </Routes>
        </Router>
    );
}

export default App;