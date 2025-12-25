import React, { useState, useEffect } from 'react';
import { useParams, useNavigate, useLocation } from 'react-router-dom';
import { auth } from "../firebase";
import { FaStar, FaArrowLeft, FaPlay } from 'react-icons/fa';
import './SingleMovie.css';

const SingleMovie = () => {
    const { id, type } = useParams();
    const navigate = useNavigate();
    const location = useLocation();

    const [item, setItem] = useState(location.state?.movieData || null);
    const [loading, setLoading] = useState(!location.state?.movieData);
    const [isOnWatchlist, setIsOnWatchlist] = useState(false);

    useEffect(() => {
        const fetchDetails = async () => {
            try {
                // Uzimam token jer su controlleri sada mozda zakljucani
                const user = auth.currentUser;
                const token = user ? await user.getIdToken() : null;

                if (!item) {
                    setLoading(true);
                    const baseUrl = "http://localhost:5081/api";
                    const endpoint = type === 'movie' 
                        ? `${baseUrl}/Movies/tmdb/${id}` 
                        : `${baseUrl}/TvShows/tmdb/${id}`;
                    
                    const response = await fetch(endpoint, {
                        headers: {
                            'Authorization': token ? `Bearer ${token}` : ''
                        }
                    });
                    
                    if (!response.ok) throw new Error("Neuspješno učitavanje detalja");
                    
                    const data = await response.json();
                    setItem(data);
                    if (user) checkWatchlistStatus(data, user.uid);
                } else {
                    if (user) checkWatchlistStatus(item, user.uid);
                }
            } catch (error) {
                console.error("Greška:", error);
            } finally {
                setLoading(false);
            }
        };

        fetchDetails();
    }, [id, type, item]);

    const checkWatchlistStatus = async (movieData, userId) => {
        try {
            const user = auth.currentUser;
            if (!user) return;

            const token = await user.getIdToken();
            const tmdbId = movieData.tmdbId || movieData.id;

            const response = await fetch(
                `http://localhost:5081/api/Watchlist/check/${tmdbId}?type=${type}&userId=${userId}`,
                {
                    headers: {
                        'Authorization': `Bearer ${token}`
                    }
                }
            );
            
            if (response.ok) {
                const exists = await response.json();
                setIsOnWatchlist(exists);
            }
        } catch (error) {
            console.error("Greška pri provjeri watchliste:", error);
        }
    };

    const addToWatchlist = async () => {
        // provjera ulogovanog korisnika
        const user = auth.currentUser;
        if (!user) {
            alert("Morate biti prijavljeni!");
            return;
        }

        try {
            // Dobavljanje tokena za POST zahtjev
            const token = await user.getIdToken();

            const watchlistItem = {
                tmdbId: item.tmdbId || item.id,
                firebaseUserId: user.uid,
                title: type === 'movie' ? item.title : null,
                name: type === 'tv' ? item.name : null,
                posterPath: item.posterPath,
                type: type,
                rating: item.voteAverage,
                overview: item.overview
            };

            const response = await fetch('http://localhost:5081/api/Watchlist', {
                method: 'POST',
                headers: { 
                    'Content-Type': 'application/json',
                    'Authorization': `Bearer ${token}` //salje token backendu
                },
                body: JSON.stringify(watchlistItem),
            });

            if (response.ok) {
                setIsOnWatchlist(true);
            } else {
                alert("Greška pri dodavanju na server.");
            }
        } catch (error) {
            console.error("Greška:", error);
        }
    };

    if (loading) return <div className="loading-screen">Loading details...</div>;
    if (!item) return <div className="error-screen">Data not found.</div>;

    return (
        <div className="single-movie-container">
            <button className="back-btn" onClick={() => navigate(-1)}>
                <FaArrowLeft /> Back
            </button>

            <div className="content-wrapper">
                <div className="left-column">
                    <img 
                        src={`https://image.tmdb.org/t/p/w500${item.posterPath}`} 
                        alt={item.title || item.name} 
                        className="main-poster"
                    />
                </div>

                <div className="right-column">
                    <h1 className="main-title">{item.title || item.name}</h1>
                    
                    <div className="stats-row">
                        <span className="rating-tag"><FaStar /> {item.voteAverage?.toFixed(1)}</span>
                        <span className="year-tag">
                            {type === 'movie' ? item.releaseDate?.split('-')[0] : item.firstAirDate?.split('-')[0]}
                        </span>
                        <span className="type-tag">{type.toUpperCase()}</span>
                    </div>

                    <p className="overview-text">{item.overview}</p>

                    <div className="button-group">
                        
                        <button 
                            className={`watchlist-button ${isOnWatchlist ? 'on-list' : ''}`}
                            onClick={addToWatchlist}
                            disabled={isOnWatchlist}
                        >
                            {isOnWatchlist ? '✓ Added to watchlist' : '+ Add to watchlist'}
                        </button>
                    </div>
                </div>
            </div>
        </div>
    );
};

export default SingleMovie;