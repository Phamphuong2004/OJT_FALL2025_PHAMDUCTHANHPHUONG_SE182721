import React from "react";
import { useNavigate } from "react-router-dom";
import OrderAPI from "../API/OrderAPI";
import PaymentAPI from "../API/PaymentAPI";

export default function OrderConfirmation({
  cart,
  shipping,
  isGuest /* hoặc lấy từ state */,
}) {
  const navigate = useNavigate();

  async function placeOrder() {
    try {
      const payload = {
        AnonymousId: localStorage.getItem("anonCartId") || null,
        CustomerName: shipping.name,
        CustomerEmail: shipping.email,
        Shipping: shipping,
        Items: cart.items.map((i) => ({
          GameId: i.gameId,
          Quantity: i.quantity,
        })),
        Total: cart.total || 0,
      };

      // Use OrderAPI which will call authenticated or guest endpoints accordingly
      const res = await OrderAPI.createOrder(payload);
      // backend may return orderNumber in res
      const orderNumber =
        res?.orderNumber || res?.OrderNumber || res?.order?.orderNumber;

      // Simulate payment flow (development): immediately confirm payment
      try {
        await PaymentAPI.confirmPayment({ OrderNumber: orderNumber });
      } catch (e) {
        console.warn("Payment confirm failed (dev)", e);
      }

      navigate("/order/success", {
        state: { orderNumber, email: shipping.email },
      });
    } catch (err) {
      console.error("Checkout failed", err);
      const status = err?.response?.status;
      if (status === 401) {
        // Save the pending order and redirect to login so user can re-authenticate.
        localStorage.setItem("pendingOrder", JSON.stringify(payload));
        // Optionally show a message then redirect to login page
        window.location.href = "/login";
        return;
      }
      // show toast / validation errors: if err.response.data.errors show them
      const msg = err.response?.data?.errors
        ? JSON.stringify(err.response.data.errors)
        : err.response?.data?.title || "Đặt hàng thất bại";
      alert(msg);
    }
  }

  return (
    <div>
      {/* ...summary UI... */}
      <button onClick={placeOrder}>Đặt hàng</button>
    </div>
  );
}
