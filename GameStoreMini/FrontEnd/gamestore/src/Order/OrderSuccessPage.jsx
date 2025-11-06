import React, { useEffect, useState } from "react";
import { useLocation, Link, useNavigate } from "react-router-dom";
import OrderAPI from "../API/OrderAPI";
import "../Decorate/OrderSuccess.css";

export default function OrderSuccessPage() {
  const { state } = useLocation();
  const navigate = useNavigate();
  const [order, setOrder] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);
  const [showContent, setShowContent] = useState(false);

  // Hiệu ứng fade-in khi trang load
  useEffect(() => {
    const timer = setTimeout(() => setShowContent(true), 100);
    return () => clearTimeout(timer);
  }, []);

  useEffect(() => {
    async function load() {
      setLoading(true);
      setError(null);
      try {
        if (state?.orderId) {
          console.log("[OrderSuccessPage] Loading order by ID:", state.orderId);
          const orderData = await OrderAPI.getOrder(state.orderId);
          setOrder(orderData);
        } else if (state?.orderNumber && state?.email) {
          console.log(
            "[OrderSuccessPage] Loading order by number:",
            state.orderNumber
          );
          // Sử dụng api client trực tiếp cho track vì OrderAPI chưa có method này
          const { api } = await import("../API/ApiClient");
          const r = await api.post("/orders/track", {
            orderNumber: state.orderNumber,
            email: state.email,
          });
          setOrder(r.data);
        } else {
          console.log(
            "[OrderSuccessPage] No order info in state, showing basic success"
          );
          // Không có orderId hoặc orderNumber, chỉ hiển thị thông báo thành công cơ bản
          setError(null);
        }
      } catch (e) {
        console.error("[OrderSuccessPage] Load order failed:", e);
        console.error("[OrderSuccessPage] Error response:", e.response);
        setError("Không thể tải thông tin đơn hàng. Vui lòng thử lại sau.");
      } finally {
        setLoading(false);
      }
    }

    if (state?.orderId || state?.orderNumber) {
      load();
    } else {
      console.log(
        "[OrderSuccessPage] No order ID/number provided, showing basic success"
      );
      setLoading(false);
    }
  }, [state]);

  if (loading) {
    return (
      <div className="order-success-container">
        <div className="loading-spinner">
          <div className="spinner"></div>
          <p>Đang tải thông tin đơn hàng...</p>
        </div>
      </div>
    );
  }

  // Nếu có lỗi NHƯNG vẫn có orderNumber từ state, hiển thị thông báo thành công
  if (error && !state?.orderNumber && !state?.orderId) {
    return (
      <div className="order-success-container">
        <div className="success-card">
          <div className="error-icon">❌</div>
          <h1 className="error-title">Có lỗi xảy ra</h1>
          <p className="error-message">{error}</p>
          <div className="action-buttons">
            <button onClick={() => navigate("/")} className="btn btn-home">
              🏠 Về trang chủ
            </button>
          </div>
        </div>
      </div>
    );
  }

  return (
    <div className="order-success-container">
      <div
        className="success-card"
        style={{
          opacity: showContent ? 1 : 0,
          transform: showContent ? "translateY(0)" : "translateY(20px)",
          transition: "all 0.5s ease-out",
        }}
      >
        {/* Icon thành công */}
        <div className="success-icon">
          <svg viewBox="0 0 52 52" className="checkmark">
            <circle
              className="checkmark-circle"
              cx="26"
              cy="26"
              r="25"
              fill="none"
            />
            <path
              className="checkmark-check"
              fill="none"
              d="M14.1 27.2l7.1 7.2 16.7-16.8"
            />
          </svg>
        </div>

        {/* Thông báo chính */}
        <h1 className="success-title">🎉 Đặt hàng thành công!</h1>
        <p className="success-subtitle">
          Cảm ơn bạn đã mua hàng tại ĐAM MÊ GAME
        </p>

        {/* Mã đơn hàng */}
        <div className="order-number-section">
          <p className="label">Mã đơn hàng:</p>
          <p className="order-number">
            {state?.orderNumber ??
              order?.orderNumber ??
              order?.OrderNumber ??
              state?.orderId ??
              "Đang cập nhật..."}
          </p>
        </div>

        {/* Chi tiết đơn hàng */}
        {(order || state?.total) && (
          <div className="order-details">
            <div className="detail-row">
              <span className="detail-label">Tổng tiền:</span>
              <span className="detail-value">
                {(
                  order?.total ??
                  order?.Total ??
                  state?.total ??
                  0
                ).toLocaleString("vi-VN")}{" "}
                đ
              </span>
            </div>
            <div className="detail-row">
              <span className="detail-label">Trạng thái:</span>
              <span
                className={`status-badge ${(
                  order?.status ??
                  order?.Status ??
                  "pending"
                ).toLowerCase()}`}
              >
                {order?.status ?? order?.Status ?? "Đang xử lý"}
              </span>
            </div>

            {order?.items && order.items.length > 0 && (
              <div className="items-section">
                <h3>Chi tiết sản phẩm:</h3>
                <ul className="items-list">
                  {order.items.map((i, index) => (
                    <li key={i.id || index} className="item-row">
                      <span className="item-name">
                        {i.gameTitle ??
                          i.productName ??
                          i.Game?.Title ??
                          "Sản phẩm"}
                      </span>
                      <span className="item-quantity">x{i.quantity ?? 1}</span>
                      <span className="item-price">
                        {(
                          (i.unitPrice ?? i.UnitPrice ?? 0) * (i.quantity ?? 1)
                        ).toLocaleString("vi-VN")}{" "}
                        đ
                      </span>
                    </li>
                  ))}
                </ul>
              </div>
            )}
          </div>
        )}

        {/* Nút hành động */}
        <div className="action-buttons">
          <Link to="/orders/track" className="btn btn-track">
            📦 Theo dõi đơn hàng
          </Link>
          <Link to="/store" className="btn btn-continue">
            🛒 Tiếp tục mua sắm
          </Link>
          <button onClick={() => navigate("/")} className="btn btn-home">
            🏠 Về trang chủ
          </button>
        </div>

        {/* Thông tin hỗ trợ */}
        <div className="support-info">
          <p>
            📧 Email xác nhận đã được gửi đến:{" "}
            <strong>{state?.email ?? "email của bạn"}</strong>
          </p>
          <p>
            💬 Cần hỗ trợ? Liên hệ: <a href="tel:0123456789">0123-456-789</a>{" "}
            hoặc{" "}
            <a href="mailto:support@dammegame.com">support@dammegame.com</a>
          </p>
        </div>
      </div>
    </div>
  );
}
