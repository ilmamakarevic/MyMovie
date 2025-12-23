import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Footer from './components/Footer/Footer'
import Dashboard from './pages/Dashboard';
import Watchlist from './pages/Watchlist';



function App() {
  return (
    <div className="App">
      <Router>

        <Navbar />

        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/Dashboard" element={<Dashboard />} />
          <Route path="/MyWatchlist" element={<Watchlist />} />
        </Routes>

        <Footer />
      </Router>
        
        
    </div>
  );
}

export default App;
