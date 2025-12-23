import React, { useState, useEffect, useRef } from 'react'; 
import { FaChevronLeft, FaChevronRight } from 'react-icons/fa'; 
import './MovieRow.css';

const MovieRow = ({ title, url }) => {
    const [movies, setMovies] = useState([]);
    const rowRef = useRef(null); // Referenca na listu filmova

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

    // Funkcija za skrolovanje
    const scroll = (direction) => {
        if (rowRef.current) {
            const { scrollLeft, clientWidth } = rowRef.current;
            const scrollTo = direction === 'left' 
                ? scrollLeft - clientWidth 
                : scrollLeft + clientWidth;
            
            rowRef.current.scrollTo({ left: scrollTo, behavior: 'smooth' });
        }
    };

    return (
        <div className="movie-row">
            <h2>{title}</h2>
            <div className="row-wrapper">
                {/* Lijeva strelica */}
                <button className="handle handle-left" onClick={() => scroll('left')}>
                    <FaChevronLeft />
                </button>

                <div className="movie-list" ref={rowRef}>
                    {movies.map(movie => (
                        <div key={movie.id} className="movie-card">
                            <img 
                                src={`https://image.tmdb.org/t/p/w300${movie.posterPath}`} 
                                alt={movie.title} 
                            />
                        </div>
                    ))}
                </div>

                {/* Desna strelica */}
                <button className="handle handle-right" onClick={() => scroll('right')}>
                    <FaChevronRight />
                </button>
            </div>
        </div>
    );
};

export default MovieRow;