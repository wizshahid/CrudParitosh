import React, { useState, useEffect } from "react";
import { Table, Button, Modal, Form } from "react-bootstrap";
import { getProducts, deleteProduct } from "../services/productService";
import { useNavigate } from "react-router-dom";
import { useAuth } from "../hooks/useAuth";
import { placeOrder } from "../services/orderService";

const ProductList = () => {
  const [products, setProducts] = useState([]);
  const [selectedProduct, setSelectedProduct] = useState(null);
  const [quantity, setQuantity] = useState(1);
  const [showModal, setShowModal] = useState(false);

  const { isAuthenticated, isInRole, getUserId } = useAuth();
  const navigate = useNavigate();

  useEffect(() => {
    loadProducts();
  }, []);

  const loadProducts = async () => {
    try {
      const result = await getProducts();
      setProducts(result.data);
    } catch {
      alert("Error");
    }
  };

  const handleDelete = async (id) => {
    await deleteProduct(id);
    loadProducts();
  };

  const handleOrderClick = (product) => {
    setSelectedProduct(product);
    setShowModal(true);
  };

  const handlePlaceOrder = async () => {
    try {
      let data = {
        userId: getUserId(),
        productId: selectedProduct.id,
        quantity,
        productName: selectedProduct.name,
        price: selectedProduct.price,
      };

      await placeOrder(data);
      alert(`Order placed for ${quantity} of ${selectedProduct.name}`);
      setShowModal(false);
      setSelectedProduct(null);
      navigate("/myorders");
    } catch {
      alert("Error placing order");
    }
  };

  return (
    <div className="container">
      <h2>Product List</h2>
      <Table striped bordered hover>
        <thead>
          <tr>
            <th>Name</th>
            <th>Category</th>
            <th>Price</th>
            <th>Stock</th>
            <th>Order Count</th>
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
              <td>{product.orderCount}</td>
              <td>
                {isAuthenticated() && (
                  <Button
                    variant="primary"
                    onClick={() => handleOrderClick(product)}
                  >
                    Order
                  </Button>
                )}
                {isInRole("Admin") && (
                  <>
                    <Button
                      variant="primary"
                      onClick={() => {
                        navigate("/products/edit/" + product.id);
                      }}
                      className="ms-2"
                    >
                      Edit
                    </Button>
                    <Button
                      variant="danger"
                      className="ms-2"
                      onClick={() => handleDelete(product.id)}
                    >
                      Delete
                    </Button>
                  </>
                )}
              </td>
            </tr>
          ))}
        </tbody>
      </Table>

      {/* React-Bootstrap Modal */}
      {selectedProduct && (
        <Modal show={showModal} onHide={() => setShowModal(false)} centered>
          <Modal.Header closeButton>
            <Modal.Title>Place Order</Modal.Title>
          </Modal.Header>
          <Modal.Body>
            <h5>{selectedProduct.name}</h5>
            <p>Price: ${selectedProduct.price}</p>
            <p>Stock: {selectedProduct.stockQuantity}</p>
            <Form.Group className="mb-3">
              <Form.Label>Quantity</Form.Label>
              <Form.Control
                type="number"
                value={quantity}
                onChange={(e) => setQuantity(e.target.value)}
                min="1"
                max={selectedProduct.stockQuantity}
              />
            </Form.Group>
          </Modal.Body>
          <Modal.Footer>
            <Button variant="secondary" onClick={() => setShowModal(false)}>
              Close
            </Button>
            <Button variant="primary" onClick={handlePlaceOrder}>
              Place Order
            </Button>
          </Modal.Footer>
        </Modal>
      )}
    </div>
  );
};

export default ProductList;
