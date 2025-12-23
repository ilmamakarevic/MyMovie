import React, { useState, useEffect } from 'react';
import './Watchlist.css';

const Watchlist = () => {
    const [movies, setMovies] = useState([]);
    const [loading, setLoading] = useState(true);

    useEffect(() => {
        fetchWatchlist();
    }, []);

    const fetchWatchlist = async () => {
        try {
            setLoading(true);
            const response = await fetch('http://localhost:5081/api/Watchlist');
            
            // Provjera: ako je status 204 (No Content) ili odgovor prazan
            if (response.status === 204) {
                setMovies([]);
                setLoading(false);
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
        if (!window.confirm(`Da li sigurno želite ukloniti "${title || 'ovaj item'}" sa watchliste?`)) {
            return;
        }

        try {
            const response = await fetch(`http://localhost:5081/api/Watchlist/${id}`, {
                method: 'DELETE'
            });

            if (response.ok || response.status === 204) {
                // Ukloni iz state-a bez ponovnog fetch-a
                setMovies(movies.filter(m => m.id !== id));
                alert('✓ Uklonjeno sa watchliste!');
            } else {
                alert('Greška pri brisanju.');
            }
        } catch (error) {
            console.error("Error removing from watchlist:", error);
            alert('Greška pri brisanju sa watchliste');
        }
    };

    if (loading) {
        return (
            <div className="watchlist-page">
                <h1>My Watchlist</h1>
                <p>Učitavanje...</p>
            </div>
        );
    }

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
                                {movie.rating && (
                                    <p className="movie-rating">⭐ {movie.rating.toFixed(1)}</p>
                                )}
                                <p className="movie-date">
                                    Dodano: {new Date(movie.addedDate).toLocaleDateString('hr-HR')}
                                </p>
                                <button 
                                    className="remove-button"
                                    onClick={() => removeFromWatchlist(movie.id, movie.title || movie.name)}
                                >
                                    ✕ Ukloni
                                </button>
                            </div>
                        </div>
                    ))}
                </div>
            ) : (
                <div className="empty-watchlist">
                    <p>Vaša watchlist je prazna.</p>
                    <p>Dodajte filmove i serije da ih pratite!</p>
                </div>
            )}
        </div>
    );
};

export default Watchlist;