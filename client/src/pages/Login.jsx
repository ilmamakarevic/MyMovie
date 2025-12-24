import { auth } from '../firebase.js';
import { signInWithEmailAndPassword } from "firebase/auth";
import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import './LoginRegister.css';

const Login = () => {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const navigate = useNavigate();
    const [error, setError] = useState("");

    const handleLogin = async (e) => {
        e.preventDefault();
        setError("");
        try {
            const userCredential = await signInWithEmailAndPassword(auth, email, password);
            console.log("Korisnik prijavljen:", userCredential.user.uid);
            navigate("/");
        } catch (error) {
            console.error("Greška pri prijavi:", error.message);
            setError("Wrong email or password.");
        }
    };

    return(

        <div className='register-container'>
            <form form onSubmit={handleLogin}>
                <h1>Login</h1>
                      
                <label for="email">Email:</label>
                <input type="email" id="email" name="email" onChange={(e) => setEmail(e.target.value)} 
                    required />

                <label for="password">Password:</label>
                <input type='password' id="password" name="password" onChange={(e) => setPassword(e.target.value)} 
                    required/>

                {error && <p className="error-text">{error}</p>}

                <button type="register-button">LOGIN</button>


                <p style={{marginTop: '15px', color: 'black'}}>
                    Don't have an account? <Link to="/register" style={{color: '#e50914'}}>Make Account</Link>
                </p>
            </form>             

        </div>
    );

}

export default Login;