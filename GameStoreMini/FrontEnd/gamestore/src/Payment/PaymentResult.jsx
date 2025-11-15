import React, { useEffect, useState } from "react";
import { useNavigate, useSearchParams } from "react-router-dom";
import { useCart } from "../Cart/CartProvider";
import "../Decorate/PaymentResult.css";

export default function PaymentResult() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const { clearCart } = useCart();
  const [countdown, setCountdown] = useState(7);

  const success = searchParams.get("success") === "true";
  const orderNumber = searchParams.get("OrderNumber");
  const message = searchParams.get("message");
  const code = searchParams.get("code");

  useEffect(() => {
    if (success) {
      clearCart();

      const timer = setInterval(() => {
        setCountdown((prev) => {
          if (prev <= 1) {
            clearInterval(timer);
            // Chuyển hướng đến trang theo dõi đơn hàng với orderNumber
            navigate(`/orders/track?orderNumber=${orderNumber}`, {
              replace: true,
            });
            return 0;
          }
          return prev - 1;
        });
      }, 1000);
      return () => clearInterval(timer);
    }
  }, [success, orderNumber, navigate, clearCart]);

  return (
    <div className="payment-result-container">
      {success ? (
        <div className="payment-result-card">
          <div className="result-icon success">✅</div>
          <h2 className="result-title success">Thanh toán thành công!</h2>
          <p className="order-number">
            Mã đơn hàng: <strong>{orderNumber}</strong>
          </p>
          <div className="countdown-box">
            <p className="countdown-text">
              Chuyển đến trang theo dõi đơn hàng sau{" "}
              <span className="countdown-number">{countdown}</span> giây...
            </p>
          </div>
        </div>
      ) : (
        <div className="payment-result-card">
          <div className="result-icon error">❌</div>
          <h2 className="result-title error">Thanh toán thất bại!</h2>
          <p className="error-message">
            {message || "Đã có lỗi xảy ra trong quá trình thanh toán"}
          </p>
          {code && (
            <p className="error-code">
              Mã lỗi: <strong>{code}</strong>
            </p>
          )}
          <div className="button-group">
            <button
              className="result-button primary"
              onClick={() => navigate("/cart")}
            >
              Quay lại giỏ hàng
            </button>
            <button
              className="result-button secondary"
              onClick={() => navigate("/")}
            >
              Trang chủ
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
