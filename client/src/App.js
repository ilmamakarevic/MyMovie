import logo from './logo.svg';
import './App.css';
import { useState, useEffect } from 'react';
import { auth } from './firebase';
import { BrowserRouter as Router, Routes, Route, Link, useLocation } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Footer from './components/Footer/Footer'
import Dashboard from './pages/Dashboard';
import Watchlist from './pages/Watchlist';
import Register from './pages/Register';
import Login from './pages/Login';
import Profile from './pages/Profile';


  function LayoutWrapper() {
  const location = useLocation();
  
  // Provjeravamo je li trenutna putanja '/register'
  const isAuthPage = location.pathname === '/register' || location.pathname === '/login';

  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  useEffect(() => {
    // Ova funkcija prati da li je korisnik ulogovan ili ne
    const unsubscribe = auth.onAuthStateChanged((currentUser) => {
      setUser(currentUser);
      setLoading(false); // Prestane s učitavanjem čim Firebase odgovori
    });

    return () => unsubscribe(); // Očisti pretplatu
  }, []);

  if (loading) {
    return <div>Učitavanje...</div>; 
  }

  return (
    <div className="App">

      {/* navbar se prikazuje samo ako NISMO na auth stranici */}
      {!isAuthPage && <Navbar />}

        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/Dashboard" element={<Dashboard />} />
          <Route path="/MyWatchlist" element={<Watchlist />} />
          <Route path="/profile" element={<Profile />} />
          
          <Route path="/register" element={<Register />} /> 
          <Route path="/login" element={<Login />} />  
        </Routes>

        {/* Footer se prikazuje samo ako NISMO na auth stranici */}
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
