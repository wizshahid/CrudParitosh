import axios from "axios";

const API_URL = process.env.REACT_APP_API_BASE_URL;

export const getProducts = async () => {
  console.log(API_URL);
  return await axios.get(`${API_URL}/products`);
};

export const getProductById = async (id) => {
  return await axios.get(`${API_URL}/products/${id}`);
};

export const addProduct = async (product) => {
  return await axios.post(`${API_URL}/products`, product);
};

export const updateProduct = async (id, product) => {
  return await axios.put(`${API_URL}/products/${id}`, product);
};

export const deleteProduct = async (id) => {
  return await axios.delete(`${API_URL}/products/${id}`);
};
