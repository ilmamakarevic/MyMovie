import './Dashboard.css';
import MovieRow from '../components/MovieRow';


const Dashboard = () => {
    const BASE_URL = "http://localhost:5081/api/movies";

    return(

        <div className='dashboard-container'>
        
            <MovieRow title="Popular Movies" url={`${BASE_URL}/popular`} />

            {/* Ovdje možeš dodati ostale sekcije kada napraviš rute u .NET-u */}

            {/*<MovieRow title="Trending Movies" url={`${BASE_URL}/trending`} />*/}
        </div>

    );


}

export default Dashboard;