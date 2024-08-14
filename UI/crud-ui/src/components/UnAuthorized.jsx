import React from "react";
import { Link } from "react-router-dom";

const UnAuthorized = () => {
  return (
    <div className="container mt-5">
      <h2>Unauthorized Access</h2>
      <p>You do not have permission to view this page.</p>
      <Link to="/" className="btn btn-primary">
        Go Back Home
      </Link>
    </div>
  );
};

export default UnAuthorized;
