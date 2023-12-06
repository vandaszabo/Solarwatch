import { React, useEffect, useState } from 'react';
import Loading from './Loading.jsx';

const UserTable = () => {
    const token = localStorage.getItem('token');
    const [isLoading, setIsLoading] = useState(false);
    const [users, setUsers] = useState(null);

    useEffect(() => {
        const getUsers = async () => {
            setIsLoading(true);
            try {
                const response = await fetch('/User/List', {
                    method: 'GET',
                    headers: {
                        Authorization: `Bearer ${token}`,
                        'Content-Type': 'application/json',
                    }
                });
    
                const data = await response.json();
                setUsers(data);
                console.log("users :", data);
    
            } catch (error) {
                console.error('Error getting users:', error.message);
            } finally {
                setIsLoading(false);
            }
        };
    
        getUsers();
    
    }, [token]);
    


    const handleDelete = async (userName) => {
        setIsLoading(true);
        try {
            const response = await fetch('/User/Delete', {
                method: 'DELETE',
                headers: {
                    Authorization: `Bearer ${token}`,
                    'Content-Type': 'application/json',
                },
                body: JSON.stringify({ userName }),
            });
            const data = await response.json();
            console.log("res:", response);
            console.log("deleted user: ", data);;
        } catch (error) {
            console.error('Error deleting user:', error.message);
        } finally {
            setIsLoading(false);
        }
    };

    return (
        <div className='userTable'>
        {isLoading ? (
            <Loading />
        ):(
            <table>
                <thead>
                    <h3>Users</h3>
                    <tr>
                        <th>Username</th>
                        <th>Email</th>
                    </tr>
                </thead>
                <tbody>
                    {users?.map(u => (
                        <tr key={u.userName}>
                            <td>{u.userName}</td>
                            <td>{u.email}</td>
                            <td><button onClick={() => handleDelete(u.userName)}>Delete</button></td>
                        </tr>
                    ))}
                </tbody>
            </table>
        )}
        </div>
    )
}
export default UserTable;