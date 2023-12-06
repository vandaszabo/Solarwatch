import React from 'react';
import sunriseIcon from '../../pictures/sunrise_icon2.png';
import sunsetIcon from '../../pictures/sunset_icon2.png';
import '../../Pages/SolarWatch/SolarWatch.css';

const ResultForm = ({ cityName, date, sunrise, sunset }) => {

//Format date and time
const monthNames = [
    'January', 'February', 'March', 'April', 'May', 'June',
    'July', 'August', 'September', 'October', 'November', 'December'
];

const formatDate = (dateString) => {
    const date = new Date(dateString);
    const year = date.getFullYear();
    const month = monthNames[date.getMonth()];
    const day = date.getDate().toString().padStart(2, '0');
    return `${month} ${day}, ${year}`;
};

const sunriseDate = new Date(sunrise);
const sunsetDate = new Date(sunset);

const formatTime = (date) => {
    const hours = date.getHours().toString().padStart(2, '0');
    const minutes = date.getMinutes().toString().padStart(2, '0');
    return `${hours}:${minutes}`;
};

const formattedDate = formatDate(date);
const formattedSunrise = formatTime(sunriseDate);
const formattedSunset = formatTime(sunsetDate);

    return (
        <>
            <h2>{cityName}</h2>

            <div>Today</div>
            <div style={{ color: 'lightcoral' }}>{formattedDate}</div>

            <div className='sunrise-sunset'>
                <div className='time'>
                    <h3>Sunrise</h3>
                    <img src={sunriseIcon}
                        alt='sunrise icon'
                        style={{ height: '50px', width: 'auto' }}
                    />
                    <h3>{formattedSunrise}</h3>
                </div>
                <div className='time'>
                    <h3>Sunset</h3>
                    <img src={sunsetIcon}
                        alt='sunset icon'
                        style={{ height: '50px', width: 'auto' }}
                    />
                    <h3>{formattedSunset}</h3>
                </div>
            </div>
        </>
    );
};

export default ResultForm;