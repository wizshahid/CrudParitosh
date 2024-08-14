import React, { useState } from "react";
import axios from "axios";
import { register } from "../services/authService";

const RegisterPage = () => {
  const [username, setUsername] = useState("");
  const [password, setPassword] = useState("");
  const [role, setRole] = useState("User"); // Default role
  const [error, setError] = useState("");
  const [success, setSuccess] = useState("");

  const handleRegister = async (e) => {
    e.preventDefault();
    try {
      const response = await register(username, password, role);
      console.log("Registration successful:", response.data);
      setSuccess("User registered successfully");
      setError("");
    } catch (err) {
      console.error("Registration failed:", err);
      setError("Registration failed");
      setSuccess("");
    }
  };

  return (
    <div className="container mt-5">
      <h2>Register</h2>
      <form onSubmit={handleRegister}>
        <div className="mb-3">
          <label htmlFor="username" className="form-label">
            Username
          </label>
          <input
            type="text"
            className="form-control"
            id="username"
            value={username}
            onChange={(e) => setUsername(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="password" className="form-label">
            Password
          </label>
          <input
            type="password"
            className="form-control"
            id="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            required
          />
        </div>
        <div className="mb-3">
          <label htmlFor="role" className="form-label">
            Role
          </label>
          <select
            className="form-select"
            id="role"
            value={role}
            onChange={(e) => setRole(e.target.value)}
          >
            <option value="Admin">Admin</option>
            <option value="Manager">Manager</option>
            <option value="User">User</option>
          </select>
        </div>
        {error && <div className="alert alert-danger">{error}</div>}
        {success && <div className="alert alert-success">{success}</div>}
        <button type="submit" className="btn btn-primary">
          Register
        </button>
      </form>
    </div>
  );
};

export default RegisterPage;
