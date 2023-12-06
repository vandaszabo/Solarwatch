import React from 'react';
import ReactDOM from 'react-dom/client';
import { createBrowserRouter, RouterProvider } from 'react-router-dom';
import './index.css';
import Registration from './Pages/Registration/Registration.jsx';
import reportWebVitals from './reportWebVitals';
import SolarWatch from "./Pages/SolarWatch/SolarWatch.jsx";
import Layout from "./Pages/Layout/Layout.jsx";
import Home from './Pages/Home/Home.jsx';
import ErrorPage from "./Pages/Error/ErrorPage.jsx";
import Login from "./Pages/Login/Login.jsx";
import AdminPage from './Pages/Admin/AdminPage.jsx';
import PrivateRoute from './Components/PrivateRoute.jsx';
import User from './Pages/User/User.jsx';

const router = createBrowserRouter([
    {
        path: '/',
        element: <Layout />,
        errorElement: <ErrorPage />,
        children: [
            {
                path: '/',
                element: <Home />
            },
            {
                path: '/Login',
                element: <Login />
            },
            {
                path: '/Registration',
                element: <Registration />
            },
            {
                path: '/Solar-Watch',
                element: <SolarWatch />
            },
            {
                path: '/User',
                element: <User />
            },
            {
                path: '/Admin',
                element: <PrivateRoute role="Admin" element={<AdminPage />} />
            },
        ],
    },
]);

ReactDOM.createRoot(document.getElementById('root')).render(
    <React.StrictMode>
        <RouterProvider router={router}>{router.route}</RouterProvider>
    </React.StrictMode>
);

reportWebVitals();

