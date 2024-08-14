import React, { useState, useEffect } from "react";
import {
  addProduct,
  updateProduct,
  getProductById,
} from "../services/productService";
import { useNavigate, useParams } from "react-router-dom";

const ProductForm = () => {
  const [product, setProduct] = useState({
    name: "",
    description: "",
    price: "",
    category: "",
    stockQuantity: "",
    manufacturer: "",
  });
  const navigate = useNavigate();
  const { id } = useParams();

  useEffect(() => {
    if (id) {
      loadProduct();
    }
  }, [id]);

  const loadProduct = async () => {
    try {
      const result = await getProductById(id);
      setProduct(result.data);
    } catch (e) {
      alert(e);
    }
  };

  const onInputChange = (e) => {
    setProduct({ ...product, [e.target.name]: e.target.value });
  };

  const onSubmit = async (e) => {
    e.preventDefault();
    if (id) {
      await updateProduct(id, product);
    } else {
      await addProduct(product);
    }
    navigate("/");
  };

  return (
    <div className="container">
      <h2>{id ? "Edit Product" : "Add Product"}</h2>
      <form onSubmit={onSubmit}>
        <div className="mb-3">
          <label className="form-label">Name</label>
          <input
            type="text"
            className="form-control"
            name="name"
            value={product.name}
            onChange={onInputChange}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Description</label>
          <textarea
            className="form-control"
            name="description"
            value={product.description}
            onChange={onInputChange}
            required
          ></textarea>
        </div>
        <div className="mb-3">
          <label className="form-label">Price</label>
          <input
            type="number"
            className="form-control"
            name="price"
            value={product.price}
            onChange={onInputChange}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Category</label>
          <input
            type="text"
            className="form-control"
            name="category"
            value={product.category}
            onChange={onInputChange}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Stock Quantity</label>
          <input
            type="number"
            className="form-control"
            name="stockQuantity"
            value={product.stockQuantity}
            onChange={onInputChange}
            required
          />
        </div>
        <div className="mb-3">
          <label className="form-label">Manufacturer</label>
          <input
            type="text"
            className="form-control"
            name="manufacturer"
            value={product.manufacturer}
            onChange={onInputChange}
            required
          />
        </div>
        <button type="submit" className="btn btn-success">
          Save
        </button>
      </form>
    </div>
  );
};

export default ProductForm;
