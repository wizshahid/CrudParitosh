import { apiClient } from "../utils/apiClient";

export const getProducts = async () => {
  return await apiClient().get(`/products`);
};

export const getProductById = async (id) => {
  return await apiClient().get(`/products/${id}`);
};

export const addProduct = async (product) => {
  return await apiClient().post(`/products`, product);
};

export const updateProduct = async (id, product) => {
  return await apiClient().put(`/products/${id}`, product);
};

export const deleteProduct = async (id) => {
  return await apiClient().delete(`/products/${id}`);
};
