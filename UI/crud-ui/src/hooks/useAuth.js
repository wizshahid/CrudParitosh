import { jwtDecode } from "jwt-decode";
import { createContext, useContext, useMemo, useState } from "react";
import { decodeToken, isExpired } from "react-jwt";
import { useNavigate } from "react-router-dom";
const AuthContext = createContext();

export const AuthProvider = ({ children }) => {
  const [user, setUser] = useUser();
  const navigate = useNavigate();

  const login = async (data) => {
    setUser(data);

    const searchParams = new URLSearchParams(window.location.search);
    const returnUrl = searchParams.get("returnUrl");
    if (returnUrl) navigate(returnUrl);
    else {
      let path = "/products";
      if (data.role === "Admin") path = "/users";
      navigate(path);
    }
  };

  const loginWithoutRedirecting = (data, cb) => {
    setUser(data);
    if (cb) cb();
  };

  const getUsername = () => {
    return user?.username;
  };

  const getUserId = () => {
    return user?.userId;
  };

  const logout = () => {
    setUser(null);
    navigate("/", { replace: true });
  };

  const isAuthenticated = () => {
    return !isExpired(user?.token);
  };

  const isAdmin = () => {
    return isInRole("Admin");
  };

  const isInRole = (...roles) => {
    if (!isAuthenticated()) return false;
    const payload = decodeToken(user?.token);
    return roles.includes(payload.role);
  };

  const value = useMemo(
    () => ({
      user,
      isAdmin,
      login,
      logout,
      isAuthenticated,
      isInRole,
      loginWithoutRedirecting,
      getUsername,
      getUserId,
    }),
    [user]
  );
  return <AuthContext.Provider value={value}>{children}</AuthContext.Provider>;
};

export const useAuth = () => {
  return useContext(AuthContext);
};

export const useUser = () => {
  const [storedValue, setStoredValue] = useState(() => {
    try {
      const authToken = localStorage.getItem("cruduitoken");
      if (authToken) {
        const decodedToken = jwtDecode(authToken);
        return {
          username: decodedToken.unique_name,
          role: decodedToken.role,
          userId: decodedToken.nameid,
          token: authToken,
        };
      } else {
        return null;
      }
    } catch (err) {
      return null;
    }
  });

  const setValue = async (token) => {
    try {
      localStorage.setItem("cruduitoken", token);

      const decodedToken = jwtDecode(token);
      setStoredValue({
        username: decodedToken.unique_name,
        role: decodedToken.role,
        userId: decodedToken.nameid,
        token: token,
      });
    } catch (err) {
      console.error(err);
    }
  };

  return [storedValue, setValue];
};
