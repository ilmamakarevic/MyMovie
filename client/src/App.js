import logo from './logo.svg';
import './App.css';
import { BrowserRouter as Router, Routes, Route, Link } from 'react-router-dom';
import Navbar from './components/Navbar/Navbar';
import Dashboard from './pages/Dashboard';


function App() {
  return (
    <div className="App">
      <Router>

        <Navbar />

        <Routes>
          <Route path="/" element={<Dashboard />} />
          <Route path="/Dashboard" element={<Dashboard />} />
          {/* <Route path="/MyWatchlist" element={<Navbar />}></Route> */}
        </Routes>
      </Router>
        
        
    </div>
  );
}

export default App;
