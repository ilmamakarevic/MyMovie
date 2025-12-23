import React, { useState, useEffect, useRef } from 'react';
import './Navbar.css';
import { Link, NavLink, useNavigate } from 'react-router-dom';
import { RiAccountPinCircleFill } from "react-icons/ri";
import { FaSearch, FaTimes } from "react-icons/fa";

const Navbar = () => {
  const [searchQuery, setSearchQuery] = useState('');
  const [searchResults, setSearchResults] = useState([]);
  const [isSearching, setIsSearching] = useState(false);
  const [showResults, setShowResults] = useState(false);
  const searchRef = useRef(null);
  const navigate = useNavigate();

  // Debounce za search - čeka 500ms nakon što korisnik prestane kucati
  useEffect(() => {
    if (searchQuery.trim().length < 2) {
      setSearchResults([]);
      setShowResults(false);
      return;
    }

    setIsSearching(true);
    const timeoutId = setTimeout(() => {
      searchMovies(searchQuery);
    }, 500);

    return () => clearTimeout(timeoutId);
  }, [searchQuery]);

  // Zatvori rezultate kada se klikne van search komponente
  useEffect(() => {
    const handleClickOutside = (event) => {
      if (searchRef.current && !searchRef.current.contains(event.target)) {
        setShowResults(false);
      }
    };

    document.addEventListener('mousedown', handleClickOutside);
    return () => document.removeEventListener('mousedown', handleClickOutside);
  }, []);

  const searchMovies = async (query) => {
    try {
      const [moviesResponse, tvShowsResponse] = await Promise.all([
        fetch(`http://localhost:5081/api/Movies/search?query=${encodeURIComponent(query)}`),
        fetch(`http://localhost:5081/api/TvShows/search?query=${encodeURIComponent(query)}`)
      ]);

      const movies = moviesResponse.ok ? await moviesResponse.json() : [];
      const tvShows = tvShowsResponse.ok ? await tvShowsResponse.json() : [];

      // Kombiniraj rezultate i ograniči na prvih 8
      const combined = [
        ...movies.slice(0, 4).map(item => ({ ...item, type: 'movie' })),
        ...tvShows.slice(0, 4).map(item => ({ ...item, type: 'tv' }))
      ];

      setSearchResults(combined);
      setShowResults(combined.length > 0);
    } catch (error) {
      console.error('Search error:', error);
      setSearchResults([]);
    } finally {
      setIsSearching(false);
    }
  };

  const handleResultClick = (item) => {
    setSearchQuery('');
    setShowResults(false);
    setSearchResults([]);
    // Možeš navigirati na detail stranicu ili otvoriti modal
    // Za sada ćemo samo zatvoriti rezultate
  };

  const clearSearch = () => {
    setSearchQuery('');
    setSearchResults([]);
    setShowResults(false);
  };

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
            <li><Link to="/movies/top-rated">Top rated</Link></li>
            <li><Link to="/movies/upcoming">Coming soon</Link></li>
          </ul>
        </li>

        {/* TV SHOWS */}
        <li className='navbar-item'>
          <button type="button">TV Shows</button>
          <ul className="sub-menu">
            <li><Link to="/tv/popular">Popular</Link></li>
            <li><Link to="/tv/top-rated">Top rated</Link></li>
            <li><Link to="/tv/upcoming">Coming soon</Link></li>
          </ul>
        </li>

        <li className='navbar-item'><Link to="/MyWatchlist">My watchlist</Link></li>
      </ul>

      <div className='navbar-right'>
        <div className='search-container' ref={searchRef}>
          <div className='search-input-wrapper'>
            <FaSearch className='search-icon' />
            <input 
              type="text" 
              placeholder="Search movies & TV shows..." 
              value={searchQuery}
              onChange={(e) => setSearchQuery(e.target.value)}
              onFocus={() => searchResults.length > 0 && setShowResults(true)}
            />
            {searchQuery && (
              <FaTimes className='clear-icon' onClick={clearSearch} />
            )}
          </div>

          {/* Search Results Dropdown */}
          {showResults && (
            <div className='search-results'>
              {isSearching ? (
                <div className='search-loading'>Pretraga...</div>
              ) : searchResults.length > 0 ? (
                <>
                  {searchResults.map((item) => (
                    <div 
                      key={`${item.type}-${item.id}`} 
                      className='search-result-item'
                      onClick={() => handleResultClick(item)}
                    >
                      <img 
                        src={item.posterPath 
                          ? `https://image.tmdb.org/t/p/w92${item.posterPath}` 
                          : '/placeholder-poster.png'
                        }
                        alt={item.title || item.name}
                        className='result-poster'
                      />
                      <div className='result-info'>
                        <div className='result-title'>
                          {item.title || item.name}
                        </div>
                        <div className='result-meta'>
                          <span className='result-type'>
                            {item.type === 'movie' ? '🎬 Film' : '📺 Serija'}
                          </span>
                          {item.voteAverage && (
                            <span className='result-rating'>
                              ⭐ {item.voteAverage.toFixed(1)}
                            </span>
                          )}
                        </div>
                      </div>
                    </div>
                  ))}
                </>
              ) : (
                <div className='no-results'>Nema rezultata</div>
              )}
            </div>
          )}
        </div>

        <RiAccountPinCircleFill className="profile-icon" />
      </div>
    </nav>
  );
};

export default Navbar;