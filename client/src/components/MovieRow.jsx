import React, { useState, useEffect, useRef } from 'react'; 
import { FaChevronLeft, FaChevronRight, FaTimes, FaStar } from 'react-icons/fa'; 
import { auth } from "../firebase";
import './MovieRow.css';

const MovieRow = ({ title, url }) => {
    const [movies, setMovies] = useState([]);
    const [selectedMovie, setSelectedMovie] = useState(null); 
    const [isOnWatchlist, setIsOnWatchlist] = useState(false);
    const [userRating, setUserRating] = useState(0);
    const [hover, setHover] = useState(0);
    const rowRef = useRef(null);

    useEffect(() => {
        setUserRating(0);
        setIsOnWatchlist(false);
    }, [selectedMovie]);

    useEffect(() => {
        const fetchMovies = () => {
            // novi XMLHttpRequest objekt (AJAX)
            const xhr = new XMLHttpRequest();
            
            // GET metoda prema proslijedjenom URL-u
            xhr.open("GET", url, true);

            //  
            xhr.onload = function () {
                if (xhr.status >= 200 && xhr.status < 300) {
                    try {
                        const data = JSON.parse(xhr.responseText);
                        setMovies(data);
                    } catch (error) {
                        console.error("Greška pri parsiranju JSON-a:", error);
                    }
                } else {
                    console.error("Server je vratio grešku:", xhr.statusText);
                }
            };

            // u slucaju mrezne greške
            xhr.onerror = function () {
                console.error("Mrežna greška pri AJAX pozivu.");
            };

            // Slanje zahtjeva
            xhr.send();
        };

        fetchMovies();
    }, [url]);

    // Provjeravanje da li je film/serija na watchlisti
    const checkWatchlistStatus = async (movie) => {
    const user = auth.currentUser;
    if (!user) return; // Ako niko nije prijavljen, ne provjerava ništa

    try {
        const type = movie.title ? 'movie' : 'tv';
        const tmdbId = movie.tmdbId || movie.id;
        
        // Dodajemo userId u query parametre kako bi backend znao koga provjerava
        const response = await fetch(
            `http://localhost:5081/api/Watchlist/check/${tmdbId}?type=${type}&userId=${user.uid}`
        );
        
        if (response.ok) {
            const isOnList = await response.json();
            setIsOnWatchlist(isOnList);
        }
    } catch (error) {
        console.error("Error checking watchlist:", error);
        setIsOnWatchlist(false);
    }
};

    const handleMovieClick = async (movie) => {
        setSelectedMovie(movie);
        await checkWatchlistStatus(movie);
    };

    const scroll = (direction) => {
        if (rowRef.current) {
            const { scrollLeft, clientWidth } = rowRef.current;
            const scrollTo = direction === 'left' ? scrollLeft - clientWidth : scrollLeft + clientWidth;
            rowRef.current.scrollTo({ left: scrollTo, behavior: 'smooth' });
        }
    };

    const addToWatchlist = async () => {
    if (!selectedMovie) return;

    // catch trenutno prijavljenog korisnika
    const user = auth.currentUser;

    if (!user) {
        alert("Morate biti prijavljeni da biste dodali film na listu!");
        return;
    }

    try {
        const type = selectedMovie.title ? 'movie' : 'tv';
        const tmdbId = selectedMovie.tmdbId || selectedMovie.id;

        const response = await fetch('http://localhost:5081/api/Watchlist', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify({
                tmdbId: tmdbId,
                firebaseUserId: user.uid, // salje UID korisnika
                title: selectedMovie.title || null,
                name: selectedMovie.name || null,
                posterPath: selectedMovie.posterPath,
                type: type,
                rating: selectedMovie.voteAverage,
                overview: selectedMovie.overview
            }),
        });

        if (response.ok) {
            setIsOnWatchlist(true);
        } else {
            const errorText = await response.text();
            alert("Greška pri dodavanju: " + errorText);
        }
    } catch (error) {
        console.error("Network error:", error);
        alert("Greška pri povezivanju s poslužiteljem.");
    }
};

    return (
        <div className="movie-row">
            <h2>{title}</h2>
            <div className="row-wrapper">
                <button className="handle handle-left" onClick={() => scroll('left')}>
                    <FaChevronLeft />
                </button>

                <div className="movie-list" ref={rowRef}>
                    {movies.map(movie => (
                        <div 
                            key={movie.id} 
                            className="movie-card" 
                            onClick={() => handleMovieClick(movie)}
                        >
                            <img 
                                src={`https://image.tmdb.org/t/p/w300${movie.posterPath}`} 
                                alt={movie.title || movie.name} 
                            />
                        </div>
                    ))}
                </div>

                <button className="handle handle-right" onClick={() => scroll('right')}>
                    <FaChevronRight />
                </button>
            </div>

            {/* MODAL / POPUP */}
            {selectedMovie && (
                <div className="modal-overlay" onClick={() => setSelectedMovie(null)}>
                    <div className="modal-content" onClick={(e) => e.stopPropagation()}>
                        <button className="close-button" onClick={() => setSelectedMovie(null)}>
                            <FaTimes />
                        </button>
                        
                        <div className="modal-body">
                            <img 
                                src={`https://image.tmdb.org/t/p/w500${selectedMovie.posterPath}`} 
                                alt={selectedMovie.title || selectedMovie.name} 
                            />
                            <div className="modal-info">
                                <h1>{selectedMovie.title || selectedMovie.name}</h1>
                                
                                <p className="modal-rating">
                                    TMDB: ⭐ {selectedMovie.voteAverage ? selectedMovie.voteAverage.toFixed(1) : "N/A"} / 10
                                </p>

                                <p className="modal-overview">{selectedMovie.overview}</p>
                                
                                <div className='modal-buttons'>
                                    <button 
                                        className={`play-button add-to-watchlist ${isOnWatchlist ? 'on-watchlist' : ''}`}
                                        onClick={addToWatchlist}
                                        disabled={isOnWatchlist}
                                    >
                                        {isOnWatchlist ? '✓ Added to watchlist' : '+ Add to watchlist'}
                                    </button>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            )}
        </div>
    );
};

export default MovieRow;