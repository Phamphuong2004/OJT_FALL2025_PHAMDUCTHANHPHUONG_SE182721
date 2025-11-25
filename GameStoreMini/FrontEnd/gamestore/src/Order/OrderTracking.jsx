import React, { useEffect, useState } from "react";
import { api } from "../API/ApiClient";
import { useSearchParams } from "react-router-dom";

export default function OrderTracking() {
  const [searchParams] = useSearchParams();
  const [order, setOrder] = useState(null);
  const [email, setEmail] = useState("");
  const [orderNumber, setOrderNumber] = useState("");
  const [myOrders, setMyOrders] = useState([]);
  const [cancelLoading, setCancelLoading] = useState({});

  useEffect(() => {
    // Tự động load order nếu có orderNumber trong URL (từ payment redirect)
    const urlOrderNumber = searchParams.get("orderNumber");
    if (urlOrderNumber) {
      setOrderNumber(urlOrderNumber);
      // Tự động load order detail cho user đã đăng nhập
      (async () => {
        try {
          const token = localStorage.getItem("token");
          if (token) {
            const r = await api.get(`/orders/${urlOrderNumber}`);
            setOrder(r.data);
          }
        } catch (e) {
          console.error("Auto load order failed", e);
        }
      })();
    }
  }, [searchParams]);

  useEffect(() => {
    // try fetch user orders if logged in
    (async () => {
      try {
        // token stored by UserAPI.setAuthToken uses key 'token'
        const token = localStorage.getItem("token");
        if (token) {
          const r = await api.get("/orders"); // protected endpoint
          // Filter out cancelled orders so tracked list doesn't show them by default
          const data = r.data || [];
          const visible = data.filter((o) => {
            const st = (o.status || o.Status || "").toString().toLowerCase();
            // hide english and vietnamese cancelled variants ("cancel", "canceled", "đã hủy", "hủy")
            if (
              st.includes("cancel") ||
              st.includes("hủy") ||
              st.includes("huy")
            )
              return false;
            return true;
          });
          setMyOrders(visible);
        }
      } catch (e) {
        // ignore if not logged in
      }
    })();
  }, []);

  // Refresh user's orders (use after cancel)
  async function refreshMyOrders() {
    try {
      const token = localStorage.getItem("token");
      if (token) {
        const r = await api.get("/orders");
        const data = r.data || [];
        const visible = data.filter(
          (o) =>
            (o.status || o.Status || "").toString().toLowerCase() !==
            "cancelled"
        );
        setMyOrders(visible);
      }
    } catch (err) {
      console.error("refreshMyOrders failed", err);
    }
  }

  // Cancel an order as authenticated user by order id
  async function handleCancelAuthenticated(orderId) {
    if (!orderId) return;
    if (!confirm("Bạn có chắc chắn muốn hủy đơn này?")) return;
    setCancelLoading((s) => ({ ...s, [orderId]: true }));
    try {
      const res = await api.post(`/orders/${orderId}/cancel`);
      alert(res.data?.message || "Đã hủy đơn");
      // Optimistically remove from myOrders so it disappears immediately
      setMyOrders((prev) =>
        prev.filter((o) => Number(o.id) !== Number(orderId))
      );
      // Refresh in background to keep state consistent
      refreshMyOrders().catch((e) =>
        console.error("Refresh after cancel failed", e)
      );
      // If currently viewing this order, reload detail
      const currentId = order?.id ?? order?.Id;
      if (currentId && Number(currentId) === Number(orderId)) {
        try {
          const r = await api.get(`/orders/${orderId}`);
          setOrder(r.data);
        } catch (e) {
          // ignore
        }
      }
    } catch (err) {
      console.error("Cancel failed", err);
      const msg = err?.response?.data?.message || "Hủy đơn thất bại";
      alert(msg);
    } finally {
      setCancelLoading((s) => ({ ...s, [orderId]: false }));
    }
  }

  // Cancel as guest by orderNumber + email
  async function handleGuestCancel(e) {
    if (e && e.preventDefault) e.preventDefault();
    if (!orderNumber || !email) {
      alert("Vui lòng nhập mã đơn và email để hủy.");
      return;
    }
    if (!confirm("Bạn có chắc chắn muốn hủy đơn (guest)?")) return;
    setCancelLoading((s) => ({ ...s, guest: true }));
    try {
      const res = await api.post("/orders/cancel", { orderNumber, email });
      alert(res.data?.message || "Đã hủy đơn");
      setOrder(null);
    } catch (err) {
      console.error("Guest cancel failed", err);
      const msg = err?.response?.data?.message || "Hủy đơn thất bại";
      alert(msg);
    } finally {
      setCancelLoading((s) => ({ ...s, guest: false }));
    }
  }

  async function handleGuestTrack(e) {
    e?.preventDefault();
    try {
      const res = await api.post("/orders/track", { orderNumber, email });
      setOrder(res.data);
    } catch (e) {
      console.error("Track order failed", e);
      // show backend error message if available
      const msg =
        e?.response?.data?.title ||
        e?.response?.data ||
        "Không tìm thấy đơn hàng với thông tin đã cung cấp.";
      alert(msg);
      setOrder(null);
    }
  }

  return (
    <div>
      <h2>Theo dõi đơn hàng</h2>

      {myOrders.length > 0 && (
        <>
          <h3>Đơn hàng của bạn</h3>
          <ul>
            {myOrders.map((o) => (
              <li key={o.id} style={{ marginBottom: 8 }}>
                <strong>{o.orderNumber ?? o.OrderNumber ?? o.OrderNum}</strong>{" "}
                - {o.status ?? o.Status} - Tổng {o.total ?? o.Total}
                <div style={{ display: "inline-block", marginLeft: 12 }}>
                  <button
                    onClick={() => handleCancelAuthenticated(o.id)}
                    disabled={!!cancelLoading[o.id]}
                    style={{ marginLeft: 8 }}
                  >
                    {cancelLoading[o.id] ? "Đang hủy..." : "Hủy đơn"}
                  </button>
                </div>
              </li>
            ))}
          </ul>
        </>
      )}

      <hr />
      <h3>Tra cứu cho khách</h3>
      <form onSubmit={handleGuestTrack}>
        <div>
          <label>Mã đơn</label>
          <input
            value={orderNumber}
            onChange={(e) => setOrderNumber(e.target.value)}
            required
          />
        </div>
        <div>
          <label>Email</label>
          <input
            value={email}
            onChange={(e) => setEmail(e.target.value)}
            type="email"
            required
          />
        </div>
        <button type="submit">Tra cứu</button>
        <button
          type="button"
          onClick={handleGuestCancel}
          disabled={!!cancelLoading.guest}
          style={{ marginLeft: 8 }}
        >
          {cancelLoading.guest ? "Đang hủy..." : "Hủy đơn (guest)"}
        </button>
      </form>

      {order && (
        <div>
          <h4>Đơn {order.orderNumber ?? order.OrderNumber}</h4>
          <p>Trạng thái: {order.status ?? order.Status}</p>
          <ul>
            {order.items?.map((i) => (
              <li key={i.id || i.gameId}>
                {i.gameTitle ?? i.Game?.Title} x{i.quantity}
              </li>
            ))}
          </ul>
          <div style={{ marginTop: 12 }}>
            <button
              onClick={() => {
                const id = order.id ?? order.Id;
                if (id) handleCancelAuthenticated(id);
                else handleGuestCancel();
              }}
              disabled={
                !!cancelLoading[order.id ?? order.Id] || !!cancelLoading.guest
              }
            >
              {cancelLoading[order.id ?? order.Id] || cancelLoading.guest
                ? "Đang hủy..."
                : "Hủy đơn"}
            </button>
          </div>
        </div>
      )}
    </div>
  );
}
