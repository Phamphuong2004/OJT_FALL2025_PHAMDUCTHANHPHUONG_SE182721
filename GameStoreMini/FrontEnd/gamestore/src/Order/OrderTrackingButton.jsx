import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { trackOrder } from "../API/OrderAPI";
import "../Decorate/OrderTrackingButton.css";

const OrderTrackingButton = () => {
  const [showModal, setShowModal] = useState(false);
  const [orderCode, setOrderCode] = useState("");
  const [email, setEmail] = useState("");
  const [error, setError] = useState("");
  const [loading, setLoading] = useState(false); // Thêm state loading
  const navigate = useNavigate();

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError("");
    setLoading(true); // Bắt đầu loading
    try {
      const result = await trackOrder({ orderCode, email });
      if (!result.success) throw new Error(result.message);
      navigate(`/orders/track?code=${orderCode}&email=${email}`);
      setShowModal(false);
    } catch (err) {
      setError(err.message || "Không thể kiểm tra đơn hàng. Vui lòng thử lại.");
    } finally {
      setLoading(false); // Kết thúc loading
    }
  };

  return (
    <>
      <button
        className="order-tracking-button-modern"
        onClick={() => setShowModal(true)}
        style={{
          background: "linear-gradient(90deg, #6a5af9 0%, #7b7bf9 100%)",
          color: "#fff",
          border: "none",
          borderRadius: "24px",
          padding: "8px 20px",
          fontWeight: 600,
          fontSize: "16px",
          boxShadow: "0 2px 8px rgba(106,90,249,0.12)",
          display: "flex",
          alignItems: "center",
          gap: "8px",
          cursor: "pointer",
          transition: "box-shadow 0.2s, transform 0.2s",
        }}
        onMouseEnter={(e) =>
          (e.currentTarget.style.boxShadow = "0 4px 16px rgba(106,90,249,0.18)")
        }
        onMouseLeave={(e) =>
          (e.currentTarget.style.boxShadow = "0 2px 8px rgba(106,90,249,0.12)")
        }
      >
        <span
          className="order-track-icon"
          style={{ display: "flex", alignItems: "center" }}
        >
          <svg width="22" height="22" fill="none" viewBox="0 0 24 24">
            <rect
              x="3"
              y="6"
              width="18"
              height="12"
              rx="4"
              fill="#fff"
              opacity="0.12"
            />
            <path
              d="M3 6h18M3 6l1.5 14h15L21 6M8 10v6M12 10v6M16 10v6"
              stroke="#fff"
              strokeWidth="1.5"
              strokeLinecap="round"
            />
          </svg>
        </span>
        <span
          className="order-track-text"
          style={{ fontSize: "16px", fontWeight: 500 }}
        >
          Theo dõi đơn hàng
        </span>
      </button>
      {showModal && (
        <div className="order-track-modal">
          <form className="order-track-form" onSubmit={handleSubmit}>
            <h3>Kiểm tra đơn hàng</h3>
            <input
              type="text"
              placeholder="Mã đơn hàng"
              value={orderCode}
              onChange={(e) => setOrderCode(e.target.value)}
              required
              disabled={loading}
            />
            <input
              type="email"
              placeholder="Email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              disabled={loading}
            />
            {error && <div className="error">{error}</div>}
            <button type="submit" disabled={loading}>
              {loading ? "Đang kiểm tra..." : "Kiểm tra"}
            </button>
            <button
              type="button"
              onClick={() => setShowModal(false)}
              disabled={loading}
              style={{ marginLeft: 8 }}
            >
              Đóng
            </button>
          </form>
        </div>
      )}
    </>
  );
};

export default OrderTrackingButton;
