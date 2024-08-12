import React, { useState, useEffect } from "react";
import { getProducts, deleteProduct } from "../services/productService";
import { useNavigate } from "react-router-dom";

const ProductList = () => {
  const [products, setProducts] = useState([]);

  useEffect(() => {
    loadProducts();
  }, []);

  const navigate = useNavigate();

  const loadProducts = async () => {
    const result = await getProducts();
    setProducts(result.data);
  };

  const handleDelete = async (id) => {
    await deleteProduct(id);
    loadProducts();
  };

  return (
    <div className="container">
      <h2>Product List</h2>
      <table className="table">
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          {products.map((product) => (
            <tr key={product.id}>
              <td>{product.name}</td>
              <td>{product.category}</td>
              <td>{product.price}</td>
              <td>{product.stockQuantity}</td>
              <td>
                <button
                  onClick={() => {
                    navigate("/edit-product/" + product.id);
                  }}
                  className="btn btn-primary"
                >
                  Edit
                </button>
                <button
                  className="btn btn-danger"
                  onClick={() => handleDelete(product.id)}
                >
                  Delete
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
};

export default ProductList;
