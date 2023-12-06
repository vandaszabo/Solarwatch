import React from 'react';

const SearchForm = ({ cityName, handleInputChange, handleSubmit }) => {
    
    return (
        <form onSubmit={handleSubmit} className='solar-watch-form'>
            <label>City:</label>
            <input
                type='text'
                name='cityname'
                placeholder="enter a city name"
                value={cityName}
                onChange={handleInputChange}
                required
            />
            <button type='submit'>Search</button>
        </form>
    );
};

export default SearchForm;
