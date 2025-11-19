import React, { useState, useEffect } from "react";
import OrderAPI from "../API/OrderAPI";
import "../Decorate/AdminOrderManagement.css";

export default function AdminOrderManagement() {
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [statistics, setStatistics] = useState(null);
  const [activeTab, setActiveTab] = useState("all");
  const [currentPage, setCurrentPage] = useState(1);
  const [totalPages, setTotalPages] = useState(1);
  const [selectedOrder, setSelectedOrder] = useState(null);
  const [showModal, setShowModal] = useState(false);

  const tabs = [
    { id: "all", label: "Tất cả" },
    { id: "Pending", label: "Chờ xác nhận" },
    { id: "Processing", label: "Đang xử lý" },
    { id: "Shipping", label: "Đang giao" },
    { id: "Completed", label: "Hoàn thành" },
    { id: "Cancelled", label: "Đã hủy" },
    { id: "Refund", label: "Hoàn tiền" },
  ];

  useEffect(() => {
    fetchOrders();
    fetchStatistics();
  }, [activeTab, currentPage]);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      const status = activeTab === "all" ? null : activeTab;
      const data = await OrderAPI.getAllOrdersAdmin(status, currentPage, 20);
      console.debug("[AdminOrderManagement] fetchOrders response:", data);

      // Handle multiple possible shapes returned by the backend:
      // - { data: [...], totalPages }
      // - { orders: [...], totalPages }
      // - [...] (array directly)
      let list = [];
      if (Array.isArray(data)) {
        list = data;
      } else if (Array.isArray(data?.data)) {
        list = data.data;
      } else if (Array.isArray(data?.orders)) {
        list = data.orders;
      } else if (Array.isArray(data?.result)) {
        list = data.result;
      }

      setOrders(list);

      const pages =
        data?.totalPages || data?.pageCount || data?.totalPagesCount || 1;
      setTotalPages(pages || 1);
    } catch (error) {
      console.error("Failed to fetch orders:", error);
      alert("Lỗi khi tải danh sách đơn hàng");
    } finally {
      setLoading(false);
    }
  };

  const fetchStatistics = async () => {
    try {
      const stats = await OrderAPI.getOrderStatistics();
      setStatistics(stats);
    } catch (error) {
      console.error("Failed to fetch statistics:", error);
    }
  };

  const handleStatusChange = async (orderId, newStatus) => {
    if (
      !window.confirm(`Bạn có chắc muốn chuyển trạng thái sang "${newStatus}"?`)
    ) {
      return;
    }

    try {
      await OrderAPI.updateOrderStatus(orderId, newStatus);
      alert("Cập nhật trạng thái thành công!");
      fetchOrders();
      fetchStatistics();
    } catch (error) {
      console.error("Failed to update status:", error);
      alert("Lỗi khi cập nhật trạng thái");
    }
  };

  const handleViewDetails = async (orderId) => {
    try {
      const order = await OrderAPI.getOrderAdmin(orderId);
      setSelectedOrder(order);
      setShowModal(true);
    } catch (error) {
      console.error("Failed to fetch order details:", error);
      alert("Lỗi khi tải chi tiết đơn hàng");
    }
  };

  const getStatusColor = (status) => {
    const colors = {
      Pending: "#faad14",
      Processing: "#1890ff",
      Shipping: "#13c2c2",
      Completed: "#52c41a",
      Cancelled: "#f5222d",
      Refund: "#eb2f96",
    };
    return colors[status] || "#666";
  };

  const getStatusText = (status) => {
    const texts = {
      Pending: "Chờ xác nhận",
      Processing: "Đang xử lý",
      Shipping: "Đang giao",
      Completed: "Hoàn thành",
      Cancelled: "Đã hủy",
      Refund: "Hoàn tiền",
    };
    return texts[status] || status;
  };

  // Helper to derive a friendly display name and email for an order
  const coalesce = (...vals) => {
    for (const v of vals) {
      if (v !== undefined && v !== null && v !== "") return v;
    }
    return null;
  };

  const composeNameFromParts = (obj) => {
    const first = coalesce(
      obj?.firstName,
      obj?.FirstName,
      obj?.first_name,
      obj?.givenName
    );
    const last = coalesce(
      obj?.lastName,
      obj?.LastName,
      obj?.last_name,
      obj?.familyName
    );
    if (first || last)
      return `${(first || "").trim()} ${(last || "").trim()}`.trim();
    return null;
  };

  const beautifyName = (raw) => {
    if (!raw) return null;
    // If it's an email, use local-part
    const emailMatch = raw.match(/^([^@]+)@/);
    let s = emailMatch ? emailMatch[1] : raw;
    // Replace separators with spaces and remove digits at the end of tokens
    s = s.replace(/[_\.\-]+/g, " ").replace(/\d+/g, "");
    // Split tokens and capitalize
    const parts = s
      .split(/\s+/)
      .map((p) => p.trim())
      .filter(Boolean)
      .map((p) => p[0]?.toUpperCase() + p.slice(1).toLowerCase());
    if (parts.length === 0) return null;
    // If result is still single token, and that token length < 3, give up
    const candidate = parts.join(" ");
    if (candidate.length < 2) return null;
    return candidate;
  };

  const getOrderEmail = (order) =>
    coalesce(
      // Prefer normalized field added by backend
      order?.CustomerEmailNormalized,
      order?.customerEmail,
      order?.CustomerEmail,
      order?.email,
      order?.Email,
      order?.user?.email,
      order?.user?.Email,
      order?.contactEmail,
      order?.contact?.email,
      order?.buyer?.email,
      order?.buyer?.emailAddress,
      order?.billing?.email,
      order?.shipping?.email,
      order?.Customer?.Email,
      order?.CustomerEmailAddress,
      null
    ) || "";

  const getOrderName = (order) => {
    // Prefer computed full name from backend if available
    if (order?.CustomerFullName) {
      // beautify if it looks like a username/email
      const cf = order.CustomerFullName;
      if (!/\s+/.test(cf)) {
        const b = beautifyName(cf);
        if (b) return b;
      }
      return cf;
    }
    const nameCandidates = [
      order?.customerName,
      order?.CustomerName,
      order?.customer,
      order?.Customer,
      order?.user?.userName,
      order?.user?.user_name,
      order?.user?.name,
      order?.user?.fullName,
      order?.user?.full_name,
      order?.buyer?.fullName,
      order?.buyer?.name,
      order?.shipping?.FullName,
      order?.shipping?.fullName,
      order?.shipping?.Fullname,
      order?.shipping?.FullName,
    ];

    for (const cand of nameCandidates) {
      if (cand) return cand;
    }

    // try composing from first/last fields in common places
    const composed =
      composeNameFromParts(order) ||
      composeNameFromParts(order?.user) ||
      composeNameFromParts(order?.Customer) ||
      composeNameFromParts(order?.shipping) ||
      composeNameFromParts(order?.buyer);
    if (composed) return composed;

    // fallback to email if present
    const email = getOrderEmail(order);
    if (email) return email;

    return "Guest";
  };

  return (
    <div className="admin-order-management">
      <div className="admin-header">
        <h1>Quản Lý Đơn Hàng</h1>
      </div>

      {/* Statistics Cards */}
      {statistics && (
        <div className="statistics-cards">
          <div className="stat-card">
            <div className="stat-icon" style={{ background: "#1890ff" }}>
              <i className="fas fa-shopping-cart"></i>
            </div>
            <div className="stat-info">
              <h3>{statistics.totalOrders}</h3>
              <p>Tổng đơn hàng</p>
            </div>
          </div>
          <div className="stat-card">
            <div className="stat-icon" style={{ background: "#faad14" }}>
              <i className="fas fa-clock"></i>
            </div>
            <div className="stat-info">
              <h3>{statistics.pendingOrders}</h3>
              <p>Chờ xác nhận</p>
            </div>
          </div>
          <div className="stat-card">
            <div className="stat-icon" style={{ background: "#13c2c2" }}>
              <i className="fas fa-truck"></i>
            </div>
            <div className="stat-info">
              <h3>{statistics.shippingOrders}</h3>
              <p>Đang giao</p>
            </div>
          </div>
          <div className="stat-card">
            <div className="stat-icon" style={{ background: "#52c41a" }}>
              <i className="fas fa-check-circle"></i>
            </div>
            <div className="stat-info">
              <h3>{statistics.completedOrders}</h3>
              <p>Hoàn thành</p>
            </div>
          </div>
          <div className="stat-card revenue">
            <div className="stat-icon" style={{ background: "#722ed1" }}>
              <i className="fas fa-dollar-sign"></i>
            </div>
            <div className="stat-info">
              <h3>{statistics.totalRevenue.toLocaleString("vi-VN")}₫</h3>
              <p>Doanh thu</p>
            </div>
          </div>
        </div>
      )}

      {/* Tabs */}
      <div className="order-tabs">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            className={`tab-btn ${activeTab === tab.id ? "active" : ""}`}
            onClick={() => {
              setActiveTab(tab.id);
              setCurrentPage(1);
            }}
          >
            {tab.label}
          </button>
        ))}
      </div>

      {/* Orders Table */}
      <div className="orders-table-container">
        {loading ? (
          <div className="loading">
            <i className="fas fa-spinner fa-spin"></i> Đang tải...
          </div>
        ) : orders.length === 0 ? (
          <div className="empty-state">
            <i className="fas fa-inbox"></i>
            <p>Không có đơn hàng nào</p>
          </div>
        ) : (
          <>
            <table className="orders-table">
              <thead>
                <tr>
                  <th>Mã đơn</th>
                  <th>Khách hàng</th>
                  <th>Ngày đặt</th>
                  <th>Số lượng</th>
                  <th>Tổng tiền</th>
                  <th>Trạng thái</th>
                  <th>Thanh toán</th>
                  <th>Thao tác</th>
                </tr>
              </thead>
              <tbody>
                {orders.map((order) => {
                  const email = getOrderEmail(order);
                  const name = getOrderName(order);
                  return (
                    <tr key={order.id}>
                      <td className="order-number">
                        #{order.orderNumber || order.id}
                      </td>
                      <td>
                        <div className="customer-info">
                          <strong>{name}</strong>
                          {email && email !== name && <small>{email}</small>}
                        </div>
                      </td>
                      <td>
                        {new Date(order.createdAt).toLocaleDateString("vi-VN")}
                      </td>
                      <td>{order.items?.length || 0} sản phẩm</td>
                      <td className="price">
                        {order.total.toLocaleString("vi-VN")}₫
                      </td>
                      <td>
                        <select
                          className="status-select"
                          style={{ color: getStatusColor(order.status) }}
                          value={order.status}
                          onChange={(e) =>
                            handleStatusChange(order.id, e.target.value)
                          }
                        >
                          <option value="Pending">Chờ xác nhận</option>
                          <option value="Processing">Đang xử lý</option>
                          <option value="Shipping">Đang giao</option>
                          <option value="Completed">Hoàn thành</option>
                          <option value="Cancelled">Đã hủy</option>
                          <option value="Refund">Hoàn tiền</option>
                        </select>
                      </td>
                      <td>
                        <span
                          className={`payment-badge ${order.paymentStatus?.toLowerCase()}`}
                        >
                          {order.paymentStatus}
                        </span>
                      </td>
                      <td>
                        <button
                          className="btn-view"
                          onClick={() => handleViewDetails(order.id)}
                        >
                          <i className="fas fa-eye"></i> Xem
                        </button>
                      </td>
                    </tr>
                  );
                })}
              </tbody>
            </table>

            {/* Pagination */}
            {totalPages > 1 && (
              <div className="pagination">
                <button
                  onClick={() => setCurrentPage((p) => Math.max(1, p - 1))}
                  disabled={currentPage === 1}
                >
                  <i className="fas fa-chevron-left"></i>
                </button>
                <span>
                  Trang {currentPage} / {totalPages}
                </span>
                <button
                  onClick={() =>
                    setCurrentPage((p) => Math.min(totalPages, p + 1))
                  }
                  disabled={currentPage === totalPages}
                >
                  <i className="fas fa-chevron-right"></i>
                </button>
              </div>
            )}
          </>
        )}
      </div>

      {/* Order Details Modal */}
      {showModal && selectedOrder && (
        <div className="modal-overlay" onClick={() => setShowModal(false)}>
          <div className="modal-content" onClick={(e) => e.stopPropagation()}>
            <div className="modal-header">
              <h2>
                Chi tiết đơn hàng #
                {selectedOrder.orderNumber || selectedOrder.id}
              </h2>
              <button className="close-btn" onClick={() => setShowModal(false)}>
                <i className="fas fa-times"></i>
              </button>
            </div>
            <div className="modal-body">
              <div className="order-info-grid">
                <div className="info-item">
                  <label>Khách hàng:</label>
                  <span>{getOrderName(selectedOrder)}</span>
                </div>
                <div className="info-item">
                  <label>Email:</label>
                  <span>{getOrderEmail(selectedOrder) || "N/A"}</span>
                </div>
                <div className="info-item">
                  <label>Số điện thoại:</label>
                  <span>{selectedOrder.shippingPhone || "N/A"}</span>
                </div>
                <div className="info-item">
                  <label>Địa chỉ giao hàng:</label>
                  <span>{selectedOrder.shippingAddress || "N/A"}</span>
                </div>
                <div className="info-item">
                  <label>Ngày đặt:</label>
                  <span>
                    {new Date(selectedOrder.createdAt).toLocaleString("vi-VN")}
                  </span>
                </div>
                <div className="info-item">
                  <label>Trạng thái:</label>
                  <span style={{ color: getStatusColor(selectedOrder.status) }}>
                    {getStatusText(selectedOrder.status)}
                  </span>
                </div>
              </div>

              <h3>Sản phẩm</h3>
              <table className="items-table">
                <thead>
                  <tr>
                    <th>Tên sản phẩm</th>
                    <th>Số lượng</th>
                    <th>Đơn giá</th>
                    <th>Thành tiền</th>
                  </tr>
                </thead>
                <tbody>
                  {selectedOrder.items?.map((item, index) => (
                    <tr key={index}>
                      <td>{item.game?.title || "Unknown"}</td>
                      <td>{item.quantity}</td>
                      <td>{item.unitPrice.toLocaleString("vi-VN")}₫</td>
                      <td>
                        {(item.quantity * item.unitPrice).toLocaleString(
                          "vi-VN"
                        )}
                        ₫
                      </td>
                    </tr>
                  ))}
                </tbody>
                <tfoot>
                  <tr>
                    <td colSpan="3">
                      <strong>Tổng cộng:</strong>
                    </td>
                    <td>
                      <strong>
                        {selectedOrder.total.toLocaleString("vi-VN")}₫
                      </strong>
                    </td>
                  </tr>
                </tfoot>
              </table>
            </div>
          </div>
        </div>
      )}
    </div>
  );
}
