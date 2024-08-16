import { apiClient } from "../utils/apiClient";

export const login = async (username, password) => {
  return await apiClient().post(`/api/auth/login`, {
    username,
    password,
  });
};

export const getUsers = async () => {
  return await apiClient().get(`/api/auth/users`);
};

export const register = async (username, password, role) => {
  return await apiClient().post(`/api/auth/register`, {
    username,
    password,
    role,
  });
};
