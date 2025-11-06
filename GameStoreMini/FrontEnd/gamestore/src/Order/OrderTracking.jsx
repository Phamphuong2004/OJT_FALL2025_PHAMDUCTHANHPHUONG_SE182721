import React, { useEffect, useState } from "react";
import api from "../API/UserAPI";
export default function OrderTracking() {
  const [order, setOrder] = useState(null);
  const [email, setEmail] = useState("");
  const [orderNumber, setOrderNumber] = useState("");
  const [myOrders, setMyOrders] = useState([]);
  useEffect(() => {
    // try fetch user orders if logged in
    (async () => {
      try {
        // token stored by UserAPI.setAuthToken uses key 'token'
        const token = localStorage.getItem("token");
        if (token) {
          const r = await api.get("/orders"); // protected endpoint
          setMyOrders(r.data || []);
        }
      } catch (e) {
        // ignore if not logged in
      }
    })();
  }, []);

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
              <li key={o.id}>
                <strong>{o.orderNumber ?? o.OrderNumber ?? o.OrderNum}</strong>{" "}
                - {o.status ?? o.Status} - Tổng {o.total ?? o.Total}
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
        </div>
      )}
    </div>
  );
}
