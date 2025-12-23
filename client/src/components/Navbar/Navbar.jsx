import React from 'react';
import './Navbar.css';
import { Link } from 'react-router-dom';
import Dashboard from '../../pages/Dashboard';
import { RiAccountPinCircleFill } from "react-icons/ri";

const Navbar = () => {

    return(

        <nav className='navbar'>

            

            <ul className='navbar-list'>

                <h1 className='heading-name'>MyMovie</h1>
                <li className='navbar-item'>
                    <Link to="/dashboard">Dashboard</Link>
                </li>
                <li className='navbar-item'>
                    <button aria-expanded="false">Movies</button>
                    <ul className="sub-menu" aria-label="Apps">
                        <li><a href="#">Popular</a></li>
                        <li><a href="#">Trending</a></li>
                        <li><a href="#">Top rated</a></li>
                        <li><a href="#">Coming soon</a></li>
                    </ul>
                </li>

                <li className='navbar-item'>
                    <button aria-expanded="false">TV Shows</button>
                    <ul className="sub-menu" aria-label="Apps">
                        <li><a href="#">Popular</a></li>
                        <li><a href="#">Trending</a></li>
                        <li><a href="#">Top rated</a></li>
                        <li><a href="#">Coming soon</a></li>
                    </ul>
                </li>

                <li className='navbar-item'>My watchlist</li>
            </ul>

            <div className='navbar-right'>
                <input type="text" placeholder="Search"></input>


                <RiAccountPinCircleFill className="profile-icon" />
                
            </div>

        </nav>
        

    );
}

export default Navbar;