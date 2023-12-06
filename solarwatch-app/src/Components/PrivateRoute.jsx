import React from 'react';
import { Link} from 'react-router-dom';

const PrivateRoute = ({ element, role }) => {

    const userRole = localStorage.getItem('role');
    
    return (
        userRole ? (
            userRole === role ? element : <Link to='/Login'><button>Login as {role}</button></Link>
        ) : null
    );

};

export default PrivateRoute;
