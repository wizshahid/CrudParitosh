import React from "react";
import { Route, Routes } from "react-router-dom";
import ProductList from "./ProductList";
import ProductForm from "./ProductForm";
import LoginPage from "./LoginPage";
import RegisterPage from "./RegisterPage";
import AllUsers from "./AllUsers";
import PrivateRoute from "./PrivateRoute";
import UnAuthorized from "./UnAuthorized";
const AppRouter = () => {
  return (
    <Routes>
      <Route exact path="/" element={<ProductList />} />
      <Route exact path="/products" element={<ProductList />} />
      <Route exact path="/unauthorized" element={<UnAuthorized />} />
      <Route
        exact
        path="/products/add"
        element={
          <PrivateRoute allowedRoles={["Admin"]}>
            <ProductForm />
          </PrivateRoute>
        }
      />
      <Route
        exact
        path="/products/edit/:id"
        element={
          <PrivateRoute allowedRoles={["Admin"]}>
            <ProductForm />
          </PrivateRoute>
        }
      />
      <Route exact path="/login" element={<LoginPage />} />
      <Route
        exact
        path="/register"
        element={
          <PrivateRoute allowedRoles={["Admin"]}>
            <RegisterPage />
          </PrivateRoute>
        }
      />
      <Route
        exact
        path="/users"
        element={
          <PrivateRoute allowedRoles={["Admin"]}>
            <AllUsers />
          </PrivateRoute>
        }
      />
    </Routes>
  );
};

export default AppRouter;
