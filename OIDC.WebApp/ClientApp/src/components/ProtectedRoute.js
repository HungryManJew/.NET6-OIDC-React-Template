import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

export const ProtectedRoute = ({ children }) => {
  const { user, login } = useAuth();
  if ((!user || !user.length)) {
    // user is not authenticated
    login();

    //return <Navigate to='/'/>  // Not using this due to my needs
  }
  return children;
};