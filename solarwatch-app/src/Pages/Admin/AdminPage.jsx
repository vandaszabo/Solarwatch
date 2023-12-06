import { React, useState } from 'react';
import './AdminPage.css';
import UserTable from '../../Components/UserTable.jsx';
import SolarwatchTable from '../../Components/SolarwatchTable.jsx';

const AdminPage = () => {

    const [showUsersTable, setShowUsersTable] = useState(false);
    const [showSolarwatchTable, setShowSolarwatchTable] = useState(false);
    const [showBack, setShowBack] = useState(false);

    const handleManageUsers = (e) => {
        e.preventDefault();
        setShowUsersTable(true);
        setShowBack(true);
    };

    const handleManageSolarwatches = (e) => {
        e.preventDefault();
        setShowSolarwatchTable(true);
        setShowBack(true);
    };

    const handleBack = (e) =>{
        e.preventDefault();
        setShowUsersTable(false);
        setShowSolarwatchTable(false);
        setShowBack(false);
    };

    return (
        <div className='admin-container'>
            <div className='header'>
                {showBack && 
                    <button onClick={handleBack}>Back</button>
                }
            </div>
            {showUsersTable ? (
                <UserTable />
            ) : 
            showSolarwatchTable ? (
                <SolarwatchTable />
            ):(
                <div>
                <h2>Admin Page</h2>
                <div className='buttons'>
                    <div><button onClick={handleManageUsers}>Manage Users</button></div>
                    <div><button onClick={handleManageSolarwatches}>Manage Solarwatches</button></div>
                </div>
                </div>
            )}
        </div>
    );
}

export default AdminPage;
