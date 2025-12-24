import { auth } from '../firebase.js';
import { createUserWithEmailAndPassword } from "firebase/auth";
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import './LoginRegister.css';




const Register = () => {

    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [name, setName] = useState("");
    const [error, setError] = useState("");

    const navigate = useNavigate();

    const handleRegister = async (e) => {
        e.preventDefault(); // Zaustavlja osvježavanje stranice
        setError("");
        try {
            const userCredential = await createUserWithEmailAndPassword(auth, email, password);
            console.log("Korisnik kreiran:", userCredential.user.uid);
            console.log("Successful registration");
            navigate("/"); // Preusmjerava korisnika na Dashboard nakon uspjeha
        } catch (error) {
            console.error("Greška pri registraciji:", error.message);
            setError("Wrong email or password.");
        }
    };
    
    return(

        <div className='register-container'>
            <form onSubmit={handleRegister}>
                <h1>Register</h1>
                <label for="name">Name and surname: </label>
                <input type="text" id="name" name="name" onChange={(e) => setName(e.target.value)}/>
                      
                <label for="email">Email:</label>
                <input type="email" id="email" name="email" onChange={(e) => setEmail(e.target.value)} required/>

                <label for="password">Password:</label>
                <input type='password' id="password" name="password" onChange={(e) => setPassword(e.target.value)} required />
                
                {error && <p className="error-text">{error}</p>}

                <button type="submit" >REGISTER</button>
            </form>             

        </div>
    );

}

export default Register;