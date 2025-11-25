import React, { useState, useEffect } from "react";
import { Link } from "react-router-dom";
import OrderAPI from "../API/OrderAPI";
import { api } from "../API/ApiClient";
import "../Decorate/MyOrders.css";
import formatCurrency from "../Utils/formatCurrency";

const MyOrders = () => {
  const [activeTab, setActiveTab] = useState("all");
  const [orders, setOrders] = useState([]);
  const [loading, setLoading] = useState(false);
  const [cancelLoading, setCancelLoading] = useState({});
  const [searchQuery, setSearchQuery] = useState("");

  const tabs = [
    { id: "all", label: "Tất cả", count: 0 },
    { id: "pending", label: "Chờ xác nhận", count: 0 },
    { id: "processing", label: "Đang xử lý", count: 0 },
    { id: "shipping", label: "Đang giao", count: 0 },
    { id: "completed", label: "Hoàn thành", count: 0 },
    { id: "cancelled", label: "Đã hủy", count: 0 },
    { id: "refund", label: "Trả hàng/Hoàn tiền", count: 0 },
  ];

  useEffect(() => {
    fetchOrders();
  }, [activeTab]);

  const fetchOrders = async () => {
    setLoading(true);
    try {
      console.log("[MyOrders] Fetching orders...");
      const data = await OrderAPI.getMyOrders();
      console.log("[MyOrders] Orders received:", data);
      console.log("[MyOrders] Orders count:", data?.length || 0);
      // normalize statuses and attach a `normalizedStatus` key so frontend
      // can filter consistently even if backend uses slightly different labels
      const normalizeStatus = (s) => {
        if (!s) return "";
        const st = s.toString().toLowerCase();
        // Detect cancelled variants in English and Vietnamese (e.g. "cancelled", "canceled", "đã hủy", "hủy")
        if (st.includes("cancel") || st.includes("hủy") || st.includes("huy"))
          return "cancelled";
        // Map backend variants to our canonical keys used by tabs
        if (st === "confirmed") return "processing"; // treat Confirmed as Processing
        if (st === "processing") return "processing";
        if (st === "pending") return "pending";
        if (st === "shipping") return "shipping";
        if (st === "completed") return "completed";
        if (st === "refund" || st === "refunded") return "refund";
        return st; // fallback to raw lowercased status
      };

      let normalized = (data || []).map((o) => ({
        ...o,
        normalizedStatus: normalizeStatus(o.status || o.Status),
      }));

      // By default (activeTab === 'all') hide cancelled orders to avoid
      // showing cancelled items in the main list unless user selects the
      // 'Đã hủy' tab explicitly.
      let filteredOrders = normalized;
      if (activeTab === "all") {
        filteredOrders = normalized.filter(
          (order) => order.normalizedStatus !== "cancelled"
        );
      } else {
        filteredOrders = normalized.filter(
          (order) => order.normalizedStatus === activeTab
        );
      }

      console.log("[MyOrders] Filtered orders count:", filteredOrders.length);
      setOrders(filteredOrders);
    } catch (error) {
      console.error("[MyOrders] Failed to fetch orders:", error);
      console.error("[MyOrders] Error response:", error.response);
      setOrders([]);
    } finally {
      setLoading(false);
    }
  };

  // Cancel an order (authenticated)
  const handleCancelOrder = async (orderId) => {
    if (!orderId) return;
    if (!confirm("Bạn có chắc chắn muốn hủy đơn này?")) return;
    setCancelLoading((s) => ({ ...s, [orderId]: true }));
    try {
      // call backend cancel endpoint (authenticated)
      const res = await api.post(`/orders/${orderId}/cancel`);
      console.log("[MyOrders] Cancel response:", res.data);

      // Optimistically remove the cancelled order from UI so it disappears immediately
      setOrders((prev) => prev.filter((o) => Number(o.id) !== Number(orderId)));

      // Refresh list in background to ensure consistency
      fetchOrders().catch((e) =>
        console.error("Refresh after cancel failed", e)
      );

      alert(res.data?.message || "Đã hủy đơn");
    } catch (err) {
      console.error("[MyOrders] Cancel failed", err);
      const msg = err?.response?.data?.message || "Hủy đơn thất bại";
      alert(msg);
    } finally {
      setCancelLoading((s) => ({ ...s, [orderId]: false }));
    }
  };

  const getStatusColor = (status) => {
    const colors = {
      pending: "#faad14",
      processing: "#1890ff",
      shipping: "#13c2c2",
      completed: "#52c41a",
      cancelled: "#f5222d",
      refund: "#eb2f96",
    };
    return colors[status?.toLowerCase()] || "#666";
  };

  const getStatusText = (status) => {
    const texts = {
      pending: "Chờ xác nhận",
      processing: "Đang xử lý",
      shipping: "Đang giao",
      completed: "Hoàn thành",
      cancelled: "Đã hủy",
      refund: "Hoàn tiền",
    };
    return texts[status?.toLowerCase()] || status;
  };

  const filteredOrders = orders.filter(
    (order) =>
      order.orderNumber?.toLowerCase().includes(searchQuery.toLowerCase()) ||
      order.items?.some((item) =>
        item.gameName?.toLowerCase().includes(searchQuery.toLowerCase())
      )
  );

  return (
    <div className="my-orders-container">
      {/* Header */}
      <div className="orders-header">
        <div className="header-user">
          <i className="fas fa-user-circle"></i>
          <span>Tài Khoản Của Tôi</span>
        </div>
        <div className="header-breadcrumb">
          <Link to="/account">Tài khoản</Link>
          <i className="fas fa-chevron-right"></i>
          <span>Đơn Mua</span>
        </div>
      </div>

      {/* Tabs */}
      <div className="orders-tabs">
        {tabs.map((tab) => (
          <button
            key={tab.id}
            className={`tab-item ${activeTab === tab.id ? "active" : ""}`}
            onClick={() => setActiveTab(tab.id)}
          >
            {tab.label}
            {tab.count > 0 && <span className="tab-count">({tab.count})</span>}
          </button>
        ))}
      </div>

      {/* Search */}
      <div className="orders-search">
        <div className="search-box">
          <i className="fas fa-search"></i>
          <input
            type="text"
            placeholder="Bạn có thể tìm kiếm theo tên Game, Mã đơn hàng hoặc Tên Sản phẩm"
            value={searchQuery}
            onChange={(e) => setSearchQuery(e.target.value)}
          />
        </div>
      </div>

      {/* Orders List */}
      <div className="orders-content">
        {loading ? (
          <div className="loading-state">
            <i className="fas fa-spinner fa-spin"></i>
            <p>Đang tải đơn hàng...</p>
          </div>
        ) : filteredOrders.length === 0 ? (
          <div className="empty-state">
            <img src="/empty-order.png" alt="No orders" />
            <p>Chưa có đơn hàng nào</p>
          </div>
        ) : (
          <div className="orders-list">
            {filteredOrders.map((order) => {
              // Debug: Log toàn bộ order data
              console.log("[MyOrders] Rendering order:", order);
              console.log("[MyOrders] Order.total:", order.total);
              console.log("[MyOrders] Order.items:", order.items);

              // Tính lại total từ items nếu order.total = 0
              let calculatedTotal = order.total || 0;
              if (calculatedTotal === 0 && order.items?.length > 0) {
                calculatedTotal = order.items.reduce((sum, item) => {
                  const price = item.unitPrice || item.game?.price || 0;
                  const quantity = item.quantity || 1;
                  return sum + price * quantity;
                }, 0);
                console.log(
                  "[MyOrders] Calculated total from items:",
                  calculatedTotal
                );
              }

              return (
                <div key={order.id || order.orderId} className="order-card">
                  {/* Order Header */}
                  <div className="order-header">
                    <div className="order-shop">
                      <i className="fas fa-store"></i>
                      <span className="shop-name">ĐAM MÊ GAME</span>
                      <button className="chat-btn">
                        <i className="fas fa-comment-dots"></i>
                        Chat
                      </button>
                    </div>
                    <div className="order-status">
                      <span
                        className="status-badge"
                        style={{
                          color: getStatusColor(
                            order.normalizedStatus || order.status
                          ),
                        }}
                      >
                        {getStatusText(order.normalizedStatus || order.status)}
                      </span>
                    </div>
                  </div>

                  {/* Order Items */}
                  <div className="order-items">
                    {order.items?.map((item, index) => {
                      // Debug logging
                      console.log(`[MyOrders] Item ${index}:`, item);
                      console.log(`[MyOrders] Item unitPrice:`, item.unitPrice);
                      console.log(
                        `[MyOrders] Item game.price:`,
                        item.game?.price
                      );

                      // Tính giá - ưu tiên unitPrice, fallback về game.price
                      const price = item.unitPrice || item.game?.price || 0;
                      const quantity = item.quantity || 1;
                      const totalPrice = price * quantity;

                      console.log(
                        `[MyOrders] Calculated price:`,
                        price,
                        `quantity:`,
                        quantity,
                        `total:`,
                        totalPrice
                      );

                      return (
                        <div key={item.id || index} className="order-item">
                          <div className="item-image">
                            <img
                              src={
                                item.game?.imageUrl ||
                                item.gameImage ||
                                "/default-game.png"
                              }
                              alt={item.game?.title || item.gameName || "Game"}
                            />
                          </div>
                          <div className="item-info">
                            <h4 className="item-name">
                              {item.game?.title ||
                                item.gameName ||
                                "Unknown Game"}
                            </h4>
                            <p className="item-desc">
                              Phân loại hàng:{" "}
                              {item.game?.category ||
                                item.category ||
                                "Standard Edition"}
                            </p>
                            <p className="item-quantity">x{quantity}</p>
                          </div>
                          <div className="item-price">
                            <span className="sale-price">
                              {formatCurrency(totalPrice)}
                            </span>
                          </div>
                        </div>
                      );
                    })}
                  </div>

                  {/* Order Footer */}
                  <div className="order-footer">
                    <div className="order-info">
                      <span className="order-number">
                        Mã đơn: {order.orderNumber || order.id}
                      </span>
                      <span className="order-date">
                        {new Date(order.createdAt).toLocaleDateString("vi-VN")}
                      </span>
                    </div>
                    <div className="order-total">
                      <span className="total-label">Thành tiền:</span>
                      <span className="total-amount">
                        {formatCurrency(calculatedTotal)}
                      </span>
                    </div>
                    <div className="order-actions">
                      {order.normalizedStatus === "completed" && (
                        <>
                          <button className="btn-secondary">Mua Lại</button>
                          <button className="btn-secondary">
                            Liên Hệ Người Bán
                          </button>
                        </>
                      )}
                      {order.normalizedStatus === "pending" && (
                        <button
                          className="btn-danger"
                          onClick={() => handleCancelOrder(order.id)}
                          disabled={!!cancelLoading[order.id]}
                        >
                          {cancelLoading[order.id]
                            ? "Đang hủy..."
                            : "Hủy Đơn Hàng"}
                        </button>
                      )}
                      {order.normalizedStatus === "shipping" && (
                        <button className="btn-success">
                          Đã Nhận Được Hàng
                        </button>
                      )}
                      <Link to={`/order/${order.id}`} className="btn-primary">
                        Xem Chi Tiết
                      </Link>
                    </div>
                  </div>

                  {/* Delivery Status */}
                  {order.status === "shipping" && (
                    <div className="delivery-status">
                      <i className="fas fa-truck"></i>
                      <span>Giao hàng thành công</span>
                      <span className="delivery-date">
                        Đánh giá sản phẩm trước 02-12-2025
                      </span>
                    </div>
                  )}
                </div>
              );
            })}
          </div>
        )}
      </div>
    </div>
  );
};

export default MyOrders;
