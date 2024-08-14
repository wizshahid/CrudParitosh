import React, { useState, useEffect } from "react";
import { getUsers } from "../services/authService";

const AllUsers = () => {
  const [users, setUsers] = useState([]);

  useEffect(() => {
    loadUsers();
  }, []);

  const loadUsers = async () => {
    const result = await getUsers();
    setUsers(result.data);
  };

  return (
    <div className="container">
      <h2>All Users</h2>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Role</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {users.map((user) => (
            <tr key={user.id}>
              <td>{user.username}</td>
              <td>{user.role}</td>
              <td></td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default AllUsers;
