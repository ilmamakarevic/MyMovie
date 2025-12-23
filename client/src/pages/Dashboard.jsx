import './Dashboard.css';
import MovieRow from '../components/MovieRow';

const Dashboard = () => {
    const BASE_URL = "http://localhost:5081/api/movies";
    const TV_SHOWS_URL = "http://localhost:5081/api/TvShows";

    return(

        <div className='dashboard-container'>
        
            <MovieRow title="Popular Movies" url={`${BASE_URL}/popular`} />
            
            <MovieRow 
                title="Popular TV Shows" 
                url={`${TV_SHOWS_URL}/popular`} 
            />

        </div>

    );


}

export default Dashboard;