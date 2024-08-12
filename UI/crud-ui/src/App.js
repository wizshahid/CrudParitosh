import React from "react";
import { BrowserRouter as Router, Route, Routes, Link } from "react-router-dom";
import ProductList from "./components/ProductList";
import ProductForm from "./components/ProductForm";

const App = () => {
  return (
    <Router>
      <nav>
        <ul>
          <li>
            <Link to="/">Product List</Link>
          </li>
          <li>
            <Link to="/add-product">Add Product</Link>
          </li>
        </ul>
        <Routes>
          <Route exact path="/" element={<ProductList />} />
          <Route exact path="/add-product" element={<ProductForm />} />
          <Route exact path="/edit-product/:id" element={<ProductForm />} />
        </Routes>
      </nav>
    </Router>
  );
};

export default App;
