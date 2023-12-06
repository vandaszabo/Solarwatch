import { Outlet, Link, useNavigate } from 'react-router-dom';
import { useState, useEffect } from 'react';
import logo from '../../pictures/logo-sw.png';
import user from '../../pictures/user-icon.png';
import './Layout.css';

const Layout = () => {

  const navigate = useNavigate();
  const name = localStorage.getItem('userName');
  const [userName, setUserName] = useState('');

  
  const removeUserData = () => {
      localStorage.removeItem('token');
      localStorage.removeItem('userName');
      localStorage.removeItem('email');
      localStorage.removeItem('role');
  };

  //Deletes all user data after 30 mins
  useEffect(() => {
    const logoutTimeout = setTimeout(() => {
      removeUserData();
      navigate('/Login');
    }, 30 * 60 * 1000);

    return () => clearTimeout(logoutTimeout);
  }, [navigate]);


  //Logout
  const handleLogout = (e) => {
    e.preventDefault();
    removeUserData();
    navigate('/');
  };

  useEffect(() => {
    setUserName(name);
  }, [name]);

  return (
    <div className='Layout'>
      <nav>
        <ul>
          <li>
            <Link to='/'>
              <img src={logo} alt="SolarWatchLogo" className='logo' height={'50vh'} />
            </Link>
            <span className='solar'>Solar</span><span className='watch'>Watch</span>
          </li>
          <li>
            {userName ? (
              <>
                <Link to='/User' style={{ display: 'flex', alignItems: 'center', textDecoration: 'none', paddingRight: '10px' }}>
                  <img src={user} alt="userIcon" className='icon' height={'30vh'} />
                  <button id='userBtn'>{userName}</button>
                </Link>
                <button className='layout_button' onClick={handleLogout}>
                  Logout
                </button>
              </>
            ) : (
              <Link to='/Login'>
                <button className='layout_button'>Login</button>
              </Link>
            )}
          </li>
        </ul>
      </nav>
      <Outlet />
    </div>
  );
};

export default Layout;
