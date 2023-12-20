import React from 'react';
import '../../Pages/SolarWatch/SolarWatch.css';

const ResultHistoryForm = ({ cityName, sunrise, sunset }) => {

    const sunriseDate = new Date(sunrise);
    const sunsetDate = new Date(sunset);

    const formatTime = (date) => {
        const hours = date.getHours().toString().padStart(2, '0');
        const minutes = date.getMinutes().toString().padStart(2, '0');
        return `${hours}:${minutes}`;
    };

    const formattedSunrise = formatTime(sunriseDate);
    const formattedSunset = formatTime(sunsetDate);

    return (
            <div className='resultHistoryForm'> 
                <h4>{cityName}</h4>
                <div>Sunrise: {formattedSunrise}</div>
                <div>Sunset: {formattedSunset}</div>
            </div>
    );
};

export default ResultHistoryForm;