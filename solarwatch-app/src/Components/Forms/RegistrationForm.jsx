import React, { useState } from 'react';

const RegisterForm = ({ onSave, disabled, onCancel, errorMessage }) => {
    const [user, setUser] = useState({
        Email: '',
        Username: '',
        Password: '',
    });

    const onSubmit = (e) => {
        e.preventDefault();

        if (e.target.password.value !== e.target.passwordAgain.value) {
            alert('Passwords must match!');
            return;
        } else {
            console.log('Form: ', user.Username);
            onSave(user);
        }
    };

    const handleChange = (e) => {
        setUser({
            ...user,
            [e.target.name]: e.target.value,
        });
    };

    return (
        <div className='auth-panel'>
            <h3>You can Register here:</h3>
            <form className='UserForm' onSubmit={onSubmit}>

                {errorMessage &&
                    <div style={{color: 'red'}}>{errorMessage}</div>
                }

                <div className='control'>
                    <label htmlFor='email'>Email:</label>
                    <input
                        type='text'
                        name='Email'
                        id='email'
                        value={user.Email}
                        onChange={handleChange}
                    />
                </div>

                <div className='control'>
                    <label htmlFor='username'>Username:</label>
                    <input
                        type='text'
                        name='Username'
                        id='username'
                        value={user.Username}
                        onChange={handleChange}
                    />
                </div>

                <div className='control'>
                    <label htmlFor='password'>Password:</label>
                    <input
                        type='password'
                        name='Password'
                        id='password'
                        value={user.Password}
                        onChange={handleChange}
                    />
                </div>

                <div className='control'>
                    <label htmlFor='passwordAgain'>Password again:</label>
                    <input
                        type='password'
                        name='passwordAgain'
                        id='passwordAgain'
                        onChange={handleChange}
                    />
                </div>

                <div className='buttons'>
                    <div>
                        <button type='submit' disabled={disabled}>
                            Register
                        </button>
                    </div>
                    <div>
                        <button type='button' onClick={onCancel}>
                            Cancel
                        </button>
                    </div>
                </div>
            </form>

        </div>

    );
};

export default RegisterForm;
