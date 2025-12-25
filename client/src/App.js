// App.js

import './App.css';
import { useState, useEffect } from 'react';
import { auth } from './firebase';
import { BrowserRouter as Router, Routes, Route, useLocation, Navigate } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Footer from './components/Footer/Footer'
import Dashboard from './pages/Dashboard';
import Watchlist from './pages/Watchlist';
import Register from './pages/Register';
import Login from './pages/Login';
import Profile from './pages/Profile';
import SingleMovie from './pages/SingleMovie';
import BrowseAll from './components/BrowseAll';

function LayoutWrapper() {
  const location = useLocation();
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  // Provjera rute - da li je korisnik na login ili register?
  const isAuthPage = location.pathname === '/register' || location.pathname === '/login';

  useEffect(() => {
    const unsubscribe = auth.onAuthStateChanged((currentUser) => {
      setUser(currentUser);
      setLoading(false);
    });
    return () => unsubscribe();
  }, []);

  // Dok Firebase provjerava sesiju, prikazujemo loading
  if (loading) {
    return <div className="loading">Loading...</div>;
  }

  // LOGIKA ZAŠTITE:
  // 1. Ako NEMA korisnika, a NIJE na login/register stranici -> šalji na /login
  if (!user && !isAuthPage) {
    return <Navigate to="/login" replace />;
  }

  // 2. Ako JE korisnik ulogovan, a pokuša otići na /login ili /register -> šalji na Dashboard
  if (user && isAuthPage) {
    return <Navigate to="/dashboard" replace />;
  }

  return (
    <div className="app-container">
      {/* Navbar i Footer se prikazuju samo ako je korisnik ulogovan (i nije na auth stranicama) */}
      {!isAuthPage && user && <Navbar />}
      
      <main className="content">
        <Routes>
          {/* Početna ruta "/" - ako si tu, baci te na Dashboard (koji je zaštićen gore) */}
          <Route path="/" element={<Navigate to="/dashboard" replace />} />
          
          <Route path="/dashboard" element={<Dashboard />} />
          <Route path="/myWatchlist" element={<Watchlist />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/details/:type/:id" element={<SingleMovie />} />
          <Route path="/all-movies" element={<BrowseAll type="movie" />} />
          <Route path="/all-tvshows" element={<BrowseAll type="tv" />} />
          
          {/* Javne rute */}
          <Route path="/register" element={<Register />} /> 
          <Route path="/login" element={<Login />} />  

          {/* Fallback: ako korisnik upiše bilo šta glupo u URL, vrati ga na početnu */}
          <Route path="*" element={<Navigate to="/" replace />} />
        </Routes>
      </main>

      {!isAuthPage && user && <Footer />}
    </div>
  );
}

function App() {
  return (
    <div className="App">
      <Router>
        <LayoutWrapper />
      </Router>
    </div>
  );
}

export default App;