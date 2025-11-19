import React, { useState } from "react";
import { Navigate, Outlet, Link, useNavigate } from "react-router-dom";
import { getUserRole, isAuthenticated } from "../Auth/useAuth";
import { logout as apiLogout } from "../API/UserAPI";
import "../Decorate/AdminLayout.css";

export default function AdminLayout() {
  const [sidebarCollapsed, setSidebarCollapsed] = useState(false);
  const navigate = useNavigate();

  // Check if user is authenticated and is Admin
  const auth = isAuthenticated();
  const role = getUserRole();

  if (!auth || role !== "Admin") {
    return <Navigate to="/login" replace />;
  }

  const handleLogout = () => {
    apiLogout();
    navigate("/");
  };

  return (
    <div className="admin-layout">
      <AdminSidebar
        collapsed={sidebarCollapsed}
        onToggle={() => setSidebarCollapsed(!sidebarCollapsed)}
        onLogout={handleLogout}
      />
      <div className={`admin-main ${sidebarCollapsed ? "expanded" : ""}`}>
        <div className="admin-content">
          <Outlet /> {/* Render nested admin routes here */}
        </div>
      </div>
    </div>
  );
}

// AdminSidebar Component
function AdminSidebar({ collapsed, onToggle, onLogout }) {
  const menuItems = [
    {
      path: "/admin/system",
      icon: "fa-th-large",
      label: "Tổng quan hệ thống",
    },
    {
      path: "/admin/orders",
      icon: "fa-shopping-cart",
      label: "Quản lý đơn hàng",
    },
    {
      path: "/admin/games",
      icon: "fa-gamepad",
      label: "Quản lý game",
    },

    {
      path: "/admin/reviews",
      icon: "fa-star",
      label: "Quản lý reviews",
    },
    {
      path: "/admin/promotion",
      icon: "fa-tags",
      label: "Quản lý khuyến mãi",
    },
    {
      path: "/admin/users",
      icon: "fa-users",
      label: "Quản lý người dùng",
    },
  ];

  return (
    <aside className={`admin-sidebar ${collapsed ? "collapsed" : ""}`}>
      <div className="sidebar-header">
        <Link to="/admin/system" className="sidebar-logo">
          {!collapsed && (
            <>
              <i className="fas fa-cogs"></i>
              <span>Quản Trị Hệ Thống</span>
            </>
          )}
          {collapsed && <i className="fas fa-cogs"></i>}
        </Link>
        <button className="sidebar-toggle" onClick={onToggle}>
          <i className={`fas fa-chevron-${collapsed ? "right" : "left"}`}></i>
        </button>
      </div>

      <nav className="sidebar-menu">
        {menuItems.map((item) => (
          <Link
            key={item.path}
            to={item.path}
            className="sidebar-menu-item"
            title={collapsed ? item.label : ""}
          >
            <i className={`fas ${item.icon}`}></i>
            {!collapsed && <span>{item.label}</span>}
          </Link>
        ))}
      </nav>

      <div className="sidebar-footer">
        <button
          className="sidebar-logout"
          onClick={onLogout}
          title={collapsed ? "Đăng xuất" : ""}
        >
          <i className="fas fa-sign-out-alt"></i>
          {!collapsed && <span>Đăng xuất</span>}
        </button>
      </div>
    </aside>
  );
}
