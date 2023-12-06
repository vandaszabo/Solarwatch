import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchForm from '../../Components/Forms/SearchForm.jsx';
import ResultForm from '../../Components/Forms/ResultForm.jsx';
import Loading from '../../Components/Loading.jsx';
import './SolarWatch.css';

const SolarWatch = () => {

    const [isLoading, setIsLoading] = useState(false);
    const [responseState, setResponseState] = useState('');
    const [cityName, setCityName] = useState('');
    const [isBack, setIsBack] = useState(false);

    const navigate = useNavigate();
    const token = localStorage.getItem('token');

    //City input setting
    const handleInputChange = (e) => {
        setCityName(e.target.value);
    };

    //SolarWatch fetch
    const getSunriseAndSunset = async (cityName) => {
        setIsLoading(true);
        try {
            const response = await fetch('/SolarWatch/GetSunriseAndSunset', {
                method: 'POST',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ cityName }),
            });

            const data = await response.json();
            setResponseState(data);

        } catch (error) {
            console.error('Error getting solarWatch:', error.message);
            setResponseState({ error: 'An error occurred while processing your request.' });
        } finally {
            setIsLoading(false);
        }
    };

    //handle Search
    const handleSubmit = async (e) => {
        e.preventDefault();
        if (token) {
            try {
                await getSunriseAndSunset(cityName);
            } catch (error) {
                console.error('Error getting solarWatch:', error.message);
                setResponseState({ error: 'An error occurred while processing your request.' });
            }
        } else {
            navigate('/Login');
        }
    };

    return (
        <div className='solar-watch'>
            {isLoading ? (
                <Loading/>
            ) : responseState === '' ? (
                <div>
                    <SearchForm
                        cityName={cityName}
                        handleInputChange={handleInputChange}
                        handleSubmit={handleSubmit}
                    />
                </div>

            ) : responseState.hasOwnProperty("error") ? (
                <div>
                    <div style={{ color: 'red' }}>{responseState.error}</div>
                    <SearchForm
                        cityName={cityName}
                        handleInputChange={handleInputChange}
                        handleSubmit={handleSubmit}
                    />
                </div>

            ) : (
                <div>
                    <div className='solarData'>
                        <ResultForm
                            cityName={cityName}
                            date={responseState.date}
                            sunrise={responseState.sunrise}
                            sunset={responseState.sunset}
                        />
                        {isBack ? (
                            <SearchForm
                                cityName={cityName}
                                handleInputChange={handleInputChange}
                                handleSubmit={handleSubmit}
                            />
                        ) : (
                            <button onClick={() => setIsBack(true)}>Search more</button>
                        )}
                    </div>
                </div>
            )}
        </div>
    );
};

export default SolarWatch;
