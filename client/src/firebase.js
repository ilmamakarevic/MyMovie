// Import the functions you need from the SDKs you need
import { initializeApp } from "firebase/app";
import { getAuth } from "firebase/auth"; 
import { getAnalytics } from "firebase/analytics";
// TODO: Add SDKs for Firebase products that you want to use
// https://firebase.google.com/docs/web/setup#available-libraries

// Your web app's Firebase configuration
// For Firebase JS SDK v7.20.0 and later, measurementId is optional
const firebaseConfig = {
  apiKey: "AIzaSyAgR-EEoP2oHLh5vAXJAXrLh9rvQ3MOZCE",
  authDomain: "mymovie-16143.firebaseapp.com",
  projectId: "mymovie-16143",
  storageBucket: "mymovie-16143.firebasestorage.app",
  messagingSenderId: "858121873226",
  appId: "1:858121873226:web:69370ae1d591b50bb243b3",
  measurementId: "G-S7MQL6M7DF"
};

// Initialize Firebase
const app = initializeApp(firebaseConfig);
const analytics = getAnalytics(app);

export const auth = getAuth(app);