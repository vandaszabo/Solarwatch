import { React, useEffect } from 'react';
import SolarWatch from '../SolarWatch/SolarWatch';
import './Home.css';

const Home = () => {

  const userName = localStorage.getItem('userName');

  //Get user Role, add to localStorage
  useEffect(() => {
    const getUserRole = async () => {
        if (userName) {
            try {
                const response = await fetch('/User/Roles', {
                    method: 'POST',
                    headers: {'Content-Type': 'application/json',},
                    body: JSON.stringify({ userName })
                });
                if (!response.ok) {
                    throw new Error(`Error: ${response.status} - ${response.statusText}`);
                }
                const data = await response.json();
                localStorage.setItem('role', data);

            } catch (error) {
                console.error('Error getting user role:', error.message);
            }
        }
    };
    getUserRole();
}, []);

  return (
    <div className='home-container'>
        <h1>Welcome to SolarWatch</h1>
        <h5>You can search for sunrise/sunset times of cities.</h5>
        <div>
          <SolarWatch />
        </div>
    </div>
  )
}

export default Home;