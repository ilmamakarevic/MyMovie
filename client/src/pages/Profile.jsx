import React from 'react';
import { auth } from '../firebase';
import { useNavigate } from 'react-router-dom';
import './Profile.css';

const Profile = () => {
    const user = auth.currentUser;
    const navigate = useNavigate();

    const handleLogout = async () => {
        try {
            await auth.signOut();
            navigate('/login'); // Preusmjeri na login nakon odjave
        } catch (error) {
            console.error("Greška pri odjavi:", error);
        }
    };

    if (!user) {
        return (
            <div className="profile-container">
                <h2>Niste prijavljeni.</h2>
                <button onClick={() => navigate('/login')}>Idi na Login</button>
            </div>
        );
    }

    return (
        <div className="profile-container">
            <h1>My Profile</h1>
            <div className="profile-card">
                <div className="profile-image">
                    {/* Generira inicijal korisnika ako nema sliku */}
                    {user.email ? user.email[0].toUpperCase() : 'U'}
                </div>
                <div className="profile-info">
                    <p><strong>Email:</strong> {user.email}</p>
                    <p><strong>User ID:</strong> <small>{user.uid}</small></p>
                    <p><strong>Account created:</strong> {new Date(user.metadata.creationTime).toLocaleDateString()}</p>
                </div>
                <button className="logout-button" onClick={handleLogout}>
                    Log out
                </button>
            </div>
        </div>
    );
};

export default Profile;