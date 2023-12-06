import { React, useEffect, useState } from 'react';
import Loading from './Loading.jsx';

const SolarwatchTable = () => {
    const token = localStorage.getItem('token');
    const [isLoading, setIsLoading] = useState(false);
    const [solarwatches, setSolarwatches] = useState(null);

    useEffect(() => {
        const getSolarwatches = async () => {
            setIsLoading(true);
            try {
                const response = await fetch('/SolarWatch/GetSolarWatches', {
                    method: 'GET',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    }
                });
    
                const data = await response.json();
                setSolarwatches(data);
                console.log("solarwatches :", data);
    
            } catch (error) {
                console.error('Error getting solarwatches:', error.message);
            } finally {
                setIsLoading(false);
            }
        };
    
        getSolarwatches();
    
    }, [token]);
    


    const handleDelete = async (id) => {
        setIsLoading(true);
        try {
            const response = await fetch('/SolarWatch/Delete', {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify( {id: id} ),
            });
            const data = await response.json();
            console.log("res:", response);
            console.log("deleted solarwatch: ", data);;
        } catch (error) {
            console.error('Error deleting solarwatch:', error.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className='solarwatchTable'>
        {isLoading ? (
            <Loading />
        ):(
            <table>
                <thead>
                    <h3>Solarwatches</h3>
                    <tr>
                        <th>Id</th>
                        <th>Date</th>
                        <th>City Id</th>
                        <th>Sunrise</th>
                        <th>Sunset</th>
                    </tr>
                </thead>
                <tbody>
                    {solarwatches?.map(sw => (
                        <tr key={sw.id}>
                            <td>{sw.id}</td>
                            <td>{sw.date}</td>
                            <td>{sw.cityId}</td>
                            <td>{sw.sunrise}</td>
                            <td>{sw.sunset}</td>
                            <td><button onClick={() => handleDelete(sw.id)}>Delete</button></td>
                        </tr>
                    ))}
                </tbody>
            </table>
        )}
        </div>
    )
}
export default SolarwatchTable;