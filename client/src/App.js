import logo from './logo.svg';
import './App.css';
import { useState, useEffect } from 'react';
import { auth } from './firebase';
import { BrowserRouter as Router, Routes, Route, Link, useLocation, Navigate } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Footer from './components/Footer/Footer'
import Dashboard from './pages/Dashboard';
import Watchlist from './pages/Watchlist';
import Register from './pages/Register';
import Login from './pages/Login';
import Profile from './pages/Profile';
import SingleMovie from './pages/SingleMovie';

// App.js - Zamijeni LayoutWrapper funkciju ovim:

function LayoutWrapper() {
  const location = useLocation();
  const isAuthPage = location.pathname === '/register' || location.pathname === '/login';
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    const unsubscribe = auth.onAuthStateChanged((currentUser) => {
      setUser(currentUser);
      setLoading(false);
    });
    return () => unsubscribe();
  }, []);

  if (loading) return <div className="loading">Učitavanje...</div>;

  // GLAVNA PROMJENA: Ako korisnik nije ulogovan i nije na login/register, šalji ga na login
  if (!user && !isAuthPage) {
    return <Navigate to="/login" replace />;
  }

  return (
    <div className="app-container">
      {!isAuthPage && <Navbar />}
      <main className="content">
        <Routes>
          {/* Početna ruta sada direktno vodi na Dashboard ako je user tu */}
          <Route path="/" element={<Dashboard />} />
          <Route path="/Dashboard" element={<Dashboard />} />
          <Route path="/MyWatchlist" element={<Watchlist />} />
          <Route path="/profile" element={<Profile />} />
          <Route path="/details/:type/:id" element={<SingleMovie />} />
          
          <Route path="/register" element={<Register />} /> 
          <Route path="/login" element={<Login />} />  
          
          {/* Ako korisnik ukuca nepostojeću rutu */}
          <Route path="*" element={<Navigate to="/" />} />
        </Routes>
      </main>
      {!isAuthPage && <Footer />}
    </div>
  );
}

function App(){

  return(
    <div className="App">
      <Router>
        <LayoutWrapper />
      </Router>
    </div>
  );

}

export default App;
