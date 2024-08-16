import React, { useEffect, useState } from "react";
import { useAuth } from "../hooks/useAuth";
import { getOrdersByUserId } from "../services/orderService";

const MyOrders = () => {
  const { getUserId } = useAuth();

  const [orders, setOrders] = useState([]);

  const getOrders = async () => {
    let result = await getOrdersByUserId(getUserId());
    setOrders(result.data);
  };

  useEffect(() => {
    getOrders();
  });

  return (
    <div className="container">
      <h2>My Orders</h2>
      <table className="table">
        <thead>
          <tr>
            <th>Product Name</th>
            <th>Quantity</th>
            <th>Price</th>
            <th>Date</th>
          </tr>
        </thead>
        <tbody>
          {orders.map((order) => (
            <tr key={order.id}>
              <td>{order.productName}</td>
              <td>{order.quantity}</td>
              <td>{order.price * order.quantity}</td>
              <td>{order.orderDate}</td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default MyOrders;
