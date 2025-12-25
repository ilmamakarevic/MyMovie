import React, { useState, useEffect } from 'react';
import { auth } from "../firebase";
import { FaTimes } from 'react-icons/fa'; // Dodaj import za ikonu
import './BrowseAll.css';

const BrowseAll = ({ type }) => {
    const [items, setItems] = useState([]);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);
    
    // State za modal (kao u MovieRow)
    const [selectedMovie, setSelectedMovie] = useState(null);
    const [isOnWatchlist, setIsOnWatchlist] = useState(false);

    const API_URL = type === 'movie' 
        ? `http://localhost:5081/api/Movies/popular?page=${page}` 
        : `http://localhost:5081/api/TvShows/popular?page=${page}`;

    // Resetiraj stranicu na 1 ako se promijeni tip (Movies -> TV Shows)
    useEffect(() => {
        setPage(1);
    }, [type]);

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await fetch(API_URL);
                const data = await response.json();
                // Postavljamo nove rezultate (mijenjamo stare)
                setItems(data);
                // Skrolaj na vrh stranice kad se učitaju novi podaci
                window.scrollTo({ top: 0, behavior: 'smooth' });
            } catch (error) {
                console.error("Greška pri dohvaćanju:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [type, page]); // Ovisnosti: type i page

    // Funkcije za Watchlist (Kopirano iz MovieRow.jsx)
    const checkWatchlistStatus = async (item) => {
        const user = auth.currentUser;
        if (!user) return;
        try {
            const itemType = item.title ? 'movie' : 'tv';
            const tmdbId = item.tmdbId || item.id;
            const response = await fetch(
                `http://localhost:5081/api/Watchlist/check/${tmdbId}?type=${itemType}&userId=${user.uid}`
            );
            if (response.ok) {
                const isOnList = await response.json();
                setIsOnWatchlist(isOnList);
            }
        } catch (error) {
            console.error("Error checking watchlist:", error);
        }
    };

    const handleItemClick = async (item) => {
        setSelectedMovie(item);
        setIsOnWatchlist(false); // Reset prije provjere
        await checkWatchlistStatus(item);
    };

    const addToWatchlist = async () => {
        if (!selectedMovie) return;
        const user = auth.currentUser;
        if (!user) {
            alert("Morate biti prijavljeni!");
            return;
        }

        try {
            const itemType = selectedMovie.title ? 'movie' : 'tv';
            const tmdbId = selectedMovie.tmdbId || selectedMovie.id;

            const response = await fetch('http://localhost:5081/api/Watchlist', {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({
                    tmdbId: tmdbId,
                    firebaseUserId: user.uid,
                    title: selectedMovie.title || null,
                    name: selectedMovie.name || null,
                    posterPath: selectedMovie.posterPath,
                    type: itemType,
                    rating: selectedMovie.voteAverage,
                    overview: selectedMovie.overview
                }),
            });

            if (response.ok) {
                setIsOnWatchlist(true);
            }
        } catch (error) {
            console.error("Network error:", error);
        }
    };

    return (
        <div className="browse-container">
            <h1>{type === 'movie' ? 'All Movies' : 'All TV Shows'}</h1>
            
            {loading ? (
                <div className="loading">Loading...</div>
            ) : (
                <div className="items-grid">
                    {items.map(item => (
                        <div 
                            key={item.id} 
                            className="movie-card-simple" 
                            onClick={() => handleItemClick(item)}
                        >
                             <img src={`https://image.tmdb.org/t/p/w300${item.posterPath}`} alt={item.title || item.name}  />
                             <p>{item.title || item.name}</p>
                        </div>
                    ))}
                </div>
            )}

            <div className="pagination">
                <button disabled={page === 1} onClick={() => setPage(p => p - 1)}>Previous</button>
                <span>Page {page}</span>
                <button onClick={() => setPage(p => p + 1)}>Next</button>
            </div>

            {/* MODAL POPUP (Ista logika kao u MovieRow.jsx) */}
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

export default BrowseAll;