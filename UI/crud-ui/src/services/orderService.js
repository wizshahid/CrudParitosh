import { apiClient } from "../utils/apiClient";

export const getOrdersByUserId = async (userId) => {
  return await apiClient().get(`/api/orders/${userId}`);
};

export const placeOrder = async (data) => {
  return await apiClient().post(`/api/orders`, data);
};
