import React, { useState } from "react";
import { Link } from "react-router-dom";
import AdminReviewDashboard from "./AdminReviewDashboard";
import "../Decorate/AdminSystemManagement.css";

export default function AdminSystemManagement() {
  const [activeTab, setActiveTab] = useState("overview");

  const tabs = [
    { id: "overview", label: "Tổng quan", icon: "fa-chart-line" },
    { id: "games", label: "Quản lý Game", icon: "fa-gamepad" },
    { id: "reviews", label: "Quản lý Review", icon: "fa-star" },
    { id: "promotions", label: "Quản lý khuyến mãi", icon: "fa-tags" },
    { id: "users", label: "Quản lý người dùng", icon: "fa-users" },
  ];

  const renderContent = () => {
    switch (activeTab) {
      case "overview":
        return <OverviewTab />;
      case "games":
        return <GamesTab />;
      case "reviews":
        return <ReviewsTab />;
      case "promotions":
        return <PromotionsTab />;
      case "users":
        return <UsersTab />;
      default:
        return <OverviewTab />;
    }
  };

  return (
    <div className="admin-system-management">
      <div className="system-header">
        <h1>
          <i className="fas fa-cog"></i> Quản Lý Hệ Thống
        </h1>
        <p>Quản lý toàn bộ nội dung và cấu hình của hệ thống</p>
      </div>

      <div className="system-tabs">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            className={`system-tab ${activeTab === tab.id ? "active" : ""}`}
            onClick={() => setActiveTab(tab.id)}
          >
            <i className={`fas ${tab.icon}`}></i>
            <span>{tab.label}</span>
          </button>
        ))}
      </div>

      <div className="system-content">{renderContent()}</div>
    </div>
  );
}

// Overview Tab
function OverviewTab() {
  return (
    <div className="overview-tab">
      <div className="quick-stats">
        <div className="stat-card blue">
          <i className="fas fa-gamepad"></i>
          <div>
            <h3>Games</h3>
            <p>Quản lý danh sách game, thêm mới, chỉnh sửa</p>
          </div>
        </div>
        <div className="stat-card yellow">
          <i className="fas fa-star"></i>
          <div>
            <h3>Reviews</h3>
            <p>Kiểm duyệt và quản lý đánh giá từ khách hàng</p>
          </div>
        </div>
        <div className="stat-card green">
          <i className="fas fa-tags"></i>
          <div>
            <h3>Promotions</h3>
            <p>Tạo và quản lý các chương trình khuyến mãi</p>
          </div>
        </div>
        <div className="stat-card purple">
          <i className="fas fa-users"></i>
          <div>
            <h3>Users</h3>
            <p>Quản lý tài khoản người dùng và phân quyền</p>
          </div>
        </div>
      </div>

      <div className="quick-actions">
        <h2>Thao tác nhanh</h2>
        <div className="action-grid">
          <Link to="/admin/add-game" className="action-card">
            <i className="fas fa-plus-circle"></i>
            <span>Thêm Game mới</span>
          </Link>
          <Link to="/admin/promotion/create" className="action-card">
            <i className="fas fa-percent"></i>
            <span>Tạo khuyến mãi</span>
          </Link>
          <Link to="/admin/users" className="action-card">
            <i className="fas fa-user-plus"></i>
            <span>Quản lý Users</span>
          </Link>
          <Link to="/admin/orders" className="action-card">
            <i className="fas fa-shopping-cart"></i>
            <span>Xem đơn hàng</span>
          </Link>
        </div>
      </div>
    </div>
  );
}

// Games Tab
function GamesTab() {
  return (
    <div className="games-tab">
      <div className="tab-header">
        <h2>
          <i className="fas fa-gamepad"></i> Quản lý Game
        </h2>
        <Link to="/admin/add-game" className="btn-primary">
          <i className="fas fa-plus"></i> Thêm Game mới
        </Link>
      </div>

      <div className="info-box">
        <i className="fas fa-info-circle"></i>
        <div>
          <h3>Tính năng đang phát triển</h3>
          <p>
            Bạn có thể thêm game mới bằng cách click vào nút "Thêm Game mới" ở
            trên. Chức năng danh sách và chỉnh sửa game đang được phát triển.
          </p>
          <Link to="/admin/add-game" className="btn-link">
            Đi đến trang thêm game <i className="fas fa-arrow-right"></i>
          </Link>
        </div>
      </div>

      <div className="feature-list">
        <h3>Các tính năng sẽ có:</h3>
        <ul>
          <li>
            <i className="fas fa-check"></i> Danh sách tất cả games với
            pagination
          </li>
          <li>
            <i className="fas fa-check"></i> Tìm kiếm và lọc games theo category
          </li>
          <li>
            <i className="fas fa-check"></i> Chỉnh sửa thông tin game
          </li>
          <li>
            <i className="fas fa-check"></i> Quản lý stock và giá
          </li>
          <li>
            <i className="fas fa-check"></i> Upload và quản lý hình ảnh
          </li>
        </ul>
      </div>
    </div>
  );
}

// Reviews Tab
function ReviewsTab() {
  return (
    <div className="reviews-tab">
      <div className="tab-header">
        <h2>
          <i className="fas fa-star"></i> Quản lý Reviews
        </h2>
      </div>
      <AdminReviewDashboard />
    </div>
  );
}

// Promotions Tab
function PromotionsTab() {
  return (
    <div className="promotions-tab">
      <div className="tab-header">
        <h2>
          <i className="fas fa-tags"></i> Quản lý khuyến mãi
        </h2>
        <Link to="/admin/promotion/create" className="btn-primary">
          <i className="fas fa-plus"></i> Tạo khuyến mãi mới
        </Link>
      </div>

      <div className="info-box">
        <i className="fas fa-info-circle"></i>
        <div>
          <h3>Quản lý chương trình khuyến mãi</h3>
          <p>
            Xem danh sách và quản lý các chương trình khuyến mãi tại trang
            chuyên dụng.
          </p>
          <Link to="/admin/promotion" className="btn-link">
            Đi đến trang quản lý khuyến mãi{" "}
            <i className="fas fa-arrow-right"></i>
          </Link>
        </div>
      </div>
    </div>
  );
}

// Users Tab
function UsersTab() {
  return (
    <div className="users-tab">
      <div className="tab-header">
        <h2>
          <i className="fas fa-users"></i> Quản lý người dùng
        </h2>
      </div>

      <div className="info-box">
        <i className="fas fa-info-circle"></i>
        <div>
          <h3>Quản lý tài khoản người dùng</h3>
          <p>
            Xem danh sách, phân quyền và quản lý tài khoản người dùng tại trang
            chuyên dụng.
          </p>
          <Link to="/admin/users" className="btn-link">
            Đi đến trang quản lý users <i className="fas fa-arrow-right"></i>
          </Link>
        </div>
      </div>
    </div>
  );
}
