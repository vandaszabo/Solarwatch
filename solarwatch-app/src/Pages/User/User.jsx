import React from 'react';
import { Link } from 'react-router-dom';
import './User.css';
import profileImg from '../../pictures/profileImage.jpg';
const User = () => {

    const userName = localStorage.getItem('userName');
    const email = localStorage.getItem('email');
    const role = localStorage.getItem('role');

    const user = {
        name: userName,
        email: email,
        role: role
    };

    return (
        <div>
            {user &&
                <div className='user-container'>
                    <div className='profile'>
                        <div className='img-container'>
                            <img src={profileImg} alt='profile-icon' height={'150px'} />
                        </div>
                        <div className='text-container'>
                        <div>Username:</div> <h3>{user.name}</h3>
                        <div>Email:</div> <h3>{user.email}</h3>
                        <div>Role:</div><h3> {user.role}</h3>
                        </div>
                    </div>
                </div>
            }
            {user && user.role === "Admin" ? (
                <div className='admin-link'>
                <Link to="/Admin"><button>Go to admin page</button></Link>
                </div>
            ) : null}
        </div>
    )
}

export default User;