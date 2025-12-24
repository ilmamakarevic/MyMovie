import React, { useState, useEffect } from 'react';
import { auth } from "../firebase"; // Putanja do tvog firebase.js
import './Watchlist.css';

const Watchlist = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        // Pratimo promjenu stanja prijave da bismo znali UID korisnika
        const unsubscribe = auth.onAuthStateChanged((user) => {
            if (user) {
                fetchWatchlist(user.uid); // Šaljemo UID funkciji
            } else {
                setLoading(false);
            }
        });

        return () => unsubscribe();
    }, []);

    const fetchWatchlist = async (userId) => {
        try {
            setLoading(true);
            // DODALI SMO userId u URL (ovo je ključno za tvoj backend)
            const response = await fetch(`http://localhost:5081/api/Watchlist?userId=${userId}`);
            
            if (response.status === 204) {
                setMovies([]);
                return;
            }

            if (!response.ok) throw new Error("Server error");

            const data = await response.json();
            setMovies(Array.isArray(data) ? data : []);
        } catch (error) {
            console.error("Greška pri dohvaćanju Watchliste:", error);
            setMovies([]);
        } finally {
            setLoading(false);
        }
    };

    const removeFromWatchlist = async (id, title) => {
        if (!window.confirm(`Da li sigurno želite ukloniti "${title}"?`)) return;

        try {
            const response = await fetch(`http://localhost:5081/api/Watchlist/${id}`, {
                method: 'DELETE'
            });

            if (response.ok || response.status === 204) {
                setMovies(movies.filter(m => m.id !== id));
            }
        } catch (error) {
            console.error("Error removing:", error);
        }
    };

    if (loading) return <div className="watchlist-page"><h1>Učitavanje...</h1></div>;

    // Ako korisnik uopće nije logiran
    if (!auth.currentUser) return <div className="watchlist-page"><h1>Molimo prijavite se.</h1></div>;

    return (
        <div className="watchlist-page">
            <h1>My Watchlist</h1>
            {movies.length > 0 ? (
                <div className="watchlist-grid">
                    {movies.map(movie => (
                        <div key={movie.id} className="movie-card">
                            <img 
                                src={`https://image.tmdb.org/t/p/w300${movie.posterPath}`} 
                                alt={movie.title || movie.name} 
                            />
                            <div className="movie-info">
                                <p className="movie-title">{movie.title || movie.name}</p>
                                <button 
                                    className="remove-button"
                                    onClick={() => removeFromWatchlist(movie.id, movie.title || movie.name)}
                                >
                                    ✕ Remove
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <p>Your watchlist is empty.</p>
            )}
        </div>
    );
};

export default Watchlist;