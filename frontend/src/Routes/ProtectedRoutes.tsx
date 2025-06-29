import React from 'react'
import { Navigate, useLocation } from 'react-router-dom';
import {useAuthHttp as useAuth } from '../Context/useAuthHttp';

type Props = {
    children?: React.ReactNode;
}

const ProtectedRoutes = ({children}: Props) => {
    const location = useLocation();
    const { isLoggedIn } = useAuth();
  return (
     isLoggedIn() ? (
        <>{children}</>
     ) :  (
        <Navigate to={"/login"} state={{ from: location }} replace={true} />
     )
  )
}

export default ProtectedRoutes