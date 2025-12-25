import React, { useState, useEffect } from 'react';
import { auth } from "../firebase";
import MovieRow from '../components/MovieRow'; // Možeš koristiti MovieRow ili napraviti novi Grid
import './BrowseAll.css';

const BrowseAll = ({ type }) => {
    const [items, setItems] = useState([]);
    const [page, setPage] = useState(1);
    const [loading, setLoading] = useState(true);

    const API_URL = type === 'movie' 
        ? `http://localhost:5081/api/Movies/popular?page=${page}` 
        : `http://localhost:5081/api/TvShows/popular?page=${page}`;

    useEffect(() => {
        const fetchData = async () => {
            setLoading(true);
            try {
                const response = await fetch(API_URL);
                const data = await response.json();
                setItems(data);
            } catch (error) {
                console.error("Greška pri dohvaćanju:", error);
            } finally {
                setLoading(false);
            }
        };
        fetchData();
    }, [type, page]);

    return (
        <div className="browse-container">
            <h1>{type === 'movie' ? 'All Movies' : 'All TV Shows'}</h1>
            
            <div className="items-grid">
                {items.map(item => (
                    // Ovdje koristiš isti dizajn kartice kao u Watchlisti ili Dashboardu
                    <div key={item.id} className="movie-card-simple">
                         <img src={`https://image.tmdb.org/t/p/w300${item.posterPath}`} alt={item.title || item.name}  />
                         <p>{item.title || item.name}</p>
                    </div>
                ))}
            </div>

            <div className="pagination">
                <button disabled={page === 1} onClick={() => setPage(p => p - 1)}>Previous</button>
                <span>Page {page}</span>
                <button onClick={() => setPage(p => p + 1)}>Next</button>
            </div>
        </div>
    );
};

export default BrowseAll;