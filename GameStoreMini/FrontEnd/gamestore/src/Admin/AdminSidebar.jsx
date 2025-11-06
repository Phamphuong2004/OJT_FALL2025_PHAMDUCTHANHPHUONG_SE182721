import React from "react";
import { NavLink } from "react-router-dom";

export default function AdminSidebar() {
  const linkStyle = ({ isActive }) => ({
    display: "block",
    padding: "10px 16px",
    textDecoration: "none",
    background: isActive ? "#eee" : "transparent",
    color: "#111",
  });
  return (
    <aside style={{ width: 220, borderRight: "1px solid #ddd", padding: 10 }}>
      <h3>Admin</h3>
      <nav>
        <NavLink to="/admin" end style={linkStyle}>
          Dashboard
        </NavLink>
        <NavLink to="/admin/users" style={linkStyle}>
          Users
        </NavLink>
        <NavLink to="/admin/games" style={linkStyle}>
          Games
        </NavLink>
        <NavLink to="/admin/orders" style={linkStyle}>
          Orders
        </NavLink>
        <NavLink to="/admin/inventory" style={linkStyle}>
          Inventory
        </NavLink>
        <NavLink to="/admin/media" style={linkStyle}>
          Media
        </NavLink>
        <NavLink to="/admin/reports" style={linkStyle}>
          Reports
        </NavLink>
      </nav>
    </aside>
  );
}
