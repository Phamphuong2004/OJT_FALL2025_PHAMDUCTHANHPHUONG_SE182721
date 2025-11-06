import React from "react";
import { Navigate, Outlet } from "react-router-dom";
import AdminSidebar from "../Components/AdminSidebar";
import useAdminAuth from "../Auth/useAdminAuth"; // Custom hook to check admin auth

export default function AdminLayout() {
  const { isAdmin, loading } = useAdminAuth(); // Assuming useAuth is a custom hook to get auth status
  if (loading) {
    return <div>Loading...</div>;
  }
  if (!isAdmin) {
    return <Navigate to="/login" replace />;
  }

  return (
    <div style={{ display: "flex", minHeight: "100vh" }}>
      <AdminSidebar />
      <main style={{ flex: 1, padding: 20 }}>
        <Outlet /> {/* Render nested admin routes here */}
      </main>
    </div>
  );
}
