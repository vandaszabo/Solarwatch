import { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import RegistrationForm from "../../Components/Forms/RegistrationForm.jsx";
import Loading from '../../Components/Loading.jsx';
import '../Login/Login.css';

const Registration = () => {

    const navigate = useNavigate();
    const [loading, setLoading] = useState(false);
    const [responseState, setResponseState] = useState('');

    //Create user
    const createUser = async (user) => {
        setLoading(true);
        const response = await fetch('/Auth/Register', {
            method: 'POST',
            headers: {
                'Content-Type': 'application/json',
            },
            body: JSON.stringify(user),
        });

        const data = await response.json();

        if (response.ok) {
            console.log('ResponseData: ', data);
            setResponseState(data);
        } else {
            console.error('Registration failed:', data);
            setResponseState(data);
        }
        setLoading(false);
    };

    //handle Registration
    const handleRegister = (user) => {
        createUser(user);
    };

    return (
        <div>
            {loading ? (
                <Loading />
            ) : (
                responseState === '' ? (
                    <div> <RegistrationForm
                        onCancel={() => navigate('/')}
                        disabled={loading}
                        onSave={handleRegister} />
                    </div>
                ) : (

                    responseState.hasOwnProperty("PasswordTooShort") ? (
                        <div>
                            <RegistrationForm
                                onCancel={() => navigate('/')}
                                disabled={loading}
                                onSave={handleRegister}
                                errorMessage={responseState.PasswordTooShort[0]} />
                        </div>

                    ) : responseState.hasOwnProperty("DuplicateUserName") ? (
                        <div>
                            <RegistrationForm
                                onCancel={() => navigate('/')}
                                disabled={loading}
                                onSave={handleRegister}
                                errorMessage={responseState.DuplicateUserName[0]} />
                        </div>

                    ) : responseState.hasOwnProperty("DuplicateEmail") ? (

                        <div>
                            <RegistrationForm
                                onCancel={() => navigate('/')}
                                disabled={loading}
                                onSave={handleRegister}
                                errorMessage={responseState.DuplicateEmail[0]} />
                        </div>

                    ) : (
                        <div>
                            Thank You For Your Registration {responseState.userName} !
                        </div>
                    )
                )
            )}
        </div>
    )
};

export default Registration;
