import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import SearchForm from '../../Components/Forms/SearchForm.jsx';
import ResultForm from '../../Components/Forms/ResultForm.jsx';
import Loading from '../../Components/Loading.jsx';
import './SolarWatch.css';
import ResultHistoryForm from '../../Components/Forms/ResultHistoryForm.jsx';

const SolarWatch = () => {

    const [isLoading, setIsLoading] = useState(false);
    const [responseState, setResponseState] = useState('');
    const [cityName, setCityName] = useState('');
    const [newSearch, setNewSearch] = useState(false);
    const [prevSearches, setPrevSearches] = useState([]);
    const [cityAlreadySearched, setCityAlreadySearched] = useState(false);

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
            console.log("response data: ", data);
            setResponseState(data);

            setPrevSearches((prevSearches) => {
                const newSearch = { city: data.cityName, date: data.solarWatch.date, sunrise: data.solarWatch.sunrise, sunset: data.solarWatch.sunset };
    
                if (!prevSearches.some(search => search.city === newSearch.city)){
                    const updatedSearches = [newSearch, ...prevSearches];
                    const limitedSearches = updatedSearches.slice(0, 10);
                    return limitedSearches;
                }
                return prevSearches;
            });

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
                if(!prevSearches.some(search => search.city === cityName)){
                    await getSunriseAndSunset(cityName);
                    setCityAlreadySearched(false);
                }
                else{
                    setCityAlreadySearched(true);
                }
            } catch (error) {
                console.error('Error getting solarWatch:', error.message);
                setResponseState({ error: 'An error occurred while processing your request.' });
            }
        } else {
            navigate('/Login');
        }
    };

    return (
        <>
            <div className='solar-watch'>
                {isLoading ? (
                    <Loading />
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
                        <div>
                            <ResultForm
                                cityName={responseState.cityName}
                                date={responseState.solarWatch.date}
                                sunrise={responseState.solarWatch.sunrise}
                                sunset={responseState.solarWatch.sunset}
                            />
                            {newSearch ? (
                                <>
                                    <SearchForm
                                        cityName={cityName}
                                        handleInputChange={handleInputChange}
                                        handleSubmit={handleSubmit}
                                    />
                                    {cityAlreadySearched && <div>You have already searched for this city.</div>}

                                    <div>Previously searched:</div>
                                    <div className='searchCards'>
                                        {prevSearches.map((ps, index) => (
                                            <ResultHistoryForm
                                                key={index}
                                                cityName={ps.city}
                                                sunrise={ps.sunrise}
                                                sunset={ps.sunset}
                                            />
                                        ))}
                                    </div>
                                </>
                            ) : (
                                <button onClick={() => setNewSearch(true)}>Search more</button>
                            )}
                        </div>
                    </div>
                )}
            </div>

        </>
    );
};

export default SolarWatch;
