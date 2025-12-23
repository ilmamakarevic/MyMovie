import React, { useState, useEffect, useRef } from 'react'; 
import { FaLinkedin, FaGithub } from "react-icons/fa";
import { Link, NavLink } from 'react-router-dom';
import './Footer.css';

const Footer = () => {

    return(
        <div className='footer-container'>
            <div className='links'>
                <a href="https://www.linkedin.com/in/ilma-makarevic-87336b2b4/"><FaLinkedin /></a>
                <a href="https://github.com/ilmamakarevic"><FaGithub/></a>
            </div>
            
            <p>Sarajevo, 2025</p>
            
        </div>
    );

}

export default Footer;