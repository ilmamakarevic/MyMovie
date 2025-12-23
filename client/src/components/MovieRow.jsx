import React, { useState, useEffect, useRef } from 'react'; 
import { FaChevronLeft, FaChevronRight, FaTimes, FaStar } from 'react-icons/fa'; 
import './MovieRow.css';

const MovieRow = ({ title, url }) => {
    const [movies, setMovies] = useState([]);
    const [selectedMovie, setSelectedMovie] = useState(null); 
    const [userRating, setUserRating] = useState(0);
    const [hover, setHover] = useState(0);
    const rowRef = useRef(null);

    useEffect(() => {
        setUserRating(0);
    }, [selectedMovie]);

    useEffect(() => {
        const fetchMovies = async () => {
            try {
                const response = await fetch(url);
                const data = await response.json();
                setMovies(data);
            } catch (error) {
                console.error("Greška:", error);
            }
        };
        fetchMovies();
    }, [url]);

    const scroll = (direction) => {
        if (rowRef.current) {
            const { scrollLeft, clientWidth } = rowRef.current;
            const scrollTo = direction === 'left' ? scrollLeft - clientWidth : scrollLeft + clientWidth;
            rowRef.current.scrollTo({ left: scrollTo, behavior: 'smooth' });
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
                            onClick={() => setSelectedMovie(movie)} // Postavlja film na klik
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
                                alt={selectedMovie.title} 
                            />
                            <div className="modal-info">
                                <h1>{selectedMovie.title || selectedMovie.name}</h1>
                                {/* TMDB Originalni Rating */}
                                <p className="modal-rating">
                                    TMDB: ⭐ {selectedMovie.voteAverage ? selectedMovie.voteAverage.toFixed(1) : "N/A"} / 10
                                </p>

                                {/* TVOJ INTERAKTIVNI RATING */}
                                
                                <p className="modal-overview">{selectedMovie.overview}</p>
                                <button className="play-button">Play</button>
                            </div>
                        </div>
                    </div>
                </div>

                
            )}
        </div>
    );
};

export default MovieRow;