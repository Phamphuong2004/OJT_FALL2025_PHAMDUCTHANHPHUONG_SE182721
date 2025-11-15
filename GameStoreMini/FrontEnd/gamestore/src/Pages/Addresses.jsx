import React from "react";
import AddressManager from "../Components/AddressManager";
import "../Decorate/Pages.css";

/**
 * Address Management Page
 * Trang quản lý địa chỉ giao hàng của người dùng
 */
const AddressesPage = () => {
  return (
    <div className="page-container">
      <div className="page-header">
        <h1>Quản lý địa chỉ giao hàng</h1>
        <p className="page-subtitle">
          Thêm, sửa, xóa và quản lý các địa chỉ giao hàng của bạn
        </p>
      </div>

      <AddressManager />
    </div>
  );
};

export default AddressesPage;
