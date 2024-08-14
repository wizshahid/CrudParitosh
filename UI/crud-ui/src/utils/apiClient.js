import axios from "axios";

const token = localStorage.getItem("cruduitoken");

export const apiClient = axios.create({
  baseURL: process.env.REACT_APP_API_BASE_URL,
  headers: {
    Authorization: `Bearer ${token}`,
  },
});
