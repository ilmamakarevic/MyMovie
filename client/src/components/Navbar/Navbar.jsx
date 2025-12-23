import React from 'react';
import './Navbar.css';
import { Link, NavLink } from 'react-router-dom';
import { RiAccountPinCircleFill } from "react-icons/ri";

const Navbar = () => {
  return (
    <nav className='navbar'>
      <ul className='navbar-list'>
         <li className="heading-name">MyMovie</li>

        <li className='navbar-item'>
          <NavLink to="/dashboard">Dashboard</NavLink>
        </li>

        {/* MOVIES */}
        <li className='navbar-item'>
          <button type="button">Movies</button>
          <ul className="sub-menu">
            <li><Link to="/movies/popular">Popular</Link></li>
            <li><Link to="/movies/trending">Trending</Link></li>
            <li><Link to="/movies/top-rated">Top rated</Link></li>
            <li><Link to="/movies/upcoming">Coming soon</Link></li>
          </ul>
        </li>

        {/* TV SHOWS */}
        <li className='navbar-item'>
          <button type="button">TV Shows</button>
          <ul className="sub-menu">
            <li><Link to="/tv/popular">Popular</Link></li>
            <li><Link to="/tv/trending">Trending</Link></li>
            <li><Link to="/tv/top-rated">Top rated</Link></li>
            <li><Link to="/tv/upcoming">Coming soon</Link></li>
          </ul>
        </li>

        <li className='navbar-item'><Link to="/MyWatchlist">My watchlist</Link></li>
      </ul>

      <div className='navbar-right'>
        <input type="text" placeholder="Search" />
        <RiAccountPinCircleFill className="profile-icon" />
      </div>
    </nav>
  );
};

export default Navbar;
