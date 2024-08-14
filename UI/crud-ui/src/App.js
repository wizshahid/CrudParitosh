import React from "react";
import { BrowserRouter as Router } from "react-router-dom";
import Navbar from "./components/Navbar";
import { AuthProvider } from "./hooks/useAuth";
import AppRouter from "./components/AppRouter";

const App = () => {
  return (
    <Router>
      <AuthProvider>
        <Navbar />
        <AppRouter />
      </AuthProvider>
    </Router>
  );
};

export default App;
