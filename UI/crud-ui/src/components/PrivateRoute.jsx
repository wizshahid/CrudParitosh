import React from "react";
import { Navigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";

const PrivateRoute = ({ children, allowedRoles }) => {
  const { isAuthenticated, isInRole } = useAuth();
  if (!isAuthenticated()) {
    return <Navigate to="/login" />;
  }

  if (allowedRoles && !isInRole(...allowedRoles)) {
    return <Navigate to="/unauthorized" />;
  }

  return children;
};

export default PrivateRoute;
