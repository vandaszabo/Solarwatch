import React from 'react';

const LoginForm = ({ email, password, handleInputChange, handleSubmit, errorMessage }) => {

    return (
        <form className='UserForm' onSubmit={handleSubmit}>

            {errorMessage &&
                    <div style={{color: 'red'}}>{errorMessage}</div>
                }
                
            <h3 style={{ color: 'red' }}>Please Login!</h3>

            <div className='control'>
            <label htmlFor='email'>Email:</label>
            <input  type='text'
                    name='email'    
                    value={email}
                    onChange={handleInputChange}
                    required 
            />
            </div>

            <div className='control'>
            <label htmlFor='password'>Password:</label>
            <input  type='password' 
                    name='password' 
                    value={password} 
                    onChange={handleInputChange}
                    required
            />
            </div>
            
            <button type='submit'>Submit</button>
        </form>
    );
};

export default LoginForm;