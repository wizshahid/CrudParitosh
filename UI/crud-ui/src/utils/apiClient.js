import axios from "axios";

export const apiClient = () => {
  const token = localStorage.getItem("cruduitoken");
  return axios.create({
    baseURL: process.env.REACT_APP_API_BASE_URL,
    headers: {
      Authorization: `Bearer ${token}`,
    },
  });
};
