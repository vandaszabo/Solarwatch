import React, { useState, useEffect } from 'react';
import { useNavigate } from 'react-router-dom';
import LoginForm from '../../Components/Forms/LoginForm';
import './Login.css';

const Login = () => {
    const initialUserData = {
        email: '',
        password: '',
    };

    const [userData, setUserData] = useState(initialUserData);
    const [responseState, setResponseState] = useState('');
    const navigate = useNavigate();

    const handleInputChange = (e) => {
        setUserData({ ...userData, [e.target.name]: e.target.value });
    };

    //LogIn User
    const loginUser = async (user) => {
        const response = await fetch('/Auth/Login', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user),
        });

        const data = await response.json();

        if (response.ok) {
            localStorage.setItem('token', data.token);
            localStorage.setItem('userName', data.userName);
            localStorage.setItem('email', data.email);
            setResponseState(data);
        } else {
            console.error('Login failed:', data);
            setResponseState(data);
            console.log('errordata: ', data);
        }
    };

    //Submit handler
    const handleSubmit = async (e) => {
        e.preventDefault();
        try {
            await loginUser(userData);
        } catch (error) {
            console.error('Login failed:', error);
            setUserData(initialUserData);
        }
    };

    //Redirect to Registration
    const handleRegister = (e) => {
        e.preventDefault();
        navigate('/Registration');
    };

    //Navigate to Home Page
    useEffect(() => {
        if (responseState && !responseState.hasOwnProperty('Bad credentials')) {
            navigate('/');
        }
    }, [responseState, navigate]);

    return (
        <div className='login'>
            {responseState === '' ? (
                <div className='auth-panel'>
                    <LoginForm
                        email={userData.email}
                        password={userData.password}
                        handleInputChange={handleInputChange}
                        handleSubmit={handleSubmit}
                    />
                    <div className='registerBtn'>
                        <button onClick={handleRegister}>Register here</button>
                    </div>
                </div>
            ) : responseState.hasOwnProperty('Bad credentials') ? (
                <div className='auth-panel'>
                    <LoginForm
                        email={userData.email}
                        password={userData.password}
                        handleInputChange={handleInputChange}
                        handleSubmit={handleSubmit}
                        errorMessage={responseState['Bad credentials'][0]}
                    />
                    <div className='registerBtn'>
                        <button onClick={handleRegister}>Register here</button>
                    </div>
                </div>
            ) : null}
        </div>
    );
};

export default Login;
